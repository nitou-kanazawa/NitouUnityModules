using System;
using UniRx;

namespace nitou.LevelActors.Controller.Interfaces.Components{

    /// <summary>
    /// Callback events for landing and leaving
    /// </summary>
    public interface IGravityEvent{

        /// <summary>
        /// ���n���ɒʒm����Observable�D
        /// </summary>
        public IObservable<float> OnLanding { get; }

        /// <summary>
        /// �������ɒʒm����Observable�D
        /// </summary>
        public IObservable<Unit> OnLeave { get; }
    }
}