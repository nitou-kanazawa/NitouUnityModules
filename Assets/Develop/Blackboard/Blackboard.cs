using System;
using System.Collections.Generic;
using UnityEngine;

// [参考]
//  github: adammyhre/Unity-Behaviour-Trees https://github.com/adammyhre/Unity-Behaviour-Trees

namespace nitou.AI.BlackboardSystem{

    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey> {
        readonly string name;
        readonly int hashedKey;

        public BlackboardKey(string name) {
            this.name = name;
            this.hashedKey = HashUtil.ComputeFNV1aHash(name);
        }

        public bool Equals(BlackboardKey other) => this.hashedKey == other.hashedKey;

        public override bool Equals(object obj) => obj is BlackboardKey other && Equals(other);
        public override int GetHashCode() => this.hashedKey;
        public override string ToString() => this.name;

        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => !(lhs == rhs);
    }


    [Serializable]
    public class BlackboardEntry<T> {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value) {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }


    [System.Serializable]
    public class Blackboard {

        private Dictionary<string, BlackboardKey> _keyRegistry = new();
        private Dictionary<BlackboardKey, object> _entries = new();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Blackboard() {

        }

        /// <summary>
        /// 
        /// </summary>
        public BlackboardKey GetOrRegisterKey(string keyName) {
            if (keyName == null) throw new ArgumentNullException(keyName);

            if(!_keyRegistry.TryGetValue(keyName, out var key)) {
                key = new BlackboardKey(keyName);
                _keyRegistry[keyName] = key;
            }

            return key;
        }

        public void Remove(BlackboardKey key) {
            _entries.Remove(key);
        }

        public bool ContainsKey(BlackboardKey key) {
            return _entries.ContainsKey(key);
        }

        public void SetValue<T>(BlackboardKey key, T value) {
            _entries[key] = new BlackboardEntry<T>(key, value);
        }
    }





    public static class HashUtil {

        public static int ComputeFNV1aHash(string str) {
            uint hash = 2166136261;
            foreach(char c in str) {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }

    }
}
