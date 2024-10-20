using UnityEngine;
using System.Linq;
using nitou.BachProcessor;

namespace nitou.Detecor{
    public partial class SequentialCollisionDetector {

        /// <summary>
        /// <see cref="SequentialCollisionDetector"/>の更新処理を担うクラス
        /// </summary>
        private class SequentialCollisionDetectorSystem :
            SystemBase<SequentialCollisionDetector, SequentialCollisionDetectorSystem>,
            IEarlyUpdate {

            private readonly Collider[] _results = new Collider[CAPACITY];

            // 1度に検出できるコリジョンの最大数.
            private const int CAPACITY = 50;

            /// <summary>
            /// システムの実行順序
            /// </summary>
            int ISystemBase.Order => 0;


            /// ----------------------------------------------------------------------------
            // LifeCycle Events

            private void OnDestroy() {
                UnregisterAllComponents();
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            void IEarlyUpdate.OnUpdate() {

                // Initialize components
                Components.ForEach(c => c.PrepareFrame());

                // Update collision detection using Physics
                Components.ForEach(c => c.OnUpdate(in _results));

                // Raise events related to collided colliders
                Components.Where(c => c.IsHitInThisFrame)
                    .ForEach(c => c.RaiseOnHitEvent(c._hitObjectsInThisFrame));
            }
        }

    }
}
