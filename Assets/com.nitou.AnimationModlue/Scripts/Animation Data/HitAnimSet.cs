using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

namespace nitou.AnimationModule{

    /// <summary>
    /// Hit���̃A�j���[�V�������Ǘ�����A�Z�b�g
    /// </summary>
    [CreateAssetMenu(
        fileName = "New HitAnimationSet",
        menuName = AssetMenu.Prefix.AnimationData + "Hit Animations"
    )]
    public class HitAnimSet : ScriptableObject, IAnimSet {

        [Title("Animation Clips")]

        [InlineEditor]
        [SerializeField, Indent] MixerTransition2DAsset _hitBlendTree;
        [SerializeField, Indent] SequentialAnimSet _knockdown;


        /// <summary>
        /// 
        /// </summary>
        public MixerTransition2DAsset Hit => _hitBlendTree;

        /// <summary>
        /// 
        /// </summary>
        public SequentialAnimSet Knockdown => _knockdown;


        /// ----------------------------------------------------------------------------
        // Public Method

    }
}
