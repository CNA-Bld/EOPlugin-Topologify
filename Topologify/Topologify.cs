using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Plugins;

namespace Topologify
{
    public class Topologify : DialogPlugin
    {
        public override string MenuTitle => "Topologify";

        public override Form GetToolWindow()
        {
            return new ToolForm(this);
        }

        public override string Version => "<BUILD_VERSION>";

        private static string JSON_PATH = @"Data\quest.json";
        private static Uri JSON_URL = new Uri("https://kcwikizh.github.io/kcdata/quest/poi.json");
        private static string RECORD_PATH = @"Record\Topologify.json";

        public Topologify()
        {
            LoadData();
            APIObserver.Instance["api_req_quest/clearitemget"].RequestReceived += OnQuestCompleted;
        }

        private void OnQuestCompleted(string apiname, dynamic data)
        {
            int id = int.Parse(data["api_quest_id"]);
            if (Quests.ContainsKey(id))
            {
                var quest = Quests[id];
                if (!quest.Recurring)
                {
                    var recordData = LoadRecord();
                    recordData.DerivedCompleted.Add(id);
                    File.WriteAllText(RECORD_PATH, DynamicJson.Serialize(recordData));
                }
            }
        }

        public readonly Dictionary<int, ExtendedQuestData> Quests = new Dictionary<int, ExtendedQuestData>();

        private void LoadData()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            if (!File.Exists(JSON_PATH)) return;

            try
            {
                var json = DynamicJson.Parse(File.ReadAllText(JSON_PATH));

                Quests.Clear();
                foreach (var quest in json)
                {
                    ExtendedQuestData questData = ExtendedQuestData.FromJson(quest);
                    Quests.Add(questData.ID, questData);
                }
            }
            catch (Exception e)
            {
                Logger.Add(2, $"Topologify: Failed to load quests JSON. Please try update data. ({e})");
            }
        }

        public Record LoadRecord()
        {
            if (File.Exists(RECORD_PATH))
                return DynamicJson.Parse(File.ReadAllText(RECORD_PATH)).Deserialize<Record>();
            return new Record();
        }

        public void SaveRecord()
        {
            var recordData = new Record();
            recordData.DerivedCompleted = BuildQuestIds(ExtendedQuestData.Status.DerivedCompleted);
            recordData.MarkedCompleted = BuildQuestIds(ExtendedQuestData.Status.MarkedCompleted);
            recordData.AggressiveDerivedCompleted = BuildQuestIds(ExtendedQuestData.Status.AggressiveDerivedCompleted);
            recordData.AggressiveMarkedCompleted = BuildQuestIds(ExtendedQuestData.Status.AggressiveMarkedCompleted);
            File.WriteAllText(RECORD_PATH, DynamicJson.Serialize(recordData));
        }

        private List<int> BuildQuestIds(ExtendedQuestData.Status status)
        {
            return Quests.Values.Where(q => q.Completed == status).Select(q => q.ID).ToList();
        }

        public void UpdateData(Action action)
        {
            var client = new WebClient {Encoding = Encoding.UTF8};
            client.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ErrorReporter.SendErrorReport(e.Error, "Error when updating data.");
                    return;
                }

                LoadData();
                action();
            };
            client.DownloadFileAsync(JSON_URL, JSON_PATH);
        }
    }
}
