using System;
using Cysharp.Threading.Tasks;
using UniRx;

// [参考]
//  qiita: 2022年現在におけるUniRxの使いみち https://qiita.com/toRisouP/items/af7d32846ab99f493d92

namespace nitou.GameSystem {
    using nitou.DesignPattern;

    /// <summary>
    /// プロセスの基底クラス
    /// </summary>
    public abstract class ProcessBase : IProcess {

        // State
        private ImtStateMachine<ProcessBase, StateEvent> _stateMachine;

        // Others
        private readonly UniTaskCompletionSource<ProcessResult> _finishedSource = new();
        private IDisposable _disposable;
        private ProcessResult _resultData = null;

        /// <summary>
        /// 終了時の通知
        /// </summary>
        public UniTask<ProcessResult> ProcessFinished => _finishedSource.Task;


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessBase() {

            // 遷移テーブル
            _stateMachine = new ImtStateMachine<ProcessBase, StateEvent>(this);
            {
                // ポーズ
                _stateMachine.AddTransition<RunningState, PauseState>(StateEvent.Pause);
                _stateMachine.AddTransition<PauseState, RunningState>(StateEvent.UnPause);
                // 終了
                _stateMachine.AddTransition<RunningState, EndState>(StateEvent.Complete);
                _stateMachine.AddTransition<PauseState, EndState>(StateEvent.Complete);
                _stateMachine.AddTransition<RunningState, EndState>(StateEvent.Cancel);
                _stateMachine.AddTransition<PauseState, EndState>(StateEvent.Cancel);
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Dispose() {
            _disposable?.Dispose();
            _disposable = null;
        }


        /// ----------------------------------------------------------------------------
        // Public Method (外部操作)

        public void Run() {
            _stateMachine.SetStartState<RunningState>();
            _stateMachine.Update();

            // 更新処理
            _disposable = Observable.EveryUpdate().Subscribe(_ => _stateMachine.Update());
        }
        public void Pause() => _stateMachine.SendEvent(StateEvent.Pause);
        public void UnPause() => _stateMachine.SendEvent(StateEvent.UnPause);
        public void Cancel(CancelResult cancelResult) {
            _stateMachine.SendEvent(StateEvent.Cancel);

            // 結果データの格納
            _resultData = cancelResult ?? new CancelResult();
        }


        /// ----------------------------------------------------------------------------
        // Protected Method 

        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnPause() { Debug_.Log("Pause", Colors.Orange); }
        protected virtual void OnUnPause() { Debug_.Log("Un Pause", Colors.Orange); }
        protected virtual void OnEnd() { }

        /// <summary>
        /// プロセス完了イベントの発火（※派生クラス用）
        /// </summary>
        protected void TriggerComplete(CompleteResult result) {
            _stateMachine.SendEvent(StateEvent.Complete);

            // 結果データの格納
            _resultData = result;
        }


        /// ----------------------------------------------------------------------------
        #region Inner State

        // 遷移イベント        
        private enum StateEvent {
            // ポーズ
            Pause,
            UnPause,
            // 終了
            Complete,
            Cancel,
        }

        private abstract class StateBase : ImtStateMachine<ProcessBase, StateEvent>.State { }

        /// <summary>
        /// 実行ステート
        /// </summary>
        private sealed class RunningState : StateBase {
            private bool isFirstEnter = true;

            protected override void Enter() {
                if (isFirstEnter) {
                    Context.OnStart();
                    isFirstEnter = false;
                }
            }
            protected override void Update() => Context.OnUpdate();
        }

        /// <summary>
        /// ポーズステート
        /// </summary>
        private sealed class PauseState : StateBase {
            protected override void Enter() {
                Context.OnPause();
            }
            protected override void Exit() {
                Context.OnUnPause();
            }
        }

        /// <summary>
        /// 終了ステート
        /// </summary>
        private sealed class EndState : StateBase {
            protected override void Enter() {
                Context.OnEnd();
                // 終了通知
                Debug_.Log($" Result : {Context._resultData.GetType()}", Colors.Orange);
                Context._finishedSource.TrySetResult(Context._resultData);
            }
        }

        #endregion
    }

}
