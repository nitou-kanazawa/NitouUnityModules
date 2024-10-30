#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [参考]
//  qiita: AnimationClip.SampleAnimation には Animator コンポーネントが必要 https://qiita.com/neusstudio/items/e98401817cc3b8c21c94

namespace nitou.AnimationModule {
    using nitou.Detecor;

    /// <summary>
    /// アニメーションと関連する処理をシーン編集するためのコンポーネント
    /// </summary>
    [ExecuteAlways]
    public partial class AnimationPreview : MonoBehaviour {

        // General
        private float _playbackSpeed = 1f;
        public static bool autoStopOnDisable = false;

        // Animation
        [SerializeField] private GameObject _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _animationClip;
        private float _animationSpeed = 1f;
        private float _animationDelay = 0f;

        // Particle


        // play
        [SerializeField] private SequentialCollisionDetector _colliders;

        // playback
        private float _playbackStart = 0f;
        private float _playbackEnd = 3f;
        private float _playbackValue = 0f;
        private double _startTime;


        /// ----------------------------------------------------------------------------
        // Properity (Playback Control)

        /// <summary>
        /// シミュレーション実行中かどうか
        /// </summary>
        public bool IsSimulating { get; private set; } = false;

        /// <summary>
        /// ポーズ状態かどうか
        /// </summary>
        public bool IsPaused { get; private set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsScrubbing { get; set; } = false;

        /// <summary>
        /// Animatorの更新処理を行えるかどうか
        /// </summary>
        public bool CanUpdateAnimator => _animator != null && _animationClip != null;

        /// <summary>
        /// Colliderの更新処理を行えるかどうか
        /// </summary>
        public bool CanUpdateCollider => _colliders != null;



        /// ----------------------------------------------------------------------------
        // Properity (Playback Control)

        public float PlaybackSpeed {
            get => _playbackSpeed;
            set => _playbackSpeed = Mathf.Max(0f, value);
        }

        // Save autoRandomSeed
        public static bool AUTO_SAVE_SEED_ON_SIMULATE = true;


        /// ----------------------------------------------------------------------------
        // Properity

        /// <summary>
        /// アニメーションさせる対象オブジェクト
        /// </summary>
        public Animator Animator => _animator;

        /// <summary>
        /// アニメーションクリップ
        /// </summary>
        public AnimationClip Clip => _animationClip;

        /// <summary>
        /// アニメーションの再生速度
        /// </summary>
        public float AnimationSpeed {
            get => _animationSpeed;
            set => _animationSpeed = Mathf.Max(0, value);
        }

        /// <summary>
        /// アニメーションの開始タイミング
        /// </summary>
        public float AnimationDelay {
            get => _animationDelay;
            set => _animationDelay = Mathf.Max(0, value);
        }


        /// ----------------------------------------------------------------------------
        // Properity (Playback Control)

        /// <summary>
        /// 開始時間
        /// </summary>
        public float PlaybackStart {
            get => _playbackStart;
            set {
                _playbackStart = Mathf.Clamp(value, 0, _playbackEnd);
                ClampPlaybackValue();
            }
        }

        /// <summary>
        /// 終了時間
        /// </summary>
        public float PlaybackEnd {
            get => _playbackEnd;
            set {
                _playbackEnd = Mathf.Clamp(value, _playbackStart, float.MaxValue);
                ClampPlaybackValue();
            }
        }

        /// <summary>
        /// 現在のシミュレーション再生時間
        /// </summary>
        public float PlaybackValue {
            get => _playbackValue;
            set {
                _playbackValue = value;
                ClampPlaybackValue();
            }
        }

        /// <summary>
        /// 現在のシミュレーシ再生率
        /// </summary>
        public float PlaybackPercent => _playbackValue / _playbackEnd;


        /// <summary>
        /// 現在のアニメーション再生時間
        /// </summary>
        public float AnimationPlayback => (_animationClip != null)
            ? (_playbackValue - AnimationDelay)
            : 0;

        /// <summary>
        /// 現在のアニメーション再生率 (※NormalizedTime)
        /// </summary>
        public float AnimationPlaybackPercent => (_animationClip != null)
            ? Mathf.Clamp01((_playbackValue - _animationDelay) / _animationClip.length)
            : 0;



        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void OnEnable() {
            EditorApplication.update += SimulateUpdate;
        }

        private void OnDisable() {
            EditorApplication.update -= SimulateUpdate;
        }

        public void Reset() {
            IsSimulating = false;
            IsPaused = true;
        }

        private void OnValidate() {
            if (_target == null) return;

            // Animatorのセットアップ
            if (_target.TryGetComponentInChildren<Animator>(out _animator)) {
                if (CanUpdateAnimator) {
                    _animationClip.SampleAnimation(_animator.gameObject, 0);
                }
            } else {
                Debug_.Log($"{_target.name}にはAnimatorがアタッチされていません．");
                _target = null;
            }
        }

        /// ----------------------------------------------------------------------------
        // Private Method

        public void PlayButton() {
            IsPaused = false;
            StartSimulation(true, false);
        }

        public void PauseButton() {
            IsPaused = true;
            StartSimulation(true, false);
        }

        public void StopButton() {
            IsPaused = true;
            StartSimulation(false, true);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// シミュレーションを開始する
        /// </summary>
        private void StartSimulation(bool _simulate, bool _resetToBeginning) {

            if (_resetToBeginning) {
                _playbackValue = _playbackStart;
            }
            // 
            else {
                _playbackValue = _playbackValue % _playbackEnd;
            }

            _startTime = EditorApplication.timeSinceStartup - (_playbackValue / _playbackSpeed);
            IsSimulating = _simulate;

            // アニメーション更新
            if (CanUpdateAnimator) {
                _animationClip.SampleAnimation(_animator.gameObject, 0f);
            }

            // パーティクル更新
            if (true) {

            }

            // コライダー更新
            if (CanUpdateCollider) {
                _colliders.Rate = 0f;
            }

        }

        /// <summary>
        /// シミュレーションを初めから開始する
        /// </summary>
        public void RestartSimulation() => StartSimulation(true, true);

        /// <summary>
        /// 更新処理
        /// </summary>
        private void SimulateUpdate() {
            if (!IsSimulating) return;

            // 再生中
            if (!IsPaused && !IsScrubbing) {
                // 経過時間
                double dt = (EditorApplication.timeSinceStartup - _startTime) * _playbackSpeed; // ※_playbackSpeedはdtのみに関与
                _playbackValue = (float)(dt) % _playbackEnd;

                if (dt >= _playbackEnd) {
                    RestartSimulation();
                    return;
                }
            }

            // アニメーション更新
            if (CanUpdateAnimator) {
                _animationClip.SampleAnimation(_animator.gameObject, AnimationPlayback);
            }

            // コライダー更新
            if (CanUpdateCollider) {
                _colliders.Rate = AnimationPlaybackPercent;
            }

        }




        /// ----------------------------------------------------------------------------
        // Private Method

        private void ClampPlaybackValue() {
            _playbackValue = Mathf.Clamp(_playbackValue, _playbackStart, _playbackEnd);
        }

        private void Save() {
            if (AUTO_SAVE_SEED_ON_SIMULATE) {
                //foreach (var p in _particlePreviews) {
                //    p.SaveSeed();
                //}
            }
        }

        private void Load() {
            if (AUTO_SAVE_SEED_ON_SIMULATE) {
                //foreach (var p in _particlePreviews) {
                //    p.LoadSeed();
                //}
            }
        }

    }

}
#endif