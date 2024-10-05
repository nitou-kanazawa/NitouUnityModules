using UnityEngine;

namespace nitou.BachProcessor{

    /// <summary>
    /// バッチ処理対象のコンポーネント基底クラス．
    /// Register this class with a class that inherits from <see cref="SystemBase{TComponent, TSystem}"/> for usage.
    /// </summary>
    public abstract class ComponentBase : MonoBehaviour, IComponentIndex{

        protected int Index { get; private set; } = -1;
        protected bool IsRegistered => Index != -1;


        /// ----------------------------------------------------------------------------
        // Interface

        /// <summary>
        /// Index of the component during batch processing.
        /// </summary>
        int IComponentIndex.Index{
            get => Index;
            set => Index = value;
        }

        /// <summary>
        /// システムへ登録済みかどうか
        /// </summary>
        bool IComponentIndex.IsRegistered => IsRegistered;
    }
}