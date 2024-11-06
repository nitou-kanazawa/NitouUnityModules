using UnityEngine;

namespace nitou.LevelActors.Controller.Smb{

    /// <summary>
    /// 
    /// </summary>
    public class ResetTriggerOnStateMachineExit : StateMachineBehaviour{

        [SerializeField] string _triggerName;

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash){
            animator.ResetTrigger(_triggerName);
        }
    }
}