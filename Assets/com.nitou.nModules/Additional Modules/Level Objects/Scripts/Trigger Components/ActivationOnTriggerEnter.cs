using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace nitou.LevelObjects{

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActivationOnTriggerEnter : MonoBehaviour
    {
        [Title("Detection")]
        [SerializeField, Indent] string playerTag = "Player";
        
        [FoldoutGroup("Reaction")]
        [SerializeField, Indent] List<GameObject> _activatedObjList = new();

        [Space]

        [FoldoutGroup("Reaction")]
        [SerializeField, Indent] UnityEvent OnPlayerEnter;
        
        // 内部処理用
        private readonly ReactiveProperty<bool> _isActiveRP = new(false);



        /// ----------------------------------------------------------------------------
        // LifeCycle Events

        private void Awake() {
            _isActiveRP.Subscribe(isActive => ActivateTargets(isActive));
        }

        private void OnDestroy() {
            _isActiveRP.Dispose();
        }

        private void OnDisable() {
            _isActiveRP.Value = false;
        }

        /// <summary>
        /// 範囲内に入ったときの処理
        /// </summary>
        private void OnTriggerEnter(Collider other) {

            if (other.CompareTag(playerTag)) {
                _isActiveRP.Value = true;
            }
        }

        /// <summary>
        /// 範囲外に出たときの処理
        /// </summary>
        private void OnTriggerExit(Collider other) {

            if (other.CompareTag(playerTag)) {
                _isActiveRP.Value = false;
            }
        }


        /// ----------------------------------------------------------------------------
        // Private Methods

        private void ActivateTargets(bool isActive) {
            if (_activatedObjList.IsNullOrEmpty()) return;

            // アクティブ設定
            _activatedObjList.WithoutNull()
                    .ForEach(o => o.SetActive(isActive));
        }
    }
}
