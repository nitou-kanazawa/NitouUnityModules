using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

// [参考]
//  qiita: いい感じのUnity用セーブデータ管理クラス https://qiita.com/tocoteron/items/b865edaa0e3018cb5e55

namespace nitou.SaveSystem {
    using nitou.SaveSystem.Utils;

    public class DataBank {

        static DataBank _instance = new DataBank();
        static Dictionary<string, object> _bank = new Dictionary<string, object>();

        static readonly string path = "SaveData";
        static readonly string fullPath = $"{ Application.persistentDataPath }/{ path }";
        static readonly string extension = "dat";

        public string SavePath => fullPath;

        private DataBank() { }

        public static DataBank Open() {
            return _instance;
        }


        /// ----------------------------------------------------------------------------

        public bool IsEmpty() {
            return _bank.Count == 0;
        }

        public bool ExistsKey(string key) {
            return _bank.ContainsKey(key);
        }


        /// ----------------------------------------------------------------------------

        public void Store(string key, object obj) {
            _bank[key] = obj;
        }

        public void Clear() {
            _bank.Clear();
        }

        public void Remove(string key) {
            _bank.Remove(key);
        }

        public DataType Get<DataType>(string key) {
            if (ExistsKey(key)) {
                return (DataType)_bank[key];
            } else {
                return default(DataType);
            }
        }


        /// ----------------------------------------------------------------------------

        public bool Save(string key) {
            if (!ExistsKey(key)) {
                return false;
            }

            string filePath = $"{ fullPath }/{ key }.{ extension }";

            string json = JsonUtility.ToJson(_bank[key]);

            byte[] data = Encoding.UTF8.GetBytes(json);
            data = Compressor.Compress(data);
            data = Cryptor.Encrypt(data);

            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }

            using (FileStream fileStream = File.Create(filePath)) {
                fileStream.Write(data, 0, data.Length);
            }

            return true;
        }

        public void SaveAll() {
            foreach (string key in _bank.Keys) {
                Save(key);
            }
        }

        public bool Load<DataType>(string key) {
            string filePath = $"{ fullPath }/{ key }.{ extension }";

            if (!File.Exists(filePath)) {
                return false;
            }

            byte[] data = null;
            using (FileStream fileStream = File.OpenRead(filePath)) {
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
            }

            data = Cryptor.Decrypt(data);
            data = Compressor.Decompress(data);

            string json = Encoding.UTF8.GetString(data);

            _bank[key] = JsonUtility.FromJson<DataType>(json);

            return true;
        }
    }
}
