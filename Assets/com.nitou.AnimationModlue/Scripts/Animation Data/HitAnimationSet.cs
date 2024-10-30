using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

namespace nitou.AnimationModule{

    /// <summary>
    /// Hit時のアニメーションを管理するアセット
    /// </summary>
    [CreateAssetMenu(
        fileName = "New HitAnimationSet",
        menuName = AssetMenu.Prefix.AnimationData + "Hit Animation"
    )]
    public class HitAnimationSet : ScriptableObject, IAnimSet {

        [Title("Animation Clips")]

        [InlineEditor]
        [SerializeField, Indent] MixerTransition2DAsset _hitBlentTree;

        [SerializeField, Indent] SequentialAnimSet _knockdown;



        /// <summary>
        /// 
        /// </summary>
        public MixerTransition2DAsset Hit => _hitBlentTree;

        /// <summary>
        /// 
        /// </summary>
        public SequentialAnimSet Knockdown => _knockdown;


        /// ----------------------------------------------------------------------------
        // Public Method

    }
}
