using PhotoCopier.Properties;

namespace PhotoCopier;

public partial class MainForm : Form
{
    private const int WmDeviceChange = 0x0219;
    private const int DbtDeviceArrival = 0x8000;
    private const int DbtDeviceRemovalComplete = 0x8004;

    private CancellationTokenSource? _cts;

    public MainForm()
    {
        InitializeComponent();

        extensionTextBox.Text = Settings.Default.Extensions;
        destinationTextBox.Text = Settings.Default.Destination;
        folderNameTextBox.Text = Settings.Default.FolderName;

        copyCompanionFilesCheckBox.Checked = Settings.Default.CopyCompanionFiles;
        verifyChecksumsCheckBox.Checked = Settings.Default.VerifyChecksums;
        overwriteExistingCheckBox.Checked = Settings.Default.OverwriteExisting;

        RefreshDrives();

        ActiveControl = copyButton;
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m is { Msg: WmDeviceChange, WParam: DbtDeviceArrival or DbtDeviceRemovalComplete })
            RefreshDrives();
    }

    private void RefreshDrives()
    {
        if (_cts != null)
            return;

        copyButton.Enabled = false;

        var drives = Manager.GetDrives();
        driveComboBox.Items.Clear();

        if (drives.Length <= 0)
            return;

        driveComboBox.Items.AddRange(drives.Cast<object>().ToArray());
        driveComboBox.SelectedIndex = 0;

        copyButton.Enabled = true;
    }

    private void SetEnabled(bool enabled)
    {
        driveComboBox.Enabled = enabled;
        extensionTextBox.Enabled = enabled;
        destinationTextBox.Enabled = enabled;
        destinationSelectButton.Enabled = enabled;
        folderNameTextBox.Enabled = enabled;
        copyCompanionFilesCheckBox.Enabled = enabled;
        verifyChecksumsCheckBox.Enabled = enabled;
        overwriteExistingCheckBox.Enabled = enabled;
    }

    private void DestinationSelectButton_Click(object sender, EventArgs e)
    {
        var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        destinationTextBox.Text = dialog.SelectedPath;
    }

    private void ExtensionTextBox_TextChanged(object sender, EventArgs e)
    {
        Settings.Default.Extensions = extensionTextBox.Text;
        Settings.Default.Save();
    }

    private void DestinationTextBox_TextChanged(object sender, EventArgs e)
    {
        Settings.Default.Destination = destinationTextBox.Text;
        Settings.Default.Save();
    }

    private void FolderNameTextBox_TextChanged(object sender, EventArgs e)
    {
        Settings.Default.FolderName = folderNameTextBox.Text;
        Settings.Default.Save();
    }

    private void CopyCompanionFilesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.CopyCompanionFiles = copyCompanionFilesCheckBox.Checked;
        Settings.Default.Save();
    }

    private void VerifyChecksumsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.VerifyChecksums = verifyChecksumsCheckBox.Checked;
        Settings.Default.Save();
    }

    private void OverwriteExistingCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.OverwriteExisting = overwriteExistingCheckBox.Checked;
        Settings.Default.Save();
    }

    // ReSharper disable once AsyncVoidMethod
    private async void CopyButton_Click(object sender, EventArgs e)
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
            return;
        }

        try
        {
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            var extension = extensionTextBox.Text;
            if (!extension.StartsWith('.'))
                extension = '.' + extension;

            var drive = driveComboBox.SelectedItem as string;
            if (!Directory.Exists(drive))
            {
                MessageBox.Show(Strings.DriveDoesNotExist_Text,
                    Strings.DriveDoesNotExist_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var destination = destinationTextBox.Text;
            if (!Directory.Exists(destination))
            {
                MessageBox.Show(Strings.DirectoryDoesNotExist_Text,
                    Strings.DirectoryDoesNotExist_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var folderName = folderNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(folderName))
            {
                MessageBox.Show(Strings.InvalidFolderName_Text,
                    Strings.InvalidFolderName_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var copyCompanionFiles = copyCompanionFilesCheckBox.Checked;
            var verify = verifyChecksumsCheckBox.Checked;
            var overwrite = overwriteExistingCheckBox.Checked;

            SetEnabled(false);
            copyButton.Text = Strings.Cancel;

            progressBar.Style = ProgressBarStyle.Marquee;

            var photos = await Manager.GetPhotosAsync(drive, extension);
            var filteredPhotos = await Manager.FilterPhotosAsync(photos, destination, folderName, overwrite);

            progressBar.Style = ProgressBarStyle.Blocks;

            CopyResult? result = null;
            try
            {
                result = await Manager.CopyAsync(filteredPhotos, folderName, destination, copyCompanionFiles, verify,
                    (index, count) =>
                    {
                        progressBar.Invoke(() =>
                        {
                            progressBar.Minimum = 0;
                            progressBar.Maximum = count;
                            progressBar.Value = index;
                        });
                    }, token);
            }
            catch (OperationCanceledException)
            {
                // Ignored
            }

            if (!result.HasValue)
                return;

            if (result.Value.FailedHashChecks > 0)
            {
                MessageBox.Show(this,
                    string.Format(Strings.Copy_VerificationFailed_Text, result.Value.CopiedPhotos, result.Value.FailedHashChecks),
                    Strings.Copy_VerificationFailed_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(this,
                    string.Format(Strings.Copy_Success_Text, result.Value.CopiedPhotos),
                    Strings.Copy_Success_Caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        finally
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 0;
            progressBar.Value = 0;
            copyButton.Text = Strings.Copy;

            SetEnabled(true);

            _cts = null;
        }
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (_cts == null)
            return;

        e.Cancel = true;

        MessageBox.Show(this,
            Strings.OperationInProgress_Text,
            Strings.OperationInProgress_Caption,
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }
}
