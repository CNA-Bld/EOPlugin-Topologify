﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private bool isQuestDataAvailable = false;

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

            if (plugin.Quests.Count > 0)
                isQuestDataAvailable = true;

            TryTopologify();
            RefreshView();
        }

        private void RefreshView()
        {
            dataGridView.Rows.Clear();

            foreach (var ext in plugin.Quests.Values)
            {
                if (ext.Recurring || ext.IsCompleted)
                    continue;

                var row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                row.Cells[ColumnDisplayed.Index].Value = KCDatabase.Instance.Quest.Quests.ContainsKey(ext.ID);
                row.Cells[ColumnId.Index].Value = ext.ID;
                row.Cells[ColumnWikiId.Index].Value = ext.WikiID;
                row.Cells[ColumnCategory.Index].Value = Constants.GetQuestCategory(ext.Category);
                row.Cells[ColumnTitle.Index].Value = ext.Title;

                var tree = BuildPrerequisiteTree(ext);
                row.Cells[ColumnTitle.Index].ToolTipText = ext.Description + "\n" + ext.Detail + "\n\n" +
                                                           string.Join("\n", tree.Select(BuildDescription));

                dataGridView.Rows.Add(row);
            }

            dataGridView.Sort(ColumnWikiId, ListSortDirection.Ascending);
        }

        private static string BuildDescription(ExtendedQuestData quest)
        {
            return quest.Description
                   + (quest.Recurring ? " (R)" : "")
                   + (KCDatabase.Instance.Quest.Quests.ContainsKey(quest.ID) ? " <=" : "");
        }

        private List<ExtendedQuestData> BuildPrerequisiteTree(ExtendedQuestData quest)
        {
            var prerequisiteQuests = new List<ExtendedQuestData>();
            foreach (var pre in quest.Prerequisite.Where(i => plugin.Quests.ContainsKey(i))
                .Select(i => plugin.Quests[i]))
            {
                if (pre.IsCompleted)
                    continue;
                var thisPreStrs = new List<ExtendedQuestData>();
                if (!KCDatabase.Instance.Quest.Quests.ContainsKey(pre.ID))
                    thisPreStrs.AddRange(BuildPrerequisiteTree(pre));
                thisPreStrs.Add(pre);
                prerequisiteQuests.AddRange(thisPreStrs);
            }

            return prerequisiteQuests.Distinct().ToList();
        }

        private void MarkQuestTree(ExtendedQuestData quest, ExtendedQuestData.Status status)
        {
            if (quest.IsCompleted)
                return;

            if (!quest.Recurring)
                quest.Completed = status;

            quest.Prerequisite.ForEach(q =>
            {
                if (plugin.Quests.ContainsKey(q))
                    MarkQuestTree(plugin.Quests[q], status);
            });
        }

        private void TryTopologifyReversed()
        {
            if (!checkBoxAllowReverse.Checked)
                return;

            bool flag = true;
            while (flag)
            {
                flag = false;
                foreach (var quest in plugin.Quests.Values.Where(q => !q.IsCompleted))
                {
                    if (!KCDatabase.Instance.Quest.Quests.ContainsKey(quest.ID) &&
                        quest.Prerequisite.All(q => plugin.Quests[q].IsCompleted))
                    {
                        if (quest.Prerequisite.Any(q => !plugin.Quests[q].IsDerivedCompleted))
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
                if (!plugin.Quests.ContainsKey(id))
                    continue;
                var ext = plugin.Quests[id];
                MarkQuestTree(ext, ExtendedQuestData.Status.DerivedCompleted);
                ext.Completed = ExtendedQuestData.Status.DerivedUncompleted;
            }

            if (plugin.Quests.Count == 0)
                return;

            Record RecordData = plugin.LoadRecord();
            RestoreStatus(RecordData.DerivedCompleted, ExtendedQuestData.Status.DerivedCompleted,
                ExtendedQuestData.Status.DerivedCompleted);
            RestoreStatus(RecordData.MarkedCompleted, ExtendedQuestData.Status.MarkedCompleted,
                ExtendedQuestData.Status.MarkedTreeCompleted);
            RestoreStatus(RecordData.AggressiveDerivedCompleted, ExtendedQuestData.Status.AggressiveDerivedCompleted,
                null);
            RestoreStatus(RecordData.AggressiveMarkedCompleted, ExtendedQuestData.Status.AggressiveMarkedCompleted,
                null);
        }

        private void RestoreStatus(List<int> questIds, ExtendedQuestData.Status thisStatus,
            ExtendedQuestData.Status? treeStatus)
        {
            foreach (var id in questIds)
            {
                if (!plugin.Quests.ContainsKey(id)) // Don't care if DB is not updated yet.
                    continue;

                var quest = plugin.Quests[id];
                if (quest.IsCompleted || quest.Recurring) // Happens when we remembered some recurring quests.
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
                    $"Sure to mark {quest.Description} as completed? This will also mark all its prerequisite.",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                ) == DialogResult.Yes)
            {
                if (quest.Completed == ExtendedQuestData.Status.DerivedUncompleted ||
                    BuildPrerequisiteTree(quest).Any(q =>
                        (!q.Recurring) && q.Completed == ExtendedQuestData.Status.DerivedUncompleted))
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
            if (isQuestDataAvailable)
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
                            "Should Topologify enable this and aggressively check all quests now?"
                        ), "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2
                    ) == DialogResult.Yes)
                {
                    TryTopologifyReversed();
                    RefreshView();
                }
                else
                {
                    checkBoxAllowReverse.Checked = false;
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            contextMenuStripReset.Show(buttonReset, new Point(0, buttonReset.Height));
        }

        private void doReset(string message, Func<ExtendedQuestData, bool> where)
        {
            if (MessageBox.Show(
                    message, "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                ) == DialogResult.Yes)
            {
                foreach (var quest in plugin.Quests.Values.Where(where))
                {
                    quest.Completed = ExtendedQuestData.Status.Unknown;
                }

                MessageBox.Show("Topologify will now close.");
                Close();
            }
        }

        private void aggressivelyMarkedQuestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doReset("Sure to reset all aggressively marked quests?", q => q.IsAggressive);
        }

        private void manuallyMarkedQuestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doReset("Sure to reset all manually marked quests?", q => q.IsManual);
        }

        private void everythingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doReset("Sure to reset everything?", q => true);
        }

        private void OnUpdated()
        {
            MessageBox.Show("Data updated. Topologify will now close.");
            TryTopologify();
            Close();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            buttonUpdate.Enabled = false;
            buttonUpdate.Text = "Updating...";
            plugin.UpdateData(OnUpdated);
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://kcwikizh.github.io/kcdata/");
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 1)
                return;
            Clipboard.SetText(dataGridView.SelectedRows[0].Cells[ColumnTitle.Index].ToolTipText);
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 1)
                return;
            MessageBox.Show(dataGridView.SelectedRows[0].Cells[ColumnTitle.Index].ToolTipText);
        }
    }
}
