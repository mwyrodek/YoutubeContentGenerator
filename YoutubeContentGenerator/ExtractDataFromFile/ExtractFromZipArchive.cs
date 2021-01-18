namespace YoutubeContentGenerator.ExtractDataFromFile
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "SA1503", Justification = "used for error throwing")]
    public class ExtractFromZipArchive : IExctrectFromArchive
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractFromZipArchive"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        public ExtractFromZipArchive(ILogger<ExtractFromZipArchive> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public bool ArchiveHasFile(FileInfo info, string fileName)
        {
            if (info == null) throw new ArgumentNullException($"File info is null");
            
            using var zipToOpen = new FileStream(info.FullName, FileMode.Open);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);
            this.logger.LogDebug($"Checking if file {fileName} is in map files");
            return archive.Entries.Any(s => s.Name == fileName);

        }

        /// <inheritdoc/>
        public string ReadFileFromArchive(FileInfo info, string fileName)
        {
            if (info == null) throw new ArgumentNullException($"File info is null");

            using var zipToOpen = new FileStream(info.FullName, FileMode.Open);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

            var readmeEntry = archive.GetEntry(fileName);
            if (readmeEntry == null) throw new FileLoadException($"{fileName} was not loaded properly");

            using var reader = new StreamReader(readmeEntry.Open());
            return reader.ReadToEnd();
        }
    }

    public interface IExctrectFromArchive
    {
        /// <summary>
        /// Checks if archive has file
        /// </summary>
        /// <param name="info">name and location of archive.</param>
        /// <param name="fileName">file to find in archive.</param>
        /// <returns>true is file is present.</returns>
        bool ArchiveHasFile(FileInfo info, string fileName);

        /// <summary>
        /// Reads given file from archive.
        /// </summary>
        /// <param name="info">name and location of archive.</param>
        /// <param name="fileName">file to read in archive.</param>
        /// <returns>file value.</returns>
        string ReadFileFromArchive(FileInfo info, string fileName);
    }
}