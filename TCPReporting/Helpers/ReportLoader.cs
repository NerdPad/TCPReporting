namespace TCPReporting.Helpers
{
    /// <summary>
    /// Helper for loading reports
    /// </summary>
    internal class ReportLoader
    {
        /// <summary>
        /// The document source folder
        /// </summary>
        private readonly string sourceFolder;

        /// <summary>
        /// The document target folder
        /// </summary>
        private readonly string targetFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportLoader"/> class.
        /// </summary>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="targetFolder">The target folder.</param>
        public ReportLoader(string sourceFolder, string targetFolder)
        {
            this.sourceFolder = sourceFolder;
            this.targetFolder = targetFolder;
        }
    }
}
