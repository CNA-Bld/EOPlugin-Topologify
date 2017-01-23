using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codeplex.Data;
using ElectronicObserver.Data;

namespace Topologify
{
	public partial class ToolForm : Form
	{
		private Topologify plugin;
		
		public ToolForm(Topologify plugin)
		{
			InitializeComponent();
			this.plugin = plugin;
		}

		private void ToolForm_Load(object sender, EventArgs e)
		{
			if (!KCDatabase.Instance.Quest.IsLoadCompleted)
			{
				MessageBox.Show("Please load all quests by visiting all pages first.");
				Close();
				return;
			}

			TryTopologify();
			RefreshView();
		}

		private void RefreshView()
		{
			dataGridView.Rows.Clear();

			foreach (var ext in plugin.Quests.Values)
			{
				if (ext.Recurring || ext.isCompleted)
					continue;

				var row = new DataGridViewRow();
				row.CreateCells(dataGridView);

				row.Cells[ColumnDisplayed.Index].Value = KCDatabase.Instance.Quest.Quests.ContainsKey(ext.ID);
				row.Cells[ColumnId.Index].Value = ext.ID;
				row.Cells[ColumnWikiId.Index].Value = ext.WikiID;
				row.Cells[ColumnCategory.Index].Value = ext.Category;
				row.Cells[ColumnTitle.Index].Value = ext.Title;

				var tree = BuildPrerequisiteTree(ext);
				row.Cells[ColumnTitle.Index].ToolTipText = ext.Description + "\n" + ext.Detail + "\n\n" + string.Join("\n", tree.Select(BuildDescription));

				dataGridView.Rows.Add(row);
			}

			dataGridView.Sort(ColumnWikiId, ListSortDirection.Ascending);
		}

		private string BuildDescription(ExtendedQuestData quest)
		{
			return quest.Description + (KCDatabase.Instance.Quest.Quests.ContainsKey(quest.ID) ? " <=" : "");
		}

		private List<ExtendedQuestData> BuildPrerequisiteTree(ExtendedQuestData quest)
		{
			List<ExtendedQuestData> prerequisiteQuests = new List<ExtendedQuestData>();
			foreach (var pre in quest.Prerequisite.Select(i => plugin.Quests[i]))
			{
				if (pre.isCompleted)
					continue;
				List<ExtendedQuestData> thisPreStrs = new List<ExtendedQuestData>();
				if (!KCDatabase.Instance.Quest.Quests.ContainsKey(pre.ID))
					thisPreStrs.AddRange(BuildPrerequisiteTree(pre));
				thisPreStrs.Add(pre);
				prerequisiteQuests.AddRange(thisPreStrs);
			}
			return prerequisiteQuests.Distinct().ToList();
		}

		private void MarkQuestTree(ExtendedQuestData quest, ExtendedQuestData.Status status)
		{
			if (quest.isCompleted)
				return;

			if (!quest.Recurring)
				quest.Completed = status;

			quest.Prerequisite.ForEach(q => MarkQuestTree(plugin.Quests[q], status));
		}

		private void TryTopologifyReversed()
		{
			if (!checkBoxAllowReverse.Checked)
				return;

			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (var quest in plugin.Quests.Values.Where(q => !q.isCompleted))
				{
					if (!KCDatabase.Instance.Quest.Quests.ContainsKey(quest.ID) && quest.Prerequisite.All(q => plugin.Quests[q].isCompleted))
					{
						if (quest.Prerequisite.Any(q => !plugin.Quests[q].isDerivedCompleted))
							quest.Completed = ExtendedQuestData.Status.AggressiveMarkedCompleted;
						else
							quest.Completed = ExtendedQuestData.Status.AggressiveDerivedCompleted;
						flag = true;
					}
				}
			}
		}

		private void TryTopologify()
		{
			foreach (var id in KCDatabase.Instance.Quest.Quests.Keys)
			{
				var ext = plugin.Quests[id];
				MarkQuestTree(ext, ExtendedQuestData.Status.DerivedCompleted);
				ext.Completed = ExtendedQuestData.Status.DerivedUncompleted;
			}

			Record RecordData = plugin.LoadRecord();
			RestoreStatus(RecordData.DerivedCompleted, ExtendedQuestData.Status.DerivedCompleted,
				ExtendedQuestData.Status.DerivedCompleted);
			RestoreStatus(RecordData.MarkedCompleted, ExtendedQuestData.Status.MarkedCompleted,
				ExtendedQuestData.Status.MarkedTreeCompleted);
			RestoreStatus(RecordData.AggressiveDerivedCompleted, ExtendedQuestData.Status.AggressiveDerivedCompleted, null);
			RestoreStatus(RecordData.AggressiveMarkedCompleted, ExtendedQuestData.Status.AggressiveMarkedCompleted, null);
		}

		private void RestoreStatus(List<int> questIds, ExtendedQuestData.Status thisStatus,
			ExtendedQuestData.Status? treeStatus)
		{
			foreach (var id in questIds)
			{
				var quest = plugin.Quests[id];
				if (quest.isCompleted)
					continue;
				if (treeStatus != null)
					MarkQuestTree(quest, treeStatus.Value);
				quest.Completed = thisStatus;
			}
		}

		private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex > -1)
			{
				dataGridView.ClearSelection();
				dataGridView.Rows[e.RowIndex].Selected = true;
			}
		}

		private void markAsCompletedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridView.SelectedRows.Count != 1)
				return;

			var quest = plugin.Quests[(int) dataGridView.SelectedRows[0].Cells[ColumnId.Index].Value];
			if (MessageBox.Show(
				string.Format("Sure to mark {0} as completed? This will also mark all its prerequisite.", quest.Description),
				"Confirm",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning,
				MessageBoxDefaultButton.Button2
				) == DialogResult.Yes)
			{
				if (quest.Completed == ExtendedQuestData.Status.DerivedUncompleted ||
				    BuildPrerequisiteTree(quest).Any(q => (!q.Recurring) && q.Completed == ExtendedQuestData.Status.DerivedUncompleted))
				{
					MessageBox.Show("Impossible lah.");
					return;
				}

				MarkQuestTree(quest, ExtendedQuestData.Status.MarkedTreeCompleted);
				quest.Completed = ExtendedQuestData.Status.MarkedCompleted;
				TryTopologifyReversed();
				RefreshView();
			}
		}

		private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			plugin.SaveRecord();
		}

		private void checkBoxAllowReverse_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxAllowReverse.Checked)
			{
				if (MessageBox.Show(
					string.Join("\n",
						"Enabling this will cause Topologify to consider all quests",
						"1) not displaying",
						"2) whose prerequisite are completed",
						"as completed.",
						"",
						"DO NOT USE THIS WHEN THERE ARE STILL 検証中!",
						"",
						"",
						"Should Topologify aggressively check all quests now?"
						), "Confirm",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning,
					MessageBoxDefaultButton.Button2
					) == DialogResult.Yes)
				{
					TryTopologifyReversed();
					RefreshView();
				}
			}
		}
	}
}
