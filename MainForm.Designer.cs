namespace PhotoCopier
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            driveLabel = new Label();
            destinationLabel = new Label();
            progressBar = new ProgressBar();
            copyButton = new Button();
            destinationTextBox = new TextBox();
            destinationSelectButton = new Button();
            driveComboBox = new ComboBox();
            extensionLabel = new Label();
            extensionTextBox = new TextBox();
            verifyChecksumsCheckBox = new CheckBox();
            overwriteExistingCheckBox = new CheckBox();
            copyCompanionFilesCheckBox = new CheckBox();
            groupBox1 = new GroupBox();
            label1 = new Label();
            folderNameTextBox = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // driveLabel
            // 
            driveLabel.AutoSize = true;
            driveLabel.Location = new Point(19, 16);
            driveLabel.Name = "driveLabel";
            driveLabel.Size = new Size(34, 15);
            driveLabel.TabIndex = 1;
            driveLabel.Text = "Drive";
            // 
            // destinationLabel
            // 
            destinationLabel.AutoSize = true;
            destinationLabel.Location = new Point(19, 78);
            destinationLabel.Name = "destinationLabel";
            destinationLabel.Size = new Size(67, 15);
            destinationLabel.TabIndex = 2;
            destinationLabel.Text = "Destination";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(19, 258);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(492, 35);
            progressBar.TabIndex = 3;
            // 
            // copyButton
            // 
            copyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            copyButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            copyButton.Location = new Point(19, 299);
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(492, 30);
            copyButton.TabIndex = 4;
            copyButton.Text = "Copy";
            copyButton.UseVisualStyleBackColor = true;
            copyButton.Click += CopyButton_Click;
            // 
            // destinationTextBox
            // 
            destinationTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            destinationTextBox.Location = new Point(94, 74);
            destinationTextBox.Name = "destinationTextBox";
            destinationTextBox.Size = new Size(375, 23);
            destinationTextBox.TabIndex = 5;
            // 
            // destinationSelectButton
            // 
            destinationSelectButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            destinationSelectButton.Location = new Point(475, 74);
            destinationSelectButton.Name = "destinationSelectButton";
            destinationSelectButton.Size = new Size(36, 23);
            destinationSelectButton.TabIndex = 6;
            destinationSelectButton.Text = "...";
            destinationSelectButton.UseVisualStyleBackColor = true;
            destinationSelectButton.Click += DestinationSelectButton_Click;
            // 
            // driveComboBox
            // 
            driveComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            driveComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            driveComboBox.FormattingEnabled = true;
            driveComboBox.Location = new Point(94, 13);
            driveComboBox.Name = "driveComboBox";
            driveComboBox.Size = new Size(417, 23);
            driveComboBox.TabIndex = 7;
            // 
            // extensionLabel
            // 
            extensionLabel.AutoSize = true;
            extensionLabel.Location = new Point(19, 46);
            extensionLabel.Name = "extensionLabel";
            extensionLabel.Size = new Size(70, 15);
            extensionLabel.TabIndex = 8;
            extensionLabel.Text = "Extension(s)";
            // 
            // extensionTextBox
            // 
            extensionTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            extensionTextBox.Location = new Point(94, 43);
            extensionTextBox.Name = "extensionTextBox";
            extensionTextBox.Size = new Size(417, 23);
            extensionTextBox.TabIndex = 9;
            extensionTextBox.TextChanged += ExtensionTextBox_TextChanged;
            // 
            // verifyChecksumsCheckBox
            // 
            verifyChecksumsCheckBox.AutoSize = true;
            verifyChecksumsCheckBox.Location = new Point(11, 47);
            verifyChecksumsCheckBox.Name = "verifyChecksumsCheckBox";
            verifyChecksumsCheckBox.Size = new Size(117, 19);
            verifyChecksumsCheckBox.TabIndex = 10;
            verifyChecksumsCheckBox.Text = "Verify checksums";
            verifyChecksumsCheckBox.UseVisualStyleBackColor = true;
            // 
            // overwriteExistingCheckBox
            // 
            overwriteExistingCheckBox.AutoSize = true;
            overwriteExistingCheckBox.Location = new Point(11, 72);
            overwriteExistingCheckBox.Name = "overwriteExistingCheckBox";
            overwriteExistingCheckBox.Size = new Size(120, 19);
            overwriteExistingCheckBox.TabIndex = 11;
            overwriteExistingCheckBox.Text = "Overwrite existing";
            overwriteExistingCheckBox.UseVisualStyleBackColor = true;
            // 
            // copyCompanionFilesCheckBox
            // 
            copyCompanionFilesCheckBox.AutoSize = true;
            copyCompanionFilesCheckBox.Location = new Point(11, 22);
            copyCompanionFilesCheckBox.Name = "copyCompanionFilesCheckBox";
            copyCompanionFilesCheckBox.Size = new Size(142, 19);
            copyCompanionFilesCheckBox.TabIndex = 12;
            copyCompanionFilesCheckBox.Text = "Copy companion files";
            copyCompanionFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.Controls.Add(copyCompanionFilesCheckBox);
            groupBox1.Controls.Add(verifyChecksumsCheckBox);
            groupBox1.Controls.Add(overwriteExistingCheckBox);
            groupBox1.Location = new Point(19, 132);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(492, 100);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 106);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 13;
            label1.Text = "Folder name";
            // 
            // folderNameTextBox
            // 
            folderNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            folderNameTextBox.Location = new Point(94, 103);
            folderNameTextBox.Name = "folderNameTextBox";
            folderNameTextBox.Size = new Size(417, 23);
            folderNameTextBox.TabIndex = 14;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(523, 341);
            Controls.Add(folderNameTextBox);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(extensionTextBox);
            Controls.Add(extensionLabel);
            Controls.Add(driveComboBox);
            Controls.Add(destinationSelectButton);
            Controls.Add(destinationTextBox);
            Controls.Add(copyButton);
            Controls.Add(progressBar);
            Controls.Add(destinationLabel);
            Controls.Add(driveLabel);
            MaximizeBox = false;
            MaximumSize = new Size(1200, 800);
            MinimumSize = new Size(500, 380);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PhotoCopier";
            FormClosing += Form_Closing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label driveLabel;
        private Label destinationLabel;
        private ProgressBar progressBar;
        private Button copyButton;
        private TextBox destinationTextBox;
        private Button destinationSelectButton;
        private ComboBox driveComboBox;
        private Label extensionLabel;
        private TextBox extensionTextBox;
        private CheckBox verifyChecksumsCheckBox;
        private CheckBox overwriteExistingCheckBox;
        private CheckBox copyCompanionFilesCheckBox;
        private GroupBox groupBox1;
        private Label label1;
        private TextBox folderNameTextBox;
    }
}
