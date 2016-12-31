using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciLors_Mashed_File_Extractor.FileFormats.PIZ {
    public class MagicNumbers {
        public static readonly byte[] FILE_SIGNATURE = new byte[] { 0x50, 0x49, 0x5A }; //PIZ
        public static readonly byte[] FILE_SIGNATURE_APPENDIX = new byte[] { 0x00, 0x03, 0x00, 0x00, 0x00 };

        public const int FILE_SIGNATURE_APPENDIX2_LENGTH = 4;

        public const int OFFSET_UNKNOWN = 0x08; //Random Number?
        public const int OFFSET_SECOND_FILE_SIGNATURE_APPENDIX = 0x0D; //0x00 or 0xCC
        public const int OFFSET_FILE_HEADER = 0x800;
        public const int OFFSET_FILE_HEADER_SIZE = 0x80;
        
        public const int OFFSET_FILE_HEADER_FILE_NAME_MAX_SIZE = 0x73;
        public const int OFFSET_FILE_HEADER_FILE_OFFSET = 0x74;
        public const int OFFSET_FILE_HEADER_FILE_SIZE = 0x78;
        public const int OFFSET_FILE_HEADER_FILE_UNKNOWN = 0x7C;
    }
}
