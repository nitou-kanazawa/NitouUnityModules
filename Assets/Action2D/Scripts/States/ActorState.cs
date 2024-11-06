using System.Collections.Generic;
using UnityEngine;
using Animancer;
using nitou.LevelActors;
using nitou.LevelActors.Controller.Core;
using nitou.LevelActors.Controller.Control;
using nitou.LevelActors.Inputs;

namespace Action2D.Actor{
    using Action2D.Actor.Animations;

    /// <summary>
    /// �A�N�^�[�̃X�e�[�g���N���X
    /// </summary>
    public abstract class ActorState : nitou.DesignPattern.State<ActorCore, ActorFMS.SetupParam> {

        // Reference
        //public CharacterActor Actor { get; private set; }
        public ActorSettings Settings { get; private set; }
        public ActorBrain Brain { get; private set; }
        public ActorAnimation Anim { get; private set; }
        //public CharacterMovement Movement { get; private set; }

        // Control
        protected MoveControl MoveControl { get; private set; }
        protected JumpControl JumpControl { get; private set; }


        /// <summary>
        /// ���̓A�N�V����
        /// </summary>
        public ActorActions InputActions => Brain.CharacterActions;


        /// ----------------------------------------------------------------------------
        // Public Method (PhysicsUpdate����)

        /// <summary>
        /// �L�����N�^�[�̕����V�~�����[�V�����O�̏���
        /// </summary>
        public virtual void PreCharacterSimulation(float dt) { }

        /// <summary>
        /// �L�����N�^�[�̕����V�~�����[�V������̏���
        /// </summary>
        public virtual void PostCharacterSimulation(float dt) { }


        /// ----------------------------------------------------------------------------
        // Protected Method (Animation)

        protected virtual void OnStartMotion() { }
        protected virtual void OnEndMotion() { }


        /// ----------------------------------------------------------------------------
        // Protected Method

        /// <summary>
        /// ����������
        /// </summary>
        protected override void OnInitialized(ActorFMS.SetupParam param) {
            // Reference
            Settings = param.actorSettings;
            Brain = param.actorBrain;
            Anim = param.actorAnimation;

            // Control
            MoveControl = Settings.GetActorComponent<MoveControl>(ActorComponent.Control);
            JumpControl = Settings.GetActorComponent<JumpControl>(ActorComponent.Control);
        }


        /// ----------------------------------------------------------------------------
        // Protected Method (State Transition)

        /*

        /// <summary>
        /// AttackState�ւ̑J�ڃ��N�G�X�g
        /// </summary>
        protected void RequestAttackState(AttackType type, float speed = 1f) {
            var attackState = StateMachine.GetState<AttackState>();
            if (attackState == null) return;

            attackState.SetNextAnimation(type, speed);
            StateMachine.EnqueueTransition(attackState);
        } 
        
        */
    }
}
