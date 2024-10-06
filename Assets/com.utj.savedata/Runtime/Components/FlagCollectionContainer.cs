using System;
using System.Collections.Generic;
using Unity.SaveData.Core;
using UnityEngine;

namespace Unity.SaveData {
    
    [Unity.VisualScripting.RenamedFrom("DataStore.FlagCollectionContainer")]
    public class FlagCollectionContainer : MonoBehaviour, IDataContainer {

        [System.Serializable]
        internal class FlagInfo {
            [SerializeField] private PropertyName _id;
            [SerializeField] private bool _value;

            public PropertyName Id => _id;
            public bool Value {
                get => _value;
                set => _value = value;
            }
        }
        
        [SerializeField] PropertyName _id;

        [FlagCollectionProperty]
        [SerializeField] List<FlagInfo> _values;

        public PropertyName Id => _id;

        public bool GetValue(string flagName) {
            var id = new PropertyName(flagName);

            var flagInfo = _values.Find(c => c.Id == id);
            if (flagInfo == null)
                throw new Exception("Flag Not Found");

            return flagInfo.Value;
        }

        public void SetValue(string flagName, bool newValue) {
            var id = new PropertyName(flagName);
            var flagInfo = _values.Find(c => c.Id == id);
            if (flagInfo == null)
                throw new Exception("Flag Not Found");

            flagInfo.Value = newValue;
        }
        
        private void Reset() {
            _id = new PropertyName("DefaultFlagCollection");
        }
    }
}