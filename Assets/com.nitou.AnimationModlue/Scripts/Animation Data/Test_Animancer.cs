using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace nitou.AnimationModule {

    public class Test_Animancer : MonoBehaviour {

        public AnimancerComponent animancer;

        public AnimationClip idle;
        public AnimationClip start;
        public AnimationClip end;

        private AnimancerState startState;
        private AnimancerState endState;



        public ClipTransition startTransidion;
        public ClipTransition endTransidion;

        private AnimancerState startState2;
        private AnimancerState endState2;

        // --

        void Start() {

            // 
            startState = animancer.States.GetOrCreate(start);
            endState = animancer.States.GetOrCreate(end);


            // event
            startState.Events.OnEnd = () => {
                Debug.Log("Onend (start motion)");
                animancer.Play(endState);
            };


            // 
            startState2 = animancer.States.GetOrCreate(startTransidion);
            endState2 = animancer.States.GetOrCreate(endTransidion);


            animancer.Play(idle);
        }

        void Update() {

            if (Input_.KeyDown_Enter()) {
                var state = animancer.Play(startState);

                Debug.Log($"state = startState : {state == startState}");

                state.Events.OnEnd = () => {
                    Debug.Log("Onend (start motion)");
                    animancer.Play(endState);
                };
            }


            if (Input_.KeyDown_Space()) {
                var state = animancer.Play(startState2);
                state.Events.OnEnd = () => {
                    Debug.Log("Onend (start motion)");
                    animancer.Play(endState2);
                };
            }

        }



        public void FuncA() => Debug_.Log("FuncA");
    }
}
