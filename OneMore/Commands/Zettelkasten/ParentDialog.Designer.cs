namespace River.OneMoreAddIn.Commands.Search
{
	partial class ParentsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParentsDialog));
            this.introLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resultTree = new River.OneMoreAddIn.UI.HierarchyView();
            this.scopeBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // introLabel
            // 
            this.introLabel.AutoSize = true;
            this.introLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.introLabel.Location = new System.Drawing.Point(18, 42);
            this.introLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.introLabel.Name = "introLabel";
            this.introLabel.Size = new System.Drawing.Size(84, 13);
            this.introLabel.TabIndex = 0;
            this.introLabel.Text = "Parents of Child:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(427, 350);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.Nevermind);
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
            this.resultTree.Location = new System.Drawing.Point(17, 73);
            this.resultTree.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.resultTree.Name = "resultTree";
            this.resultTree.ShowLines = false;
            this.resultTree.ShowRootLines = false;
            this.resultTree.Size = new System.Drawing.Size(487, 262);
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
            "In this notebook",
            "In all notebooks",
            "In this section"});
            this.scopeBox.Location = new System.Drawing.Point(380, 42);
            this.scopeBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.scopeBox.Name = "scopeBox";
            this.scopeBox.Size = new System.Drawing.Size(123, 21);
            this.scopeBox.TabIndex = 11;
            // 
            // ParentsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(519, 386);
            this.Controls.Add(this.scopeBox);
            this.Controls.Add(this.resultTree);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.introLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(482, 259);
            this.Name = "ParentsDialog";
            this.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.Text = "Search and Move or Copy";
            this.Activated += new System.EventHandler(this.ParentsDialog_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label introLabel;
		private System.Windows.Forms.Button cancelButton;
		private River.OneMoreAddIn.UI.HierarchyView resultTree;
        private System.Windows.Forms.ComboBox scopeBox;
    }
}