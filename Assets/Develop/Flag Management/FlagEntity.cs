using UnityEngine;

namespace nitou.FlagManagement{

    [System.Serializable]
    public sealed class FlagEntity{

        [SerializeField] private string _id;
        [SerializeField] private bool _enabled;

        public string Id => _id;
        public bool Enabled => _enabled;
    }
}
