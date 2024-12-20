using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Sirenix.OdinInspector;
using nitou;
using nitou.LevelActors.Controller.Core;
using nitou.LevelActors.Controller.Check;
using nitou.LevelActors.Controller.Control;
using nitou.LevelActors.Inputs;

namespace Action2D.Actor {
    using Action2D.Actor.Animations;

    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class ActorCore : MonoBehaviour, ISetupable {

        [Title("Behaviour")]
        [SerializeField, Indent] ActorSettings _settings;
        [SerializeField, Indent] ActorBrain _brain;
        [SerializeField, Indent] ActorFMS _statemachine;

        [Title("Check")]
        [SerializeField, Indent] GroundCheck _groundCheck;

        [Title("Control")]
        [SerializeField, Indent] MoveControl _moveControl;
        [SerializeField, Indent] JumpControl _jumpControl;


        [Title("Animation")]
        [SerializeField, Indent] ActorAnimation _animation;

        //
        private CompositeDisposable _disposables;


        /// ----------------------------------------------------------------------------
        // Properity

        public bool IsSetupped { get; private set; }

        /// <summary>
        /// 接地状態かどうか．
        /// </summary>
        public bool IsGrounded => _groundCheck.IsOnGround;


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Start() {
            Setup();

        }

        private void OnDestroy() {
            Teardown();
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Setup() {
            if (IsSetupped) return;

            // ステートマシン
            var param = new ActorFMS.SetupParam(_settings, _brain, _animation);
            _statemachine.Initialize(this, param);



            // 更新処理の開始
            _disposables = new CompositeDisposable();

            // 
            this.UpdateAsObservable()
                .Subscribe(_ => _statemachine.UpdateProcess())
                .AddTo(_disposables);

            IsSetupped = true;
        }

        public void Teardown() {
            if (!IsSetupped) return;

            _disposables?.Dispose();
            _disposables = null;

            IsSetupped = false;
        }
    }
}
