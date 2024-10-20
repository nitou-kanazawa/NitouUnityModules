using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using nitou.BachProcessor;

namespace nitou.Detecor{

    /// <summary>
    /// 交差検出用コンポーネントの基底クラス．
    /// </summary>
    public abstract class DetectorBase : ComponentBase{

        // 実行タイミング
        [DisableInPlayMode]
        [EnumToggleButtons, HideLabel]
        [SerializeField, Indent] protected UpdateTiming Timing = UpdateTiming.Update;

        [Title("Hit Settings")]

        // コライダー保有オブジェクトの判定
        [DisableInPlayMode]
        [SerializeField, Indent] protected CachingTarget _cacheTargetType = CachingTarget.Collider;

        // 対象レイヤー
        [DisableInPlayMode]
        [SerializeField, Indent] protected LayerMask _hitLayer;

        // タグ判定を有無
        [DisableInPlayMode]
        [SerializeField, Indent] protected bool _useHitTag = false;

        // 対象タグ
        [ShowIf("@_useHitTag")]
        [DisableInPlayMode]
        [SerializeField, Indent] protected string[] _hitTagArray;


        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        protected virtual void Reset() {
            // 既定レイヤーは"Default"
            _hitLayer = LayerMaskUtil.OnlyDefault();
        }
#endif
    }
}
