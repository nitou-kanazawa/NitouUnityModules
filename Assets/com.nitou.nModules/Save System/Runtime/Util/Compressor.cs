using System.IO;
using System.IO.Compression;

namespace nitou.SaveSystem.Utils {

    /// <summary>
    /// バイナリデータをgzipフォーマットに従って圧縮、解凍する静的クラス
    /// </summary>
    public static class Compressor{

        /// <summary>
        /// データを圧縮する
        /// </summary>
        public static byte[] Compress(byte[] rawData) {
            byte[] result = null;

            using (MemoryStream compressedStream = new MemoryStream()) {
                using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Compress)) {
                    gZipStream.Write(rawData, 0, rawData.Length);
                }
                result = compressedStream.ToArray();
            }

            return result;
        }

        /// <summary>
        /// データを解凍する
        /// </summary>
        public static byte[] Decompress(byte[] compressedData) {
            byte[] result = null;

            using (MemoryStream compressedStream = new MemoryStream(compressedData)) {
                using (MemoryStream decompressedStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress)) {
                        gZipStream.CopyTo(decompressedStream);
                    }
                    result = decompressedStream.ToArray();
                }
            }

            return result;
        }
    }
}
