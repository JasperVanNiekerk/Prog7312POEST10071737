using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Prog7312POEST10071737.Models
{
    [Serializable]
    public class UploadedFile
    {
        /// <summary>
        /// Gets or sets the name of the uploaded file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the byte array representing the file data.
        /// </summary>
        public byte[] FileData { get; set; }

        public UploadedFile() { }

        public UploadedFile(string fileName, byte[] fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }

        /// <summary>
        /// Opens the file using the user's default application.
        /// The file is written to a temporary location.
        /// </summary>
        public void OpenFile()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), FileName);
            File.WriteAllBytes(tempPath, FileData);
            Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
    }
}