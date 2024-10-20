using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace nitou.Timeline {

    public class ColorTrackMixer : PlayableBehaviour {

        private Renderer _renderer = null;
        private Material _originalMat = null;
        private Material _newMat = null;


        /// ----------------------------------------------------------------------------
        // Override Method (実行時の処理)


        public override void OnBehaviourPause(Playable playable, FrameData info) {
            if (_newMat != null) Object.DestroyImmediate(_newMat);
            if (_renderer != null) _renderer.material = _originalMat;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {

            var renderer = playerData as Renderer;
            if (renderer == null) return;

            if (_newMat == null) {
                // 初期情報の記録
                _renderer = renderer;
                _originalMat = renderer.sharedMaterial;

                // マテリアル生成
                _newMat = new Material(renderer.sharedMaterial);
                renderer.material = _newMat;
            }

            // ブレンドカラーの生成
            var color = Color.clear;
            for (int i = 0; i < playable.GetInputCount(); i++) {
                
                var sp = (ScriptPlayable<ColorClipBehaviour>)playable.GetInput(i);
                
                var behaviour = sp.GetBehaviour();
                var weight = playable.GetInputWeight(i);
                color += behaviour.OutputColor * weight;
            }

            _newMat.color = color;

        }

    }
}
