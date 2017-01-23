namespace Topologify
{
	partial class ToolForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.buttonReset = new System.Windows.Forms.Button();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.ColumnDisplayed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnWikiId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStripMark = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.markAsCompletedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linkLabel = new System.Windows.Forms.LinkLabel();
			this.checkBoxAllowReverse = new System.Windows.Forms.CheckBox();
			this.contextMenuStripReset = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.aggressivelyMarkedQuestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.manuallyMarkedQuestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.everythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.contextMenuStripMark.SuspendLayout();
			this.contextMenuStripReset.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 4;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.buttonReset, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.buttonUpdate, 3, 0);
			this.tableLayoutPanel.Controls.Add(this.dataGridView, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.linkLabel, 0, 2);
			this.tableLayoutPanel.Controls.Add(this.checkBoxAllowReverse, 0, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 3;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.Size = new System.Drawing.Size(584, 361);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// buttonReset
			// 
			this.buttonReset.AutoSize = true;
			this.buttonReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonReset.Location = new System.Drawing.Point(452, 3);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(45, 23);
			this.buttonReset.TabIndex = 0;
			this.buttonReset.Text = "Reset";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.AutoSize = true;
			this.buttonUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonUpdate.Location = new System.Drawing.Point(503, 3);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(78, 23);
			this.buttonUpdate.TabIndex = 1;
			this.buttonUpdate.Text = "Update Data";
			this.buttonUpdate.UseVisualStyleBackColor = true;
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToResizeRows = false;
			this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDisplayed,
            this.ColumnId,
            this.ColumnWikiId,
            this.ColumnCategory,
            this.ColumnTitle});
			this.tableLayoutPanel.SetColumnSpan(this.dataGridView, 4);
			this.dataGridView.ContextMenuStrip = this.contextMenuStripMark;
			this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView.Location = new System.Drawing.Point(3, 32);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.RowHeadersVisible = false;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView.Size = new System.Drawing.Size(578, 313);
			this.dataGridView.TabIndex = 2;
			this.dataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDown);
			// 
			// ColumnDisplayed
			// 
			this.ColumnDisplayed.HeaderText = "";
			this.ColumnDisplayed.Name = "ColumnDisplayed";
			this.ColumnDisplayed.ReadOnly = true;
			this.ColumnDisplayed.Width = 5;
			// 
			// ColumnId
			// 
			this.ColumnId.HeaderText = "ID";
			this.ColumnId.Name = "ColumnId";
			this.ColumnId.ReadOnly = true;
			this.ColumnId.Width = 43;
			// 
			// ColumnWikiId
			// 
			this.ColumnWikiId.HeaderText = "Wiki ID";
			this.ColumnWikiId.Name = "ColumnWikiId";
			this.ColumnWikiId.ReadOnly = true;
			this.ColumnWikiId.Width = 67;
			// 
			// ColumnCategory
			// 
			this.ColumnCategory.HeaderText = "Category";
			this.ColumnCategory.Name = "ColumnCategory";
			this.ColumnCategory.ReadOnly = true;
			this.ColumnCategory.Width = 74;
			// 
			// ColumnTitle
			// 
			this.ColumnTitle.HeaderText = "Title";
			this.ColumnTitle.Name = "ColumnTitle";
			this.ColumnTitle.ReadOnly = true;
			this.ColumnTitle.Width = 52;
			// 
			// contextMenuStripMark
			// 
			this.contextMenuStripMark.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markAsCompletedToolStripMenuItem});
			this.contextMenuStripMark.Name = "contextMenuStrip";
			this.contextMenuStripMark.Size = new System.Drawing.Size(178, 26);
			// 
			// markAsCompletedToolStripMenuItem
			// 
			this.markAsCompletedToolStripMenuItem.Name = "markAsCompletedToolStripMenuItem";
			this.markAsCompletedToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.markAsCompletedToolStripMenuItem.Text = "Mark as Completed";
			this.markAsCompletedToolStripMenuItem.Click += new System.EventHandler(this.markAsCompletedToolStripMenuItem_Click);
			// 
			// linkLabel
			// 
			this.linkLabel.AutoSize = true;
			this.tableLayoutPanel.SetColumnSpan(this.linkLabel, 4);
			this.linkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.linkLabel.Location = new System.Drawing.Point(3, 348);
			this.linkLabel.Name = "linkLabel";
			this.linkLabel.Size = new System.Drawing.Size(578, 13);
			this.linkLabel.TabIndex = 3;
			this.linkLabel.TabStop = true;
			this.linkLabel.Text = "linkLabel1";
			// 
			// checkBoxAllowReverse
			// 
			this.checkBoxAllowReverse.AutoSize = true;
			this.checkBoxAllowReverse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkBoxAllowReverse.Location = new System.Drawing.Point(3, 3);
			this.checkBoxAllowReverse.Name = "checkBoxAllowReverse";
			this.checkBoxAllowReverse.Size = new System.Drawing.Size(228, 23);
			this.checkBoxAllowReverse.TabIndex = 4;
			this.checkBoxAllowReverse.Text = "Aggressive Detection of Completed Quests";
			this.checkBoxAllowReverse.UseVisualStyleBackColor = true;
			this.checkBoxAllowReverse.CheckedChanged += new System.EventHandler(this.checkBoxAllowReverse_CheckedChanged);
			// 
			// contextMenuStripReset
			// 
			this.contextMenuStripReset.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aggressivelyMarkedQuestsToolStripMenuItem,
            this.manuallyMarkedQuestsToolStripMenuItem,
            this.everythingToolStripMenuItem});
			this.contextMenuStripReset.Name = "contextMenuStripReset";
			this.contextMenuStripReset.Size = new System.Drawing.Size(223, 70);
			// 
			// aggressivelyMarkedQuestsToolStripMenuItem
			// 
			this.aggressivelyMarkedQuestsToolStripMenuItem.Name = "aggressivelyMarkedQuestsToolStripMenuItem";
			this.aggressivelyMarkedQuestsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.aggressivelyMarkedQuestsToolStripMenuItem.Text = "Aggressively Marked Quests";
			this.aggressivelyMarkedQuestsToolStripMenuItem.Click += new System.EventHandler(this.aggressivelyMarkedQuestsToolStripMenuItem_Click);
			// 
			// manuallyMarkedQuestsToolStripMenuItem
			// 
			this.manuallyMarkedQuestsToolStripMenuItem.Name = "manuallyMarkedQuestsToolStripMenuItem";
			this.manuallyMarkedQuestsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.manuallyMarkedQuestsToolStripMenuItem.Text = "Manually Marked Quests";
			this.manuallyMarkedQuestsToolStripMenuItem.Click += new System.EventHandler(this.manuallyMarkedQuestsToolStripMenuItem_Click);
			// 
			// everythingToolStripMenuItem
			// 
			this.everythingToolStripMenuItem.Name = "everythingToolStripMenuItem";
			this.everythingToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
			this.everythingToolStripMenuItem.Text = "Everything";
			this.everythingToolStripMenuItem.Click += new System.EventHandler(this.everythingToolStripMenuItem_Click);
			// 
			// ToolForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 361);
			this.Controls.Add(this.tableLayoutPanel);
			this.Name = "ToolForm";
			this.Text = "Topologify";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolForm_FormClosing);
			this.Load += new System.EventHandler(this.ToolForm_Load);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.contextMenuStripMark.ResumeLayout(false);
			this.contextMenuStripReset.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.LinkLabel linkLabel;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnDisplayed;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWikiId;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCategory;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTitle;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripMark;
		private System.Windows.Forms.ToolStripMenuItem markAsCompletedToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBoxAllowReverse;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripReset;
		private System.Windows.Forms.ToolStripMenuItem aggressivelyMarkedQuestsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem manuallyMarkedQuestsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem everythingToolStripMenuItem;
	}
}