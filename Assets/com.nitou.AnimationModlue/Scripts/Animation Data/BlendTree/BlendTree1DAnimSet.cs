using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

namespace nitou.AnimationModule{

    public class BlendTree1DAnimSet : ScriptableObject, 
        IAnimSet{

        [Title("Animation Clips")]
        [SerializeField, Indent] protected LinearMixerTransitionAsset _blendTree;


        public LinearMixerTransitionAsset BlendTree => _blendTree;

        public bool HasAnimation => _blendTree != null;
    }
}
