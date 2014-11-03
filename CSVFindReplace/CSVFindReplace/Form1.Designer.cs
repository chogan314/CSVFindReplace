namespace CSVFindReplace
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.csvFileList = new System.Windows.Forms.CheckedListBox();
            this.source = new System.Windows.Forms.Button();
            this.destination = new System.Windows.Forms.Button();
            this.sourcePath = new System.Windows.Forms.TextBox();
            this.destPath = new System.Windows.Forms.TextBox();
            this.refresh = new System.Windows.Forms.Button();
            this.sourceBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.destBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.findReplace = new System.Windows.Forms.FlowLayoutPanel();
            this.findLabel = new System.Windows.Forms.Label();
            this.replaceLabel = new System.Windows.Forms.Label();
            this.formatLabel = new System.Windows.Forms.Label();
            this.columnsLabel = new System.Windows.Forms.Label();
            this.startRowLabel = new System.Windows.Forms.Label();
            this.startRow = new System.Windows.Forms.TextBox();
            this.runButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.saveParameters = new System.Windows.Forms.Button();
            this.resetParameters = new System.Windows.Forms.Button();
            this.delimiter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // csvFileList
            // 
            this.csvFileList.CheckOnClick = true;
            this.csvFileList.ColumnWidth = 250;
            this.csvFileList.FormattingEnabled = true;
            this.csvFileList.Location = new System.Drawing.Point(12, 42);
            this.csvFileList.MultiColumn = true;
            this.csvFileList.Name = "csvFileList";
            this.csvFileList.Size = new System.Drawing.Size(568, 154);
            this.csvFileList.TabIndex = 0;
            this.csvFileList.UseCompatibleTextRendering = true;
            // 
            // source
            // 
            this.source.Location = new System.Drawing.Point(13, 13);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(60, 23);
            this.source.TabIndex = 1;
            this.source.Text = "Source";
            this.source.UseVisualStyleBackColor = true;
            this.source.Click += new System.EventHandler(this.source_Click);
            // 
            // destination
            // 
            this.destination.Location = new System.Drawing.Point(13, 200);
            this.destination.Name = "destination";
            this.destination.Size = new System.Drawing.Size(75, 23);
            this.destination.TabIndex = 2;
            this.destination.Text = "Destination";
            this.destination.UseVisualStyleBackColor = true;
            this.destination.Click += new System.EventHandler(this.destination_Click);
            // 
            // sourcePath
            // 
            this.sourcePath.Location = new System.Drawing.Point(79, 15);
            this.sourcePath.Name = "sourcePath";
            this.sourcePath.ReadOnly = true;
            this.sourcePath.Size = new System.Drawing.Size(435, 20);
            this.sourcePath.TabIndex = 3;
            // 
            // destPath
            // 
            this.destPath.Location = new System.Drawing.Point(95, 202);
            this.destPath.Name = "destPath";
            this.destPath.ReadOnly = true;
            this.destPath.Size = new System.Drawing.Size(485, 20);
            this.destPath.TabIndex = 4;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(520, 13);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(60, 23);
            this.refresh.TabIndex = 5;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Click to restore.";
            this.notifyIcon.BalloonTipTitle = "CSV Find and Replace";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "CSV Find and Replace";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // findReplace
            // 
            this.findReplace.AutoScroll = true;
            this.findReplace.Location = new System.Drawing.Point(15, 275);
            this.findReplace.Name = "findReplace";
            this.findReplace.Size = new System.Drawing.Size(565, 158);
            this.findReplace.TabIndex = 6;
            // 
            // findLabel
            // 
            this.findLabel.AutoSize = true;
            this.findLabel.Location = new System.Drawing.Point(59, 257);
            this.findLabel.Name = "findLabel";
            this.findLabel.Size = new System.Drawing.Size(27, 13);
            this.findLabel.TabIndex = 7;
            this.findLabel.Text = "Find";
            // 
            // replaceLabel
            // 
            this.replaceLabel.AutoSize = true;
            this.replaceLabel.Location = new System.Drawing.Point(167, 257);
            this.replaceLabel.Name = "replaceLabel";
            this.replaceLabel.Size = new System.Drawing.Size(69, 13);
            this.replaceLabel.TabIndex = 8;
            this.replaceLabel.Text = "Replace with";
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(302, 257);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(61, 13);
            this.formatLabel.TabIndex = 9;
            this.formatLabel.Text = "With format";
            // 
            // columnsLabel
            // 
            this.columnsLabel.AutoSize = true;
            this.columnsLabel.Location = new System.Drawing.Point(425, 257);
            this.columnsLabel.Name = "columnsLabel";
            this.columnsLabel.Size = new System.Drawing.Size(72, 13);
            this.columnsLabel.TabIndex = 10;
            this.columnsLabel.Text = "From columns";
            // 
            // startRowLabel
            // 
            this.startRowLabel.AutoSize = true;
            this.startRowLabel.Location = new System.Drawing.Point(24, 231);
            this.startRowLabel.Name = "startRowLabel";
            this.startRowLabel.Size = new System.Drawing.Size(64, 13);
            this.startRowLabel.TabIndex = 11;
            this.startRowLabel.Text = "Start at row:";
            // 
            // startRow
            // 
            this.startRow.Location = new System.Drawing.Point(95, 228);
            this.startRow.Name = "startRow";
            this.startRow.Size = new System.Drawing.Size(100, 20);
            this.startRow.TabIndex = 12;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(13, 439);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 13;
            this.runButton.Text = "Run ";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(527, 251);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveParameters
            // 
            this.saveParameters.Location = new System.Drawing.Point(481, 439);
            this.saveParameters.Name = "saveParameters";
            this.saveParameters.Size = new System.Drawing.Size(98, 23);
            this.saveParameters.TabIndex = 20;
            this.saveParameters.Text = "Save Parameters";
            this.saveParameters.UseVisualStyleBackColor = true;
            this.saveParameters.Click += new System.EventHandler(this.saveParameters_Click);
            // 
            // resetParameters
            // 
            this.resetParameters.Location = new System.Drawing.Point(327, 439);
            this.resetParameters.Name = "resetParameters";
            this.resetParameters.Size = new System.Drawing.Size(148, 23);
            this.resetParameters.TabIndex = 21;
            this.resetParameters.Text = "Reset Parameters to Default";
            this.resetParameters.UseVisualStyleBackColor = true;
            this.resetParameters.Click += new System.EventHandler(this.resetParameters_Click);
            // 
            // delimiter
            // 
            this.delimiter.Location = new System.Drawing.Point(506, 228);
            this.delimiter.MaxLength = 1;
            this.delimiter.Name = "delimiter";
            this.delimiter.Size = new System.Drawing.Size(73, 20);
            this.delimiter.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(426, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Text Delimiter:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(594, 472);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.delimiter);
            this.Controls.Add(this.resetParameters);
            this.Controls.Add(this.saveParameters);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.startRow);
            this.Controls.Add(this.startRowLabel);
            this.Controls.Add(this.columnsLabel);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.replaceLabel);
            this.Controls.Add(this.findLabel);
            this.Controls.Add(this.findReplace);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.destPath);
            this.Controls.Add(this.sourcePath);
            this.Controls.Add(this.destination);
            this.Controls.Add(this.source);
            this.Controls.Add(this.csvFileList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(600, 500);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CSV Find and Replace";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox csvFileList;
        private System.Windows.Forms.Button source;
        private System.Windows.Forms.Button destination;
        private System.Windows.Forms.TextBox sourcePath;
        private System.Windows.Forms.TextBox destPath;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.FolderBrowserDialog sourceBrowserDialog;
        private System.Windows.Forms.FolderBrowserDialog destBrowserDialog;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.FlowLayoutPanel findReplace;
        private System.Windows.Forms.Label findLabel;
        private System.Windows.Forms.Label replaceLabel;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label columnsLabel;
        private System.Windows.Forms.Label startRowLabel;
        private System.Windows.Forms.TextBox startRow;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button saveParameters;
        private System.Windows.Forms.Button resetParameters;
        private System.Windows.Forms.TextBox delimiter;
        private System.Windows.Forms.Label label1;
    }
}

