using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SciLors_Mashed_File_Extractor.FileFormats.PIZ {
    public class FileSignature {
        public static int Length { get {
            return MagicNumbers.OFFSET_SECOND_FILE_SIGNATURE_APPENDIX + MagicNumbers.FILE_SIGNATURE_APPENDIX2_LENGTH;
            }
        }
        public byte fileCount { get; set; }
        public Appendix2Enum appendix2 { get; set; }

        private byte[] signature;

        public enum Appendix2Enum {
            TYPE_4x00 = 0x00,
            TYPE_4xCC = 0xCC,
            NONE = 0xFF
        }

        public FileSignature(byte[] signature) {
            if (signature.Length != Length)
                throw new FileFormatException(String.Format("File header length is {0}, but it should be {1}bytes long", signature.Length, Length));

            this.signature = signature;

            int pos = 0;
            pos = checkSignature(pos);
            pos = checkSignatureAppendix(pos);
            extraxtUnknownInfo();
            checkAndExtractSecondAppendix();

        }

        private int checkSignature(int start) {
            int pos = start;
            for (int i = 0; i < MagicNumbers.FILE_SIGNATURE.Length; i++) {
                if (signature[i + start] != MagicNumbers.FILE_SIGNATURE[i])
                    throw new FileFormatException(String.Format("File signature wrong"));
                pos = i;
            }
            return pos + 1;
        }
        private int checkSignatureAppendix(int start) {
            int pos = start;
            for (int i = 0; i < MagicNumbers.FILE_SIGNATURE_APPENDIX.Length; i++) {
                if (signature[i + start] != MagicNumbers.FILE_SIGNATURE_APPENDIX[i])
                    throw new FileFormatException(String.Format("File signature appendix wrong"));
                pos = i;
            }
            return pos + 1;
        }
        private void extraxtUnknownInfo() {
            fileCount = signature[MagicNumbers.OFFSET_FILE_COUNT];
        }
        private void checkAndExtractSecondAppendix() {
            appendix2 = Appendix2Enum.NONE;
            int pos = MagicNumbers.OFFSET_SECOND_FILE_SIGNATURE_APPENDIX;
            IEnumerable<Appendix2Enum> secApps = Enum.GetValues(typeof(Appendix2Enum)).Cast<Appendix2Enum>();
            foreach (Appendix2Enum app in secApps) {
                if (signature[pos + 0] == (int)app && signature[pos + 0] == (int)app && signature[pos + 0] == (int)app && signature[pos + 0] == (int)app) {
                    appendix2 = (Appendix2Enum)signature[pos];
                    break;
                }

                if (app == Appendix2Enum.NONE)
                    throw new FileFormatException(
                        String.Format("File signature second appendix is unknown: #{0:X}{1:X}{2:X}{3:X}",
                        signature[pos + 0], signature[pos + 1], signature[pos + 2], signature[pos + 3])
                );
            }
        }
    }
}
