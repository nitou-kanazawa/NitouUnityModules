using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace nitou.Detecor {

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class SequentialCollisionDetector : CollisionHitDetector {

        [Title("Position and timing")]

        [LabelText("Rate")]
        [SerializeField, Indent] NormalizedValue _normalizedValue;

        [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [SerializeField, Indent] List<DetectionBox> _dataList = new();


        /// <summary>
        /// 正規化された値．
        /// （※AnimationClipのNormalizedTimeを流し込む用）
        /// </summary>
        public NormalizedValue Rate {
            get => _normalizedValue;
            set => _normalizedValue = value;
        }

        /// <summary>
        /// 検出に使用するコライダー群．
        /// </summary>
        public IReadOnlyList<DetectionBox> DataList => _dataList;


        /// ----------------------------------------------------------------------------
        // LifeCycle Events

        private void OnEnable() {
            SequentialCollisionDetectorSystem.Register(this, Timing);
            InitializeBufferOfCollidedCollision();
        }

        private void OnDisable() {
            SequentialCollisionDetectorSystem.Unregister(this, Timing);
        }

        /// <summary>
        /// 各フレームでの検出開始時の初期化処理．
        /// </summary>
        private void PrepareFrame() {
            _hitCollidersInThisFrame.Clear();
            _hitObjectsInThisFrame.Clear();
        }

        /// <summary>
        /// 更新処理．
        /// </summary>
        private void OnUpdate(in Collider[] hitColliders) {

            // 各コライダーを検証する
            foreach (var data in _dataList) {

                // コライダーがアクティブ状態かチェック
                if (!data.IsInTimeRange(_normalizedValue)) continue;

                // Perform collision detection.
                var count = CalculateBoxCast(hitColliders, data.volume);

                // Register objects that are not in contact from the list obtained with OverlapSphereNonAlloc(...).
                for (var hitIndex = 0; hitIndex < count; hitIndex++) {
                    var hit = hitColliders[hitIndex];

                    // Exclude own Collider from the collision targets.
                    //if (Owner != null && Owner.IsOwnCollider(hit)) continue;

                    // Get the object judged to have collided.
                    var hitObject = DetectionUtil.GetHitObject(hit, _cacheTargetType);

                    // 既にヒット検出済み，または指定タグではないGameObjectはスキップする
                    // However, if nothing is set in _hitTags, it won't be skipped.
                    if (_hitObjects.Contains(hitObject) || hitObject.ContainTag(_hitTagArray) == false) {
                        continue;
                    }

                    // Register the target.
                    _hitColliders.Add(hit);
                    _hitObjects.Add(hitObject);
                    _hitCollidersInThisFrame.Add(hit);
                    _hitObjectsInThisFrame.Add(hitObject);
                }

            }
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        /// <summary>
        /// Box領域で交差判定を行う．
        /// </summary>
        private int CalculateBoxCast(Collider[] hitColliders, Shapes.Box box) {

            var worldPosition = box.GetWorldPosition(transform);
            var colRotation = box.GetWorldRotaion(transform);

            // 
            var count = Physics.OverlapBoxNonAlloc(
                center: worldPosition,
                halfExtents: box.size * 0.5f,
                results: hitColliders,
                orientation: colRotation,
                mask: _hitLayer,
                queryTriggerInteraction: QueryTriggerInteraction.Ignore);
            return count;
        }



        /// ----------------------------------------------------------------------------
        #region Scene editing

        [ShowInInspector, ReadOnly]
        public DetectionBox SelectedBox { get; private set; } = null;

        /// ----------------------------------------------------------------------------
        // Private Method

        [ButtonGroup("Control Buttons")]
        private void AddData() {
            _dataList.Add(new DetectionBox());
        }

        [ButtonGroup("Control Buttons"), EnableIf("@_dataList.Count >= 1")]
        private void RemoveData() {
            if (_dataList.Count < 1) return;

            _dataList.Remove(SelectedBox);
            SelectedBox = null;
        }

        [ButtonGroup("Control Buttons")]
        private void Sort() {
            if (_dataList.Count < 1) return;
            _dataList = _dataList.OrderBy(data => data.timeRange.x).ToList();
        }

        /// <summary>
        /// 対象のボックスを選択する
        /// </summary>
        internal void Select(DetectionBox target) {
            if (target == null) return;
            var index = _dataList.FindIndex(data => data == target);
            if (index >= 0) {
                SelectedBox = target;
            }
        }

        #endregion

    }
}
