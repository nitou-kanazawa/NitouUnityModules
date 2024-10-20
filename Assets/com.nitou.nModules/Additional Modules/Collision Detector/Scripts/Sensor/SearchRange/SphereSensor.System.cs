using UnityEngine;
using nitou.BachProcessor;

namespace nitou.Detecor {
    public partial class SphereSensor {

        /// <summary>
        /// <see cref="SphereSensor"/>の更新処理を担うクラス
        /// </summary>
        public class SphereSensorSystem :
            SystemBase<SphereSensor, SphereSensorSystem>,
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

            void IEarlyUpdate.OnUpdate() {

                // Update
                Components.ForEach(c => c.OnUpdate(_results));
            }
        }

    }
}
