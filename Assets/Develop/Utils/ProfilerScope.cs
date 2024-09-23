using UnityEngine.Profiling;

// [éQçl]
//  Unity Document: Profiler.BeginSample https://docs.unity3d.com/ja/current/ScriptReference/Profiling.Profiler.BeginSample.html

namespace nitou {

    public readonly struct ProfilerScope : System.IDisposable{

        public ProfilerScope(string name) {
            Profiler.BeginSample(name);
        }

        public void Dispose() {
            Profiler.EndSample();
        }
    }
}
