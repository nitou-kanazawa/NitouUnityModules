using UnityEngine;

namespace nitou.BachProcessor{

    /// <summary>
    /// Base class for components that perform batch processing.
    /// Register this class with a class that inherits from <see cref="SystemBase{TComponent, TSystem}"/> for usage.
    /// </summary>
    public abstract class ComponentBase : MonoBehaviour, IComponentIndex{

        /// <summary>
        /// Index of the component during batch processing.
        /// </summary>
        protected int Index { get; private set; } = -1;

        /// <summary>
        /// True if the component is registered.
        /// </summary>
        protected bool IsRegistered => Index != -1;


        /// ----------------------------------------------------------------------------
        // Interface

        int IComponentIndex.Index{
            get => Index;
            set => Index = value;
        }

        /// <summary>
        /// ìoò^çœÇ›Ç©Ç«Ç§Ç©
        /// </summary>
        bool IComponentIndex.IsRegistered => IsRegistered;
    }
}