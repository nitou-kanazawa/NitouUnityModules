using System;
using System.Collections.Generic;
using UnityEngine;

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
        public override int GetHashCode() => this.hashedKey;
        public override string ToString() => this.name;

        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => !(lhs == rhs);
    }


    [System.Serializable]
    public class Blackboard {

        private Dictionary<string, object> _entries = new();

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
