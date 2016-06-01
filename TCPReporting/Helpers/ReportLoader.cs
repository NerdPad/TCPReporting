namespace TCPReporting.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

        /// <summary>
        /// Gets the list of run ids based on the report date
        /// </summary>
        /// <param name="date">The report date.</param>
        /// <returns>A list of run ids</returns>
        public List<string> GetRunIDs(DateTime date)
        {
            // format date
            var selectedDate = date.ToString("yyyyMMdd");
            var runIds = new List<string>();

            // get all files, and find matching run ids
            this.GetAllSourceFiles().ForEach((file) =>
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                // if the file name doesn't contain the selected date
                if (string.IsNullOrEmpty(fileName) || !fileName.Contains(selectedDate))
                {
                    // skip it
                    return;
                }

                // add to list
                runIds.Add(this.GetRunIDFromFileName(fileName));
            });

            return runIds;
        }

        /// <summary>
        /// Loads the matching report.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="runId">The run identifier.</param>
        /// <param name="reportType">Type of the report.</param>
        public void Load(DateTime date, string runId, int reportType)
        {
            // copy files from source to target
            var sourceFiles = this.GetSourceFiles(date, runId, reportType);

            if (sourceFiles.Count == 0)
            {
                throw new Exception("No files found for the selection");
            }

            sourceFiles.ForEach((file) =>
            {
                var destFile = Path.Combine(this.targetFolder, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            });

            // TODO: Call the stored procedure
            // TODO: Create a new helper for handling database operation
        }

        /// <summary>
        /// Gets the source files.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="runId">The run identifier.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>A list of matching source files</returns>
        public List<string> GetSourceFiles(DateTime date, string runId, int reportType)
        {
            // create file name part to match
            var fileNameSegment = string.Format("_{0}_{1}", date.ToString("yyyyMMdd"), runId);
            var sourceFiles = new List<string>();

            // get all files, and find matching run ids
            this.GetAllSourceFiles().ForEach((file) =>
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                // if the file name doesn't contain the selected date, or is not the matching report type
                if (string.IsNullOrEmpty(fileName) || !fileName.Contains(fileNameSegment) || !this.IsMatchingReportType(fileName, reportType))
                {
                    // skip it
                    return;
                }

                // add to list
                sourceFiles.Add(file);
            });

            return sourceFiles;
        }

        /// <summary>
        /// Gets all source files.
        /// </summary>
        /// <returns>All the files in the source directory</returns>
        private List<string> GetAllSourceFiles()
        {
            return Directory.GetFiles(this.sourceFolder).ToList();
        }

        /// <summary>
        /// Determines whether the file name matches the report type
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns></returns>
        private bool IsMatchingReportType(string fileName, int reportType)
        {
            bool isCorrelated = fileName.ToLowerInvariant().StartsWith("corr");

            // This can be implemented in better ways but keeping it simple for better understanding
            switch ((ReportType) reportType)
            {
                case ReportType.NonCorrelated:
                    return !isCorrelated;
                case ReportType.Correlated:
                    return isCorrelated;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Gets the name of the run identifier from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private string GetRunIDFromFileName(string fileName)
        {
            var index = fileName.LastIndexOf('_');

            return fileName.Substring(index + 1);
        }

        /// <summary>
        /// Available report types
        /// </summary>
        private enum ReportType
        {
            NonCorrelated = 1,
            Correlated = 2,
            Both = 3
        }
    }
}
