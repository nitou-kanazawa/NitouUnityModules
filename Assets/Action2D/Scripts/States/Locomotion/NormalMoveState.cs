using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using nitou;


namespace Action2D.Actor {
    using StateBase = nitou.DesignPattern.State<ActorCore, ActorFMS.SetupParam>;

    /// <summary>
    /// �퓬���̊�{�ړ��X�e�[�g�D
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class NormalMoveState : ActorState {

        [Title("Movement")]
        [SerializeField, Indent] bool _canRun = true;
        [SerializeField, Indent] float _speedMultipiler = 1f;

        [Title("Animations")]
        [SerializeField, Indent] LinearMixerTransitionAsset _blendTree;


        // ���������p
        private LinearMixerTransitionAsset.UnShared _moveAnim = new();
        private SmoothDampFloat _smoothPlanarVelocity = new(0);


        /// ----------------------------------------------------------------------------
        // Override Method (�X�e�[�g����)
        
        /// <summary>
        /// ����������
        /// </summary>
        protected override void OnInitialized(ActorFMS.SetupParam param) {
            base.OnInitialized(param);

            // Animation
            _moveAnim.Asset = _blendTree;
            Anim.Animancer.States.GetOrCreate(_moveAnim);
        }

        /// <summary>
        /// �X�e�[�g�J�n����
        /// </summary>
        public override void EnterBehaviour(float dt, StateBase fromState) {

            // �ړ��R���|�[�l���g�̐ݒ�
            //Movement.LookingDirection.CurrentMode = LookingDirection.Mode.Movement;  // ���ړ�����������
            //Movement.PlanarMovement.CanRun = _canRun;
            //Movement.PlanarMovement.SpeedMultipiler = _speedMultipiler;

            // �A�j���[�V�����Đ�
            OnStartMotion();
            Anim.Animancer.Play(_moveAnim);
        }

        /// <summary>
        /// �X�e�[�g�X�V����
        /// </summary>
        public override void UpdateBehaviour(float dt) {

            // �ړ�����
            MoveControl.Move(InputActions.movement.value);

            //// �����ړ��̍X�V
            //Movement.VerticalMovement.ProcessGravity(dt);
            ////Context.VerticalMovement.ProcessRegularJump(dt);

            //// ���ʈړ��̍X�V
            //Movement.PlanarMovement.ProcessMovement(dt, InputActions.movement.value, InputActions.run.value);

            //// ���_����̍X�V
            //Movement.LookingDirection.HandleLookingDirection(dt, InputActions.movement.value);

            PostCharacterSimulation(dt);
        }

        /// <summary>
        /// �X�e�[�g�I������
        /// </summary>
        public override void ExitBehaviour(float dt, StateBase toState) {

            // �A�j���[�V�����I��
            OnEndMotion();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method (Animation)

        protected override void OnStartMotion() {
            // �A�j���[�V������ԗp
            _smoothPlanarVelocity.Reset(MoveControl.CurrentSpeed, 0);
        }

        protected override void OnEndMotion() { }


        /// ----------------------------------------------------------------------------
        // Override Method (�J�ڏ���)

        /// <summary>
        /// ���X�e�[�g�ւ̑J�ڏ���
        /// </summary>
        public override bool CheckEnterTransition(StateBase fromState) => true;

        /// <summary>
        /// ���X�e�[�g�ւ̑J�ڏ���
        /// </summary>
        public override void CheckExitTransition() {

            // �ڒn��Ԃ̏ꍇ�C
            //if (Context.IsGrounded) {

            //    // ���b�N�I���J�n
            //    if (InputActions.lockon.Canceled && InputActions.lockon.StartedElapsedTime < 0.2f) {
            //        Debug_.Log("Lock On");
            //        Context.IsLockOn = true;
            //        StateMachine.EnqueueTransition<StrafeMoveState>();
            //        return;
            //    }

            //    // ����X�e�b�v
            //    if (InputActions.dodge.Started) {
            //        StateMachine.EnqueueTransition<DashState>();
            //    }

            //    // �U���P
            //    if (InputActions.attack1.Started) {
            //        RequestAttackState(AttackType.Attack1, 1.2f);
            //        return;
            //    }
            //    // �U��2
            //    if (InputActions.attack2.Started) {
            //        RequestAttackState(AttackType.Attack2, 1f);
            //        return;
            //    }
            //}

        }


        /// ----------------------------------------------------------------------------
        // Public Method (PhysicsUpdate����)

        /// <summary>
        /// �L�����N�^�[�̕����V�~�����[�V������̏���
        /// </summary>
        public override void PostCharacterSimulation(float dt) {
            // �A�j���[�^�[���x�ݒ�
            var dampTime = (StateMachine.StateElapsedTime < 0.5f) ? 0f : 0.1f;    // ���J�ڒ����damping��OFF
            _moveAnim.State.Parameter = _smoothPlanarVelocity.GetNext(MoveControl.CurrentSpeed, dampTime);
        }


        /// ----------------------------------------------------------------------------
        // Public Method (���̑�)

        public override string GetName() => "Normal Move";
        public override Color GetColor() => Colors.White;
    }

}