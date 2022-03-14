﻿namespace River.OneMoreAddIn.Commands.Search
{
	partial class SearchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDialog));
            this.introLabel = new System.Windows.Forms.Label();
            this.findLabel = new System.Windows.Forms.Label();
            this.findBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.resultTree = new River.OneMoreAddIn.UI.HierarchyView();
            this.scopeBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // introLabel
            // 
            this.introLabel.AutoSize = true;
            this.introLabel.Location = new System.Drawing.Point(32, 28);
            this.introLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 18);
            this.introLabel.Name = "introLabel";
            this.introLabel.Size = new System.Drawing.Size(710, 25);
            this.introLabel.TabIndex = 0;
            this.introLabel.Text = "Enter one or more keywords, \"quoted phrases\", and use uppercase AND and OR";
            // 
            // findLabel
            // 
            this.findLabel.AutoSize = true;
            this.findLabel.Location = new System.Drawing.Point(32, 79);
            this.findLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.findLabel.Name = "findLabel";
            this.findLabel.Size = new System.Drawing.Size(56, 25);
            this.findLabel.TabIndex = 1;
            this.findLabel.Text = "Find:";
            // 
            // findBox
            // 
            this.findBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.findBox.BackColor = System.Drawing.SystemColors.Control;
            this.findBox.Location = new System.Drawing.Point(99, 76);
            this.findBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.findBox.Name = "findBox";
            this.findBox.Size = new System.Drawing.Size(495, 29);
            this.findBox.TabIndex = 0;
            this.findBox.TextChanged += new System.EventHandler(this.ChangeQuery);
            this.findBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchOnKeydown);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(783, 646);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 18);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(137, 42);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.Nevermind);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Enabled = false;
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.Location = new System.Drawing.Point(605, 74);
            this.searchButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 18);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(83, 38);
            this.searchButton.TabIndex = 4;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.Search);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.copyButton.Enabled = false;
            this.copyButton.Location = new System.Drawing.Point(490, 646);
            this.copyButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(137, 42);
            this.copyButton.TabIndex = 6;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.CopyPressed);
            // 
            // moveButton
            // 
            this.moveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.moveButton.Enabled = false;
            this.moveButton.Location = new System.Drawing.Point(637, 646);
            this.moveButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(137, 42);
            this.moveButton.TabIndex = 7;
            this.moveButton.Text = "Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.MovePressed);
            // 
            // resultTree
            // 
            this.resultTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultTree.CheckBoxes = true;
            this.resultTree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.resultTree.HideSelection = false;
            this.resultTree.HotTracking = true;
            this.resultTree.Location = new System.Drawing.Point(31, 134);
            this.resultTree.Margin = new System.Windows.Forms.Padding(4);
            this.resultTree.Name = "resultTree";
            this.resultTree.ShowLines = false;
            this.resultTree.ShowRootLines = false;
            this.resultTree.Size = new System.Drawing.Size(889, 480);
            this.resultTree.Suspend = false;
            this.resultTree.TabIndex = 5;
            this.resultTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeAfterCheck);
            this.resultTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ClickNode);
            // 
            // scopeBox
            // 
            this.scopeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scopeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scopeBox.FormattingEnabled = true;
            this.scopeBox.Items.AddRange(new object[] {
            "In all notebooks",
            "In this notebook",
            "In this section"});
            this.scopeBox.Location = new System.Drawing.Point(697, 78);
            this.scopeBox.Margin = new System.Windows.Forms.Padding(4);
            this.scopeBox.Name = "scopeBox";
            this.scopeBox.Size = new System.Drawing.Size(223, 32);
            this.scopeBox.TabIndex = 11;
            // 
            // SearchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(951, 713);
            this.Controls.Add(this.scopeBox);
            this.Controls.Add(this.resultTree);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.findBox);
            this.Controls.Add(this.findLabel);
            this.Controls.Add(this.introLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(877, 467);
            this.Name = "SearchDialog";
            this.Padding = new System.Windows.Forms.Padding(27, 28, 27, 18);
            this.Text = "Search and Move or Copy";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label introLabel;
		private System.Windows.Forms.Label findLabel;
		private System.Windows.Forms.TextBox findBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Button copyButton;
		private System.Windows.Forms.Button moveButton;
		private River.OneMoreAddIn.UI.HierarchyView resultTree;
		private System.Windows.Forms.ComboBox scopeBox;
	}
}