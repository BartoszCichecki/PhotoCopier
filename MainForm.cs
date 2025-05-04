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

        extensionTextBox.Text = "ARW";
        destinationTextBox.Text = "D:\\Photos";

        RefreshDrives();
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

        var drives = Manager.GetDrives();
        driveComboBox.Items.Clear();

        if (drives.Length <= 0)
            return;

        driveComboBox.Items.AddRange(drives.Cast<object>().ToArray());
        driveComboBox.SelectedIndex = 0;
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
        if (_cts != null)
        {
            await _cts.CancelAsync();
            return;
        }

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

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

        _cts = null;

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
        if (_cts == null)
            return;

        e.Cancel = true;

        MessageBox.Show(this,
            "Operation in progress.",
            "Warning",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }
}
