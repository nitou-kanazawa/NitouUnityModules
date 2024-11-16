using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using nitou.DesignPattern;
using nitou.Inspector;

namespace nitou.LevelObjects{


    public class Chest : MonoBehaviour{

        [TagSelector]
        [SerializeField] string _targetTag = "Player";

        [SerializeField] Animator _animator;

        [Space]
        [SerializeField] GameObject _treasureObj;


        private readonly string openAnimName = "Chest_Open";
        private readonly string closeAnimName = "Chest_Close";


        private enum Event {
            EnterRange,
            ExitRange,
        }

        private ImtStateMachine<Chest, Event> _stateMachine;


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Awake() {

            _stateMachine = new ImtStateMachine<Chest, Event>(this);

            // 
            this.OnTriggerEnterAsObservable()
                .Where(c => c.gameObject.CompareTag(_targetTag))
                .Subscribe(_ => _animator.Play(openAnimName))
                .AddTo(this);

            // 
            this.OnTriggerExitAsObservable()
                .Where(c => c.gameObject.CompareTag(_targetTag))
                .Subscribe(_ => _animator.Play(closeAnimName))
                .AddTo(this);

        }

        private void OnDestroy() {
            
        }




    }
}
