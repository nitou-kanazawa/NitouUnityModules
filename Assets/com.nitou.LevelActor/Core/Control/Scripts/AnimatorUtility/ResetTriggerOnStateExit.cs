using UnityEngine;

namespace nitou.LevelActors.Controller.Smb{

    /// <summary>
    /// 
    /// </summary>
    public sealed class ResetTriggerOnStateExit : StateMachineBehaviour{

        [SerializeField] string _triggerName;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
            animator.ResetTrigger(_triggerName);
        }
    }
    
}

