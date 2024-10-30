using System;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

namespace nitou.AnimationModule{

    [CreateAssetMenu(
        fileName = "New Sequential_AnimSet",    
        menuName = AssetMenu.Prefix.AnimationData + "Sequential AnimSet"
    )]
    public class SequentialAnimSetAsset : ScriptableObject{

        [SerializeField, Indent] ClipTransition _startMotion;
        [SerializeField, Indent] ClipTransition _loopMotion;
        [SerializeField, Indent] ClipTransition _endMotion;


        /// <summary>
        /// 開始アニメーション
        /// </summary>
        public ClipTransition StartClip => _startMotion;

        /// <summary>
        /// ループアニメーション
        /// </summary>
        public ClipTransition LoopClip => _loopMotion;

        /// <summary>
        /// 終了アニメーション
        /// </summary>
        public ClipTransition EndClip => _endMotion;

    }


    /// <summary>
    /// 
    /// </summary>
    public class SequentialAnimStates {

        public readonly AnimancerState start;
        public readonly AnimancerState loop;
        public readonly AnimancerState end;

        public SequentialAnimStates(AnimancerState start, AnimancerState loop, AnimancerState end) {
            this.start = start;
            this.loop = loop;
            this.end = end;
        }
    }


    /// <summary>
    /// <see cref="SequentialAnimSetAsset"/>に関する拡張メソッド集
    /// </summary>
    public static class SequentialAnimSetAssetExtensitons {

        /// <summary>
        /// <see cref="AnimancerComponent"/>に登録する
        /// </summary>
        public static SequentialAnimStates GetOrCreateStates(this AnimancerPlayable.StateDictionary dicti, SequentialAnimSetAsset animSet) {

            // Animancer State
            var startState = dicti.GetOrCreate(animSet.StartClip);
            var loopState = dicti.GetOrCreate(animSet.LoopClip);
            var endState = dicti.GetOrCreate(animSet.EndClip);

            // [NOTE]
            // AnimancerのデフォルトではPlay()するたびにstateのEventsはクリアされている
            // →ここでイベント登録しても意味ない


            return new SequentialAnimStates(startState, loopState, endState);
        }

    }

}
