using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciLors_Mashed_File_Extractor.FileFormats.PIZ {
    public class FileHeader {
        public String fileName { get; set; }
        public int fileOffset { get; set; }
        public int fileSize { get; set; }
        public int fileUnknown { get; set; }

        public FileHeader(byte[] header) {
            if (header.Length != MagicNumbers.OFFSET_FILE_HEADER_SIZE)
                throw new IndexOutOfRangeException("File header must be " + MagicNumbers.OFFSET_FILE_HEADER_SIZE + "bytes long");
            fileName = extractFileName(header);
            fileOffset = extractFileOffset(header);
            fileSize = extractFileSize(header);
            fileUnknown = extractFileUnknown(header);
        }
        private String extractFileName(byte[] header) {
            StringBuilder fileNameBuilder = new StringBuilder();
            for (int i = 0; i < MagicNumbers.OFFSET_FILE_HEADER_FILE_NAME_MAX_SIZE; i++) {
                if (header[i] != 0x00) {
                    fileNameBuilder.Append(Convert.ToChar(header[i]));
                } else {
                    break;
                }
            }
            return fileNameBuilder.ToString();
        }

        private int extractFileOffset(byte[] header) {
            int offset = header[MagicNumbers.OFFSET_FILE_HEADER_FILE_OFFSET + 0] << 0;
            offset += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_OFFSET + 1] << 8;
            offset += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_OFFSET + 2] << 16;
            offset += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_OFFSET + 3] << 24;
            return offset;
        }

        private int extractFileSize(byte[] header) {
            int size = header[MagicNumbers.OFFSET_FILE_HEADER_FILE_SIZE + 0] << 0;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_SIZE + 1] << 8;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_SIZE + 2] << 16;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_OFFSET + 3] << 24;
            return size;
        }

        private int extractFileUnknown(byte[] header) {
            int size = header[MagicNumbers.OFFSET_FILE_HEADER_FILE_UNKNOWN + 0] << 0;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_UNKNOWN + 1] << 8;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_UNKNOWN + 2] << 16;
            size += header[MagicNumbers.OFFSET_FILE_HEADER_FILE_UNKNOWN + 3] << 24;
            return size;
        }

    }
}
