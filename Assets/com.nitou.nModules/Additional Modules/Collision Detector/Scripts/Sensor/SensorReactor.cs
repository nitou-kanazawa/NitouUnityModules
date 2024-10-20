using UnityEngine;
using UnityEngine.Events;

namespace nitou.Detecor{

    /// <summary>
    /// 
    /// </summary>
    public sealed class SensorReactor : MonoBehaviour, ISensorDetectable{

        [SerializeField] UnityEvent OnEnterEvent;
        [SerializeField] UnityEvent OnExitEvent;


        void ISensorDetectable.OnEnter() {
            OnEnterEvent?.Invoke();
        }

        void ISensorDetectable.OnExit() {
            OnExitEvent?.Invoke();
        }
    }
}
