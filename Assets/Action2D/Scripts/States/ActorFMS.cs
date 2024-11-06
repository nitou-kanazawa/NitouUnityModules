using System.Collections.Generic;
using UnityEngine;
using nitou;
using nitou.LevelActors.Controller.Core;
using nitou.LevelActors.Inputs;

namespace Action2D.Actor {
    using Action2D.Actor.Animations;

    public class ActorFMS : nitou.DesignPattern.SimpleFMS<ActorCore, ActorFMS.SetupParam> {

        /// ----------------------------------------------------------------------------
        // Protected Method 

        protected override void OnInitialize(SetupParam param) {
            // イベント登録
            //param.characterActor.OnPreSimulation += PreCharacterSimulation;
            //param.characterActor.OnPostSimulation += PostCharacterSimulation;
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void PreCharacterSimulation(float dt) {
            var state = CurrentState as ActorState;
            state?.PreCharacterSimulation(dt);
        }

        private void PostCharacterSimulation(float dt) {
            var state = CurrentState as ActorState;
            state?.PostCharacterSimulation(dt);
        }


        /// ----------------------------------------------------------------------------

        /// <summary>
        /// <see cref="ActorFMS"/> のセットアップ用データ
        /// </summary>
        public class SetupParam : nitou.DesignPattern.FMSSetupParam {

            public readonly ActorSettings actorSettings;
            public readonly ActorBrain actorBrain;
            public readonly ActorAnimation actorAnimation;

            /// <summary>
            /// コンストラクタ．
            /// </summary>
            public SetupParam(
                ActorSettings actorSettings,
                ActorBrain actorBrain,
                ActorAnimation actorAnimation
            ) {
                Error.ArgumentNullException(actorSettings);
                Error.ArgumentNullException(actorBrain);
                Error.ArgumentNullException(actorAnimation);

                this.actorSettings = actorSettings;
                this.actorBrain = actorBrain;
                this.actorAnimation = actorAnimation;
            }
        }

    }
}
