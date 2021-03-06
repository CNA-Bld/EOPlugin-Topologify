﻿using System;
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
            if (Quests.ContainsKey(id) && Quests[id].Recurring)
                return;

            // If DB does not know about the quest yet, we still remember it.
            // (even if it's recurring - we would ignore it later when loading.)

            var recordData = LoadRecord();
            recordData.DerivedCompleted.Add(id);
            File.WriteAllText(RECORD_PATH, DynamicJson.Serialize(recordData));
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
            var recordData = LoadRecord();
            recordData.DerivedCompleted =
                UpdateIds(ExtendedQuestData.Status.DerivedCompleted, recordData.DerivedCompleted);
            recordData.MarkedCompleted =
                UpdateIds(ExtendedQuestData.Status.MarkedCompleted, recordData.MarkedCompleted);
            recordData.AggressiveDerivedCompleted =
                UpdateIds(ExtendedQuestData.Status.AggressiveDerivedCompleted, recordData.AggressiveDerivedCompleted);
            recordData.AggressiveMarkedCompleted =
                UpdateIds(ExtendedQuestData.Status.AggressiveMarkedCompleted, recordData.AggressiveMarkedCompleted);
            File.WriteAllText(RECORD_PATH, DynamicJson.Serialize(recordData));
        }

        private List<int> UpdateIds(ExtendedQuestData.Status status, List<int> current)
        {
            return Quests.Values.Where(q => q.Completed == status).Select(q => q.ID).ToList()
                .Concat(current).Distinct().ToList(); // If DB does not know, keep.
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
