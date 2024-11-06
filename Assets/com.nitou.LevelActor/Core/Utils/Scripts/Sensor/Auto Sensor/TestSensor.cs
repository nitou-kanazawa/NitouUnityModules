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
            _sphereCollider.isTrigger = true; // Trigger��L����
        }


        private void Start() {
            //�ǉ����ɒʒm
            _targetsRP.ObserveAdd().Subscribe(x => {
                Debug.Log(x.Value + "���ǉ�����܂���");
            });

            //�폜���ɒʒm
            _targetsRP.ObserveRemove().Subscribe(x => {
                Debug.Log(x.Value + "���폜����܂���");
            });

            //�l�ύX���ɒʒm
            _targetsRP.ObserveReplace().Subscribe(x => {
                Debug.Log(x.OldValue + "��" + x.NewValue + "�ɒu���������܂���");
            });

            //Clear���ɒʒm
            _targetsRP.ObserveReset().Subscribe(x => {
                Debug.Log("���Z�b�g����܂���");
            });
        }

        private void OnTriggerEnter(Collider otherCol) {

            var obj = otherCol.GetHitObject(_cachingType);

            // �͈͓��ɓ������^�[�Q�b�g���R���N�V�����ɒǉ�
            if (!_targetsRP.Contains(obj)) {
                _targetsRP.Add(obj);
            }
        }

        private void OnTriggerExit(Collider otherCol) {

            var obj = otherCol.GetHitObject(_cachingType);

            // �͈͊O�ɏo���^�[�Q�b�g���R���N�V��������폜
            if (_targetsRP.Contains(obj)) {
                _targetsRP.Remove(obj);
            }
        }

    }
}
