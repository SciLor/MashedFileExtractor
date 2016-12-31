using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SciLors_Mashed_File_Extractor.FileFormats.PIZ {
    public class PIZFile : MashedFile {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public FileSignature fileSignature { get; set; }
        public List<FileHeader> files { get; set; }
        public PIZFile(string path) {
            filePath = path;
            fileName = Path.GetFileName(path);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                byte[] signatureBytes = new byte[FileSignature.Length];

                readBytesSafe(stream, signatureBytes);
                fileSignature = new FileSignature(signatureBytes);

                stream.Position = MagicNumbers.OFFSET_FILE_HEADER; //Jump to File Table
                byte[] headerBytes = new byte[MagicNumbers.OFFSET_FILE_HEADER_SIZE];
                files = new List<FileHeader>();
                while (true) {
                    readBytesSafe(stream, headerBytes);
                    if (headerBytes[0] == 0x00)
                        break;
                    files.Add(new FileHeader(headerBytes));
                }

            }
        }
        private void readBytesSafe(FileStream stream, byte[] target) {
            int n = 0;
            int bytesToRead = target.Length;
            int bytesRead = 0;
            while (bytesToRead > 0) {
                n = stream.Read(target, bytesRead, bytesToRead);
                if (n == 0)
                    throw new FileFormatException("File too small");
                bytesRead += n;
                bytesToRead -= n;
            }
        }
    }
}
