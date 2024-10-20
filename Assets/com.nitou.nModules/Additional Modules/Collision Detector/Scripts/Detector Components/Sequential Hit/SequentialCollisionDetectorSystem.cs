using UnityEngine;
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
                foreach (var component in Components) {
                    component.PrepareFrame();
                }

                // Update collision detection using Physics
                foreach (var component in Components) {
                    component.OnUpdate(in _results);
                }

                //// Raise events related to collided colliders
                //foreach (var component in Components) {
                //    if (component.IsHitInThisFrame) {
                //        component.RaiseOnHitEvent(component._hitObjectsInThisFrame);
                //    }
                //}
            }
        }

    }
}
