namespace TCPReporting
{
    using System;
    using System.Windows.Forms;
    using Helpers;
    using Properties;

    public partial class LoadReports : Form
    {
        private readonly ReportLoader report;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadReports"/> class.
        /// </summary>
        public LoadReports()
        {
            this.InitializeComponent();

            // Initialize the report helper
            this.report = new ReportLoader(Settings.Default.SourceFolder, Settings.Default.TargetFolder);
        }

        /// <summary>
        /// Occurs when the close button is clicked
        /// </summary>
        /// <param name="sender">Event origin</param>
        /// <param name="e">Event arguments</param>
        private void OnBtnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when load reports button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBtnLoadClick(object sender, EventArgs e)
        {
            // choosen date
            this.reportDate.Value.ToString("MM-dd-yyyy");

            // run id
        }
    }
}
