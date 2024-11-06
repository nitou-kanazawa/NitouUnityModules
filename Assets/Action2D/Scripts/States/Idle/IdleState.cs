using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using nitou;
using nitou.AnimationModule;

namespace Action2D.Actor {
    using Action2D.Actor.Animations;
    using StateBase = nitou.DesignPattern.State<ActorCore, ActorFMS.SetupParam>;

    /// <summary>
    /// �ҋ@�X�e�[�g�D
    /// </summary>
    public sealed class IdleState : ActorState {

        [Title("Animation")]        
        [SerializeField] IdleAnimSet _idleAnimSet;

        [Title("Random Motion")]        
        [LabelText("Interval range")]
        [SerializeField, Indent] RangeFloat _randomizeIntervalRange = new (5,20);

        // ���������p
        private float _randomizeTime;
        private AnimancerState _idleState;


        /// ----------------------------------------------------------------------------
        // Override Method (�X�e�[�g����)

        /// <summary>
        /// ����������
        /// </summary>
        protected override void OnInitialized(ActorFMS.SetupParam param) {
            base.OnInitialized(param);

            // Main animation
            _idleState = Anim.Animancer.States.GetOrCreate(_idleAnimSet.MainClip);
            
            // Random animation
            System.Action onEnd = PlayMainAnimation;
            _idleAnimSet.RandomMotionClips.ForEach(clip => clip.Events.OnEnd = onEnd);
        }

        /// <summary>
        /// �X�e�[�g�J�n����
        /// </summary>
        public override void EnterBehaviour(float dt, StateBase fromState) {

            // �A�j���[�V�����Đ�
            PlayMainAnimation();
            _randomizeTime += _randomizeIntervalRange.Min;
        }

        /// <summary>
        /// �X�e�[�g�X�V����
        /// </summary>
        public override void UpdateBehaviour(float dt) {

            // �����_�����[�V����
            var state = Anim.Animancer.States.Current;
            if (state == _idleState && state.Time >= _randomizeTime) {
                PlayRandomAnimation();
            }
        }

        /// <summary>
        /// �X�e�[�g�I������
        /// </summary>
        public override void ExitBehaviour(float dt, StateBase toState) { }


        /// ----------------------------------------------------------------------------
        // Private Method (�A�j���[�V����)

        /// <summary>
        /// Idle�A�j���[�V�����̍Đ�
        /// </summary>
        private void PlayMainAnimation() {
            Anim.Animancer.Play(_idleState);
            _randomizeTime = _randomizeIntervalRange.Random;
        }

        /// <summary>
        /// �����_���ȃA�j���[�V�������Đ�����
        /// </summary>
        private void PlayRandomAnimation() {
            if (_idleAnimSet.TryGetRandomMotionClip(out var clip)) {
                Anim.Animancer.Play(clip);
            }
        }



        /// ----------------------------------------------------------------------------
        // Public Method (���̑�)

        public override string GetName() => "Idle";
        public override Color GetColor() => Colors.White;
    }
}
