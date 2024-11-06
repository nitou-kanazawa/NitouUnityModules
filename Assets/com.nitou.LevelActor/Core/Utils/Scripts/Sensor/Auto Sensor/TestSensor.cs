using UniRx;
using UnityEngine;

namespace nitou.Detecor {


    public class TestSensor : MonoBehaviour {

        [SerializeField] private SphereCollider _sphereCollider;


        [SerializeField] CachingTarget _cachingType;
        [SerializeField] ReactiveCollection<GameObject> _targetsRP = new();


        public IReadOnlyReactiveCollection<GameObject> Targets => _targetsRP;



        private void Reset() {
            _sphereCollider = gameObject.GetOrAddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true; // Triggerを有効化
        }


        private void Start() {
            //追加時に通知
            _targetsRP.ObserveAdd().Subscribe(x => {
                Debug.Log(x.Value + "が追加されました");
            });

            //削除時に通知
            _targetsRP.ObserveRemove().Subscribe(x => {
                Debug.Log(x.Value + "が削除されました");
            });

            //値変更時に通知
            _targetsRP.ObserveReplace().Subscribe(x => {
                Debug.Log(x.OldValue + "が" + x.NewValue + "に置き換えられました");
            });

            //Clear時に通知
            _targetsRP.ObserveReset().Subscribe(x => {
                Debug.Log("リセットされました");
            });
        }

        private void OnTriggerEnter(Collider otherCol) {

            var obj = otherCol.GetHitObject(_cachingType);

            // 範囲内に入ったターゲットをコレクションに追加
            if (!_targetsRP.Contains(obj)) {
                _targetsRP.Add(obj);
            }
        }

        private void OnTriggerExit(Collider otherCol) {

            var obj = otherCol.GetHitObject(_cachingType);

            // 範囲外に出たターゲットをコレクションから削除
            if (_targetsRP.Contains(obj)) {
                _targetsRP.Remove(obj);
            }
        }

    }
}
