namespace PhotoCopier;

public partial class MainForm : Form
{
    private CancellationTokenSource? cts;

    public MainForm()
    {
        InitializeComponent();

        extensionTextBox.Text = "ARW";
        destinationTextBox.Text = "D:\\Photos";

        var drives = Manager.GetDrives();
        driveComboBox.Items.Clear();

        if (drives.Length > 0)
        {
            driveComboBox.Items.AddRange(drives.Cast<object>().ToArray());
            driveComboBox.SelectedIndex = 0;
        }
    }

    private void DestinationSelectButton_Click(object sender, EventArgs e)
    {
        var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        destinationTextBox.Text = dialog.SelectedPath;
    }

    // ReSharper disable once AsyncVoidMethod
    private async void CopyButton_Click(object sender, EventArgs e)
    {
        if (cts != null)
        {
            await cts.CancelAsync();
            return;
        }

        cts = new CancellationTokenSource();
        var token = cts.Token;

        var extension = extensionTextBox.Text;
        if (!extension.StartsWith('.'))
            extension = '.' + extension;

        var drive = driveComboBox.SelectedItem as string;
        if (!Directory.Exists(drive))
            return;

        var destination = destinationTextBox.Text;
        if (!Directory.Exists(destination))
            return;

        var verify = verifyChecksumsCheckBox.Checked;
        var overwrite = overwriteExistingCheckBox.Checked;

        driveComboBox.Enabled = false;
        extensionTextBox.Enabled = false;
        destinationTextBox.Enabled = false;
        destinationSelectButton.Enabled = false;
        copyButton.Text = "Cancel";

        progressBar.Style = ProgressBarStyle.Marquee;

        var photos = await Manager.GetPhotosAsync(drive, extension);
        var filteredPhotos = await Manager.FilterPhotosAsync(photos, destination, overwrite);

        progressBar.Style = ProgressBarStyle.Blocks;

        CopyResult? result = null;
        try
        {
            result = await Manager.CopyAsync(filteredPhotos, destination, verify, (index, count) =>
            {
                progressBar.Invoke(() =>
                {
                    progressBar.Maximum = count;
                    progressBar.Value = index;
                });
            }, token);
        }
        catch (OperationCanceledException)
        {
            // Ignored
        }

        progressBar.Value = 0;
        driveComboBox.Enabled = true;
        extensionTextBox.Enabled = true;
        destinationTextBox.Enabled = true;
        destinationSelectButton.Enabled = true;
        copyButton.Text = "Copy";

        cts = null;

        if (!result.HasValue)
            return;

        if (result.Value.FailedHashChecks > 0)
        {
            MessageBox.Show(this,
                $"Successfully copied {result.Value.CopiedPhotos} photos, but {result.Value.FailedHashChecks} failed verification.",
                "Verification failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(this,
            $"Successfully copied {result.Value.CopiedPhotos} photos.",
            "Success",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (cts == null)
            return;

        e.Cancel = true;

        MessageBox.Show(this,
            "Operation in progress.",
            "Warning",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }
}
