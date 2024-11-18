using System;
using System.Diagnostics;
using System.IO;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedFile"/> class.
        /// </summary>
        public UploadedFile() { }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedFile"/> class with the specified file name and file data.
        /// </summary>
        /// <param name="fileName">The name of the uploaded file.</param>
        /// <param name="fileData">The byte array representing the file data.</param>
        public UploadedFile(string fileName, byte[] fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }
        //___________________________________________________________________________________________________________

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
//____________________________________EOF_________________________________________________________________________