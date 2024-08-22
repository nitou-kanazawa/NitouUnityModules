using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.HitSystem {

    /*

    /// <summary>
    /// Enableな間，独自PlayerLoop上で干渉検出を行うコンポーネント
    /// </summary>
    public partial class SequentialCollisionDetector : CollisionDetectorBase, INormalizedValueTicker,
        IDataContainer<DetectionBox>, IDataSelector<DetectionBox>{

        [Title("Position and timing")]

        [LabelText("Rate")]
        [SerializeField, Indent] NormalizedValue _normalizedValue;

        /// <summary>
        /// 正規化された値（※AnimationClipのNormalizedTimeを流し込む用）
        /// </summary>
        public NormalizedValue Rate {
            get => _normalizedValue;
            set => _normalizedValue = value;
        }





        [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [SerializeField, Indent] List<DetectionBox> _dataList = new();


        /// ----------------------------------------------------------------------------
        // Properity


        /// <summary>
        /// 検出に使用するコライダー群
        /// </summary>
        public IReadOnlyList<DetectionBox> DataList => _dataList;





        [ShowInInspector, ReadOnly]
        public CollisionBox SelectedBox { get; private set; } = null;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void OnEnable() {
            MultiStepCollisionDetectorSystem.Register(this, Timing);
            InitializeBufferOfCollidedCollision();      // ※キャッシュをクリア
        }

        private void OnDisable() {
            MultiStepCollisionDetectorSystem.Unregister(this, Timing);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        [ButtonGroup("Control Buttons")]
        private void AddData() {
            _dataList.Add(new DetectionBox());
        }

        [ButtonGroup("Control Buttons")]
        private void RemoveData() {
            if (_dataList.Count < 1) return;
            _dataList.RemoveAt(_dataList.Count - 1);
        }

        [ButtonGroup("Control Buttons")]
        private void Sort() {
            if (_dataList.Count < 1) return;
            _dataList = _dataList.OrderBy(data => data.timeRange.x).ToList();
        }

        /// <summary>
        /// 対象のボックスを選択する
        /// </summary>
        internal void Select(CollisionBox box) {
            if (box == null) return;
            var index = _dataList.FindIndex(data => data.volume == box);
            if (index >= 0) {
                SelectedBox = box;
            }
        }



        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// 各フレームでの検出開始時の初期化処理
        /// </summary>
        private void PrepareFrame() {
            _hitCollidersInThisFrame.Clear();
            _hitObjectsInThisFrame.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnUpdate(in Collider[] hitColliders) {

            // 各コライダーを検証する
            foreach (var data in _dataList) {

                // コライダーがアクティブ状態かチェック
                if (!data.IsInTimeRange(_normalizedValue)) continue;

                // Perform collision detection.
                var count = CalculateBoxCast(hitColliders, data.volume);

                // Register objects that are not in contact from the list obtained with OverlapBoxNonAlloc(...).
                for (var hitIndex = 0; hitIndex < count; hitIndex++) {
                    var hit = hitColliders[hitIndex];

                    // *****
                    // Exclude own Collider from the collision targets.
                    //if (Owner != null && Owner.IsOwnCollider(hit)) continue;
                    // *****

                    // Get the object judged to have collided.
                    var hitObject = base.GetHitObject(hit);

                    // 既にヒット検出済み，または指定タグではないGameObjectはスキップする
                    // However, if nothing is set in _hitTags, it won't be skipped.
                    if (_hitObjects.Contains(hitObject) || hitObject.ContainTag(_hitTagArray) == false) {
                        continue;
                    }

                    // コライダーを登録する
                    _hitObjects.Add(hitObject);
                    _hitCollidersInThisFrame.Add(hit);
                    _hitObjectsInThisFrame.Add(hitObject);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int CalculateBoxCast(Collider[] hitColliders, CollisionBox box) {
            var worldPosition = box.GetWorldPosition(transform);
            var colRotation = box.GetWorldRotaion(transform);
            var count = Physics.OverlapBoxNonAlloc(worldPosition, box.size * 0.5f,
                hitColliders, colRotation, _hitLayer, QueryTriggerInteraction.Ignore);
            return count;
        }


    }


    */
}
