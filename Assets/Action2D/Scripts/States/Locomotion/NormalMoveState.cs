using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using nitou;


namespace Action2D.Actor {
    using StateBase = nitou.DesignPattern.State<ActorCore, ActorFMS.SetupParam>;

    /// <summary>
    /// 戦闘時の基本移動ステート．
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class NormalMoveState : ActorState {

        [Title("Movement")]
        [SerializeField, Indent] bool _canRun = true;
        [SerializeField, Indent] float _speedMultipiler = 1f;

        [Title("Animations")]
        [SerializeField, Indent] LinearMixerTransitionAsset _blendTree;


        // 内部処理用
        private LinearMixerTransitionAsset.UnShared _moveAnim = new();
        private SmoothDampFloat _smoothPlanarVelocity = new(0);


        /// ----------------------------------------------------------------------------
        // Override Method (ステート処理)
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void OnInitialized(ActorFMS.SetupParam param) {
            base.OnInitialized(param);

            // Animation
            _moveAnim.Asset = _blendTree;
            Anim.Animancer.States.GetOrCreate(_moveAnim);
        }

        /// <summary>
        /// ステート開始処理
        /// </summary>
        public override void EnterBehaviour(float dt, StateBase fromState) {

            // 移動コンポーネントの設定
            //Movement.LookingDirection.CurrentMode = LookingDirection.Mode.Movement;  // ※移動方向を向く
            //Movement.PlanarMovement.CanRun = _canRun;
            //Movement.PlanarMovement.SpeedMultipiler = _speedMultipiler;

            // アニメーション再生
            OnStartMotion();
            Anim.Animancer.Play(_moveAnim);
        }

        /// <summary>
        /// ステート更新処理
        /// </summary>
        public override void UpdateBehaviour(float dt) {

            // 移動処理
            MoveControl.Move(InputActions.movement.value);

            //// 垂直移動の更新
            //Movement.VerticalMovement.ProcessGravity(dt);
            ////Context.VerticalMovement.ProcessRegularJump(dt);

            //// 平面移動の更新
            //Movement.PlanarMovement.ProcessMovement(dt, InputActions.movement.value, InputActions.run.value);

            //// 視点操作の更新
            //Movement.LookingDirection.HandleLookingDirection(dt, InputActions.movement.value);

            PostCharacterSimulation(dt);
        }

        /// <summary>
        /// ステート終了処理
        /// </summary>
        public override void ExitBehaviour(float dt, StateBase toState) {

            // アニメーション終了
            OnEndMotion();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method (Animation)

        protected override void OnStartMotion() {
            // アニメーション補間用
            _smoothPlanarVelocity.Reset(MoveControl.CurrentSpeed, 0);
        }

        protected override void OnEndMotion() { }


        /// ----------------------------------------------------------------------------
        // Override Method (遷移条件)

        /// <summary>
        /// 自ステートへの遷移条件
        /// </summary>
        public override bool CheckEnterTransition(StateBase fromState) => true;

        /// <summary>
        /// 他ステートへの遷移条件
        /// </summary>
        public override void CheckExitTransition() {

            // 接地状態の場合，
            //if (Context.IsGrounded) {

            //    // ロックオン開始
            //    if (InputActions.lockon.Canceled && InputActions.lockon.StartedElapsedTime < 0.2f) {
            //        Debug_.Log("Lock On");
            //        Context.IsLockOn = true;
            //        StateMachine.EnqueueTransition<StrafeMoveState>();
            //        return;
            //    }

            //    // 回避ステップ
            //    if (InputActions.dodge.Started) {
            //        StateMachine.EnqueueTransition<DashState>();
            //    }

            //    // 攻撃１
            //    if (InputActions.attack1.Started) {
            //        RequestAttackState(AttackType.Attack1, 1.2f);
            //        return;
            //    }
            //    // 攻撃2
            //    if (InputActions.attack2.Started) {
            //        RequestAttackState(AttackType.Attack2, 1f);
            //        return;
            //    }
            //}

        }


        /// ----------------------------------------------------------------------------
        // Public Method (PhysicsUpdate処理)

        /// <summary>
        /// キャラクターの物理シミュレーション後の処理
        /// </summary>
        public override void PostCharacterSimulation(float dt) {
            // アニメーター速度設定
            var dampTime = (StateMachine.StateElapsedTime < 0.5f) ? 0f : 0.1f;    // ※遷移直後はdampingをOFF
            _moveAnim.State.Parameter = _smoothPlanarVelocity.GetNext(MoveControl.CurrentSpeed, dampTime);
        }


        /// ----------------------------------------------------------------------------
        // Public Method (その他)

        public override string GetName() => "Normal Move";
        public override Color GetColor() => Colors.White;
    }

}