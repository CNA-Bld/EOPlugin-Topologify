using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codeplex.Data;
using ElectronicObserver.Data;
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

		private static string JSON_PATH = @"Data\quest.json";
		private static Uri JSON_URL = new Uri("https://kcwikizh.github.io/kcdata/quest/all.json");
		private static string RECORD_PATH = @"Record\Topologify.json";

		public Topologify()
	    {
		    LoadData();
			LoadRecord();
	    }

		public Dictionary<int, ExtendedQuestData> Quests = new Dictionary<int, ExtendedQuestData>();

		public void LoadData()
		{
			if (!Directory.Exists("Data"))
				Directory.CreateDirectory("Data");
			if (File.Exists(JSON_PATH))
			{
				var json = DynamicJson.Parse(File.ReadAllText(JSON_PATH));

				Quests.Clear();
				foreach (var quest in json)
				{
					ExtendedQuestData questData = ExtendedQuestData.FromJson(quest);
					Quests.Add(questData.ID, questData);
				}
			}
		}

	    public Record LoadRecord()
	    {
		    if (File.Exists(RECORD_PATH))
			    return DynamicJson.Parse(File.ReadAllText(RECORD_PATH)).Deserialize<Record>();
		    else
				return new Record();
	    }

	    public void SaveRecord()
	    {
			var RecordData = new Record();
		    RecordData.DerivedCompleted =
			    Quests.Values.Where(q => q.Completed == ExtendedQuestData.Status.DerivedCompleted).Select(q => q.ID).ToList();
		    RecordData.MarkedCompleted =
			    Quests.Values.Where(q => q.Completed == ExtendedQuestData.Status.MarkedCompleted).Select(q => q.ID).ToList();
			File.WriteAllText(RECORD_PATH, DynamicJson.Serialize(RecordData));
	    }
    }
}
