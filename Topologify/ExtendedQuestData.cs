using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topologify
{
    public class ExtendedQuestData
    {
        public int ID { get; set; }
        public int Category { get; set; }
        public bool Recurring { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public List<int> Prerequisite { get; set; }
        public string WikiID { get; set; }

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
            ExtendedQuestData data = new ExtendedQuestData
            {
                ID = (int) json.id,
                Category = (int) json.category,
                Recurring = (int) json.type != 4,
                Title = (string) json.title,
                Detail = ((string) json.detail).Replace("<br>", "\n"),
                Prerequisite = new List<int>(),
                WikiID = (string) json.wiki_id,
                Completed = Status.Unknown
            };
            foreach (var i in json.prerequisite)
            {
                data.Prerequisite.Add((int) i);
            }

            return data;
        }
    }
}
