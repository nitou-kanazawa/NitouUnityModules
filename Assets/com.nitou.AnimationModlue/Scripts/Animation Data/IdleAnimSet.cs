using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

// [�Q�l]
//  Animancer: https://kybernetik.com.au/animancer/docs/examples/animator-controllers/3d-game-kit/idle/

namespace nitou.AnimationModule{

    /// <summary>
    /// Idle��Ԃ̃A�j���[�V�������Ǘ�����A�Z�b�g
    /// </summary>
    [CreateAssetMenu(
        fileName = "New IdelAnimSet",
        menuName = AssetMenu.Prefix.AnimationData + "Idle Animations"
    )]
    public sealed class IdleAnimSet : ScriptableObject, IAnimSet{

        [Title("Animation Clips")]
        [SerializeField, Indent] ClipTransition _mainIdleClip;
        [SerializeField, Indent] ClipTransition[] _randomMotionClips;

        /// <summary>
        /// �ҋ@�A�j���[�V����
        /// </summary>
        public ClipTransition MainClip => _mainIdleClip;

        /// <summary>
        /// �����_�����[�V����
        /// </summary>
        public IReadOnlyList<ClipTransition> RandomMotionClips => _randomMotionClips;

        /// <summary>
        /// �����_�����[�V���������݂��邩�ǂ���
        /// </summary>
        public bool HasRandomMotion => !_randomMotionClips.IsNullOrEmpty();


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// �����_�����[�V�������擾����
        /// </summary>
        public bool TryGetRandomMotionClip(out ClipTransition clip) {
            clip = null;
            
            if (_randomMotionClips.IsNullOrEmpty()) return false;

            // �����_���ɗv�f�擾
            var index = Random.Range(0, _randomMotionClips.Length);
            clip = _randomMotionClips[index];
            return true;
        }
    }
}
