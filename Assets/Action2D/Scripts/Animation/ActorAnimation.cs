using System;
using UniRx;
using UnityEngine;
using Animancer;

namespace Action2D.Actor.Animations {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimancerComponent))]
    public sealed class ActorAnimation : MonoBehaviour {
        
        [SerializeField] AnimancerComponent _animancer;

        private readonly Subject<Vector3> _animatorMoveSubject = new();

        /// <summary>
        /// 
        /// </summary>
        public AnimancerComponent Animancer => _animancer;

        /// <summary>
        /// 
        /// </summary>
        public Animator Animator => _animancer.Animator;

        /// <summary>
        /// Animatorが更新されたときに通知するObservable．
        /// </summary>
        public IObservable<Vector3> OnAnimatorMoved => _animatorMoveSubject;


        // Lifecycle Events

        private void Awake() {
            GatherComponents();
        }

        private void OnDestroy() {
            _animatorMoveSubject.Dispose();
        }

        private void OnAnimatorMove() {
            _animatorMoveSubject.OnNext(Animator.velocity);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void GatherComponents() {
            _animancer = GetComponent<AnimancerComponent>();
        }

        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnValidate() {
            GatherComponents();
        }
#endif
    }
}
