namespace TCPReporting
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Helpers;
    using Properties;

    public partial class LoadReports : Form
    {
        /// <summary>
        /// The report loader
        /// </summary>
        private readonly ReportLoader report;

        /// <summary>
        /// The report types
        /// </summary>
        private List<ReportType> reportTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadReports"/> class.
        /// </summary>
        public LoadReports()
        {
            this.InitializeComponent();

            // Initialize
            this.report = new ReportLoader(Settings.Default.SourceFolder, Settings.Default.TargetFolder);
            this.BindReportTypes();

            // Type to load the run ids
            this.OnReportDateSelected(null, null);

            this.EnableDisableLoadButton();
        }

        /// <summary>
        /// Binds the report types drop down.
        /// </summary>
        private void BindReportTypes()
        {
            this.reportTypes = new List<ReportType>
            {
                new ReportType {ID = 1, Name = "Non Correlated"},
                new ReportType {ID = 2, Name = "Correlated"},
                new ReportType {ID = 3, Name = "Both"}
            };

            this.ddlReportType.DataSource = this.reportTypes;

            // do not select anything by default
            this.ddlReportType.SelectedItem = null;
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
        /// Enables or disable the load button.
        /// </summary>
        private void EnableDisableLoadButton()
        {
            this.btnLoad.Enabled = this.ddlRunID.SelectedItem != null && this.ddlReportType.SelectedItem != null;
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowError(string message = "")
        {
            this.lblError.Text = message;
        }

        /// <summary>
        /// Called when report date is selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnReportDateSelected(object sender, EventArgs e)
        {
            // populate the run ids
            var runIds = this.report.GetRunIDs(this.reportDate.Value);
            this.ddlRunID.DataSource = runIds;

            // no selection by default
            this.ddlRunID.SelectedItem = null;

            this.ShowError(runIds.Count == 0 ? "No run ids found" : string.Empty);

            this.EnableDisableLoadButton();
        }

        /// <summary>
        /// Called when report type is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnReportTypeChanged(object sender, EventArgs e)
        {
            this.EnableDisableLoadButton();
        }

        /// <summary>
        /// Called when run id is selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRunIDSelected(object sender, EventArgs e)
        {
            this.EnableDisableLoadButton();
        }

        /// <summary>
        /// Called when load reports button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBtnLoadClick(object sender, EventArgs e)
        {
            if (this.ddlReportType.SelectedItem == null || this.ddlRunID.SelectedItem == null)
            {
                this.ShowError("Please select all fields");
                return;
            }

            // clear error
            this.ShowError();

            try
            {
                this.report.Load(this.reportDate.Value, this.ddlRunID.SelectedItem.ToString(), (this.ddlReportType.SelectedItem as ReportType).ID);

                // clear any previous errors
                this.ShowError();
                MessageBox.Show("All files processed");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
