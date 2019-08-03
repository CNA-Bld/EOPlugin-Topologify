using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topologify
{
    public class ExtendedQuestData
    {
        private dynamic RawData { get; set; }
        public int ID => (int) RawData.game_id;
        public int Category => (int) RawData.category;
        public bool Recurring => (int) RawData.type != 1;  
        public string Title => (string) RawData.name;
        public string Detail => ((string) RawData.detail).Replace("<br>", "\n");
        public List<int> Prerequisite => ((int[]) RawData.prerequisite).ToList();
        public string WikiID => (string) RawData.wiki_id;

        public enum Status
        {
            Unknown,
            DerivedCompleted,
            DerivedUncompleted,
            MarkedCompleted,
            MarkedTreeCompleted,
            AggressiveDerivedCompleted,
            AggressiveMarkedCompleted
        }

        public Status Completed { get; set; }

        public bool IsCompleted
            =>
                Completed == Status.DerivedCompleted || Completed == Status.MarkedCompleted ||
                Completed == Status.MarkedTreeCompleted || Completed == Status.AggressiveDerivedCompleted ||
                Completed == Status.AggressiveMarkedCompleted;

        public bool IsDerivedCompleted
            => Completed == Status.AggressiveDerivedCompleted || Completed == Status.DerivedCompleted;

        public bool IsAggressive
            => Completed == Status.AggressiveDerivedCompleted || Completed == Status.AggressiveMarkedCompleted;

        public bool IsManual
            =>
                Completed == Status.AggressiveMarkedCompleted || Completed == Status.MarkedCompleted ||
                Completed == Status.MarkedTreeCompleted;

        public string Description => $"({ID}) {WikiID} {Title}";

        public static ExtendedQuestData FromJson(dynamic json)
        {
            return new ExtendedQuestData
            {
                RawData = json,
                Completed = Status.Unknown
            };
        }
    }
}
