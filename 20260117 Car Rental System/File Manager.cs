using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20260117_Car_Rental_System
{
    /// <summary>
    /// Simple file manager that reads and writes a text file as a list of lines.
    /// </summary>
    /// <remarks>
    /// - Designed for small plain-text files where the entire file is held in memory.
    /// - Uses the current system default text encoding when reading/writing via <see cref="StreamReader"/> and <see cref="StreamWriter"/>.
    /// - This class is not thread-safe. Callers should synchronize access if multiple threads may call instance methods concurrently.
    /// </remarks>
    internal class File_Manager
    {
        // In-memory cache of file lines. Replaced on each successful <see cref="Read"/> call.
        private List<string> lines = new List<string>();

        // Path to the underlying text file.
        private string filePath = null;

        // Last known status: true when the most recent Read in the constructor succeeded.
        private bool status = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="File_Manager"/> class and attempts to read the file.
        /// </summary>
        /// <param name="path">Full or relative path to the text file to manage.</param>
        public File_Manager(string path)
        {
            filePath = path;
            // Attempt an initial read; store the result in <see cref="status"/>.
            status = Read();
        }

        /// <summary>
        /// Gets the cached lines that were read from the file.
        /// </summary>
        /// <returns>A <see cref="List{String}"/> containing the file lines in order. This reference points to the internal list.</returns>
        /// <remarks>
        /// The returned list is the internal cache; modifying it will affect subsequent calls to <see cref="Write"/> if append is used.
        /// If you need an independent copy call <c>new List&lt;string&gt;(fileManager.getLines())</c>.
        /// </remarks>
        public List<string> getLines() { return lines; }

        /// <summary>
        /// Gets the last known status of the manager.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the initial read in the constructor succeeded; otherwise <see langword="false"/>.
        /// </returns>
        public bool getStatus() { return status; }

        /// <summary>
        /// Reads the entire file into memory, replacing the internal cache.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the read succeeded; <see langword="false"/> if an exception occurred.
        /// When an exception occurs, the exception message is appended as the only line in the internal cache.
        /// </returns>
        /// <remarks>
        /// This method swallows exceptions and returns false; callers that require exception details should inspect the cached lines.
        /// </remarks>
        public bool Read()
        {
            // Reset the cache before reading.
            lines = new List<string>();
            try
            {
                // Open the file for reading. Uses default encoding and will throw if filePath is invalid.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    // Read line-by-line to preserve lines and memory usage for reasonably-sized files.
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                // On failure, populate the cache with the error message to aid debugging,
                // and return false so callers can detect the failure without an exception.
                //lines.Add(e.Message);
                //Console.WriteLine("Error reading file: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Writes the cached lines (optionally merged with provided content) back to the underlying file.
        /// </summary>
        /// <param name="content">Lines to write or append to the internal cache before saving.</param>
        /// <param name="append">
        /// If <see langword="true"/>, <paramref name="content"/> will be appended to the internal cache prior to saving.
        /// If <see langword="false"/>, the internal cache will be replaced with <paramref name="content"/> before saving.
        /// </param>
        /// <remarks>
        /// - The method always overwrites the file (it opens <see cref="StreamWriter"/> with append=false).
        /// - Any exceptions thrown by <see cref="StreamWriter"/> will propagate to the caller.
        /// - If you need to preserve the original file on error, wrap this call in a try/catch and implement your own backup logic.
        /// </remarks>
        public void Write(List<string> content, bool append = true)
        {
            if (append)
                // Merge provided content into the internal cache.
                foreach (string c in content)
                    lines.Add(c);
            else
                // Replace cache with provided content.
                lines = content;

            // Write the entire cache back to disk, overwriting the file.
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (string line in lines)
                    sw.WriteLine(line);
            }
        }
    }
}
