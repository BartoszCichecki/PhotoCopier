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
            progressBar.Location = new Point(19, 180);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(524, 23);
            progressBar.TabIndex = 3;
            // 
            // copyButton
            // 
            copyButton.Location = new Point(19, 213);
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(524, 23);
            copyButton.TabIndex = 4;
            copyButton.Text = "Copy";
            copyButton.UseVisualStyleBackColor = true;
            copyButton.Click += CopyButton_Click;
            // 
            // destinationTextBox
            // 
            destinationTextBox.Location = new Point(94, 74);
            destinationTextBox.Name = "destinationTextBox";
            destinationTextBox.Size = new Size(412, 23);
            destinationTextBox.TabIndex = 5;
            // 
            // destinationSelectButton
            // 
            destinationSelectButton.Location = new Point(512, 74);
            destinationSelectButton.Name = "destinationSelectButton";
            destinationSelectButton.Size = new Size(31, 23);
            destinationSelectButton.TabIndex = 6;
            destinationSelectButton.Text = "...";
            destinationSelectButton.UseVisualStyleBackColor = true;
            destinationSelectButton.Click += DestinationSelectButton_Click;
            // 
            // driveComboBox
            // 
            driveComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            driveComboBox.FormattingEnabled = true;
            driveComboBox.Location = new Point(94, 13);
            driveComboBox.Name = "driveComboBox";
            driveComboBox.Size = new Size(449, 23);
            driveComboBox.TabIndex = 7;
            // 
            // extensionLabel
            // 
            extensionLabel.AutoSize = true;
            extensionLabel.Location = new Point(19, 46);
            extensionLabel.Name = "extensionLabel";
            extensionLabel.Size = new Size(57, 15);
            extensionLabel.TabIndex = 8;
            extensionLabel.Text = "Extension";
            // 
            // extensionTextBox
            // 
            extensionTextBox.Location = new Point(94, 43);
            extensionTextBox.Name = "extensionTextBox";
            extensionTextBox.Size = new Size(449, 23);
            extensionTextBox.TabIndex = 9;
            // 
            // verifyChecksumsCheckBox
            // 
            verifyChecksumsCheckBox.AutoSize = true;
            verifyChecksumsCheckBox.Location = new Point(19, 118);
            verifyChecksumsCheckBox.Name = "verifyChecksumsCheckBox";
            verifyChecksumsCheckBox.Size = new Size(117, 19);
            verifyChecksumsCheckBox.TabIndex = 10;
            verifyChecksumsCheckBox.Text = "Verify checksums";
            verifyChecksumsCheckBox.UseVisualStyleBackColor = true;
            // 
            // overwriteExistingCheckBox
            // 
            overwriteExistingCheckBox.AutoSize = true;
            overwriteExistingCheckBox.Location = new Point(19, 143);
            overwriteExistingCheckBox.Name = "overwriteExistingCheckBox";
            overwriteExistingCheckBox.Size = new Size(120, 19);
            overwriteExistingCheckBox.TabIndex = 11;
            overwriteExistingCheckBox.Text = "Overwrite existing";
            overwriteExistingCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(562, 249);
            Controls.Add(overwriteExistingCheckBox);
            Controls.Add(verifyChecksumsCheckBox);
            Controls.Add(extensionTextBox);
            Controls.Add(extensionLabel);
            Controls.Add(driveComboBox);
            Controls.Add(destinationSelectButton);
            Controls.Add(destinationTextBox);
            Controls.Add(copyButton);
            Controls.Add(progressBar);
            Controls.Add(destinationLabel);
            Controls.Add(driveLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PhotoCopier";
            TopMost = true;
            FormClosing += Form_Closing;
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
    }
}
