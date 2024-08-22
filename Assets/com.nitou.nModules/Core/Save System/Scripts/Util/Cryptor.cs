using System.IO;
using System.Text;
using System.Security.Cryptography;

// [参考]
//  qiita: いい感じのUnity用セーブデータ管理クラス https://qiita.com/tocoteron/items/b865edaa0e3018cb5e55

namespace nitou.SaveSystem.Utils {

    /// <summary>
    /// データをAESを用いて暗号化、復号化する静的クラス
    /// </summary>
    public static class Cryptor {

        private static readonly int KEY_SIZE = 256;
        private static readonly int BLOCK_SIZE = 128;

        // 暗号化キー・初期化ベクトル
        private static readonly string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        private static readonly string EncryptionIV = "0123456789ABCDEF";


        /// ----------------------------------------------------------------------------
        // Public Method (暗号化)

        /// <summary>
        /// 規定のパラメータを用いて暗号化する
        /// </summary>
        public static byte[] Encrypt(byte[] rawData) {
            return Encrypt(rawData, EncryptionKey, EncryptionIV);
        }

        /// <summary>
        /// 指定された暗号化キーと初期化ベクトルを用いて暗号化する
        /// </summary>
        public static byte[] Encrypt(byte[] rawData, string key, string iv) {
            byte[] result = null;

            using (AesManaged aes = new AesManaged()) {
                SetAesParams(aes, key, iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream encryptedStream = new MemoryStream()) {
                    using (CryptoStream cryptStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write)) {
                        cryptStream.Write(rawData, 0, rawData.Length);
                    }
                    result = encryptedStream.ToArray();
                }
            }

            return result;
        }


        /// ----------------------------------------------------------------------------
        // Public Method (復号化)

        /// <summary>
        /// 規定のパラメータを用いて復号化する
        /// </summary>
        public static byte[] Decrypt(byte[] encryptedData) {
            return Decrypt(encryptedData, EncryptionKey, EncryptionIV);
        }

        /// <summary>
        /// 指定された暗号化キーと初期化ベクトルを用いて復号化する
        /// </summary>
        public static byte[] Decrypt(byte[] encryptedData, string key, string iv) {
            byte[] result = null;

            using (AesManaged aes = new AesManaged()) {
                SetAesParams(aes, key, iv);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream encryptedStream = new MemoryStream(encryptedData)) {
                    using (MemoryStream decryptedStream = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read)) {
                            cryptoStream.CopyTo(decryptedStream);
                        }
                        result = decryptedStream.ToArray();
                    }
                }
            }

            return result;
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private static void SetAesParams(AesManaged aes, string key, string iv) {
            aes.KeySize = KEY_SIZE;
            aes.BlockSize = BLOCK_SIZE;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.Key = Encoding.UTF8.GetBytes(CreateKeyFromString(key));
            aes.IV = Encoding.UTF8.GetBytes(CreateIVFromString(iv));
        }

        private static string CreateKeyFromString(string str) {
            return PaddingString(str, KEY_SIZE / 8);
        }

        private static string CreateIVFromString(string str) {
            return PaddingString(str, BLOCK_SIZE / 8);
        }

        private static string PaddingString(string str, int len) {

            // [注意]
            //  FromBase64Stringでは、4の倍数の文字数しか受け付けないらしい
            //  そのため文字数が4の倍数になるようにPaddingしている
            //  qiita: Convert.FromBase64Stringがエラーになる https://qiita.com/chanchanko/items/d2a23e8a569eea98d04f

            const char PaddingCharacter = '.';

            if (str.Length < len) {
                string key = str;
                for (int i = 0; i < len - str.Length; ++i) {
                    key += PaddingCharacter;
                }
                return key;
            } else if (str.Length > len) {
                return str.Substring(0, len);
            } else {
                return str;
            }
        }
    }
}
