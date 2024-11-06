using UnityEngine;
using UnityEngine.InputSystem;
using nitou;
using nitou.LevelActors.Controller.Control;
using nitou.LevelActors.Controller.Effect;
using nitou.LevelActors.Inputs;

public class TestPlayer : MonoBehaviour
{

    //[SerializeField] PlayerInput _input;
    [SerializeField] ActorBrain _input;

    [Space]

    //[SerializeField] Animator _animator;
    [SerializeField] MoveControl _move;
    [SerializeField] JumpControl _jump;
    [SerializeField] ExtraForce _extraForce;


    private InputAction move;

    //void Start() {

    //    move = _input.actions["Move"];
    //}


    public void OnMove(InputAction.CallbackContext context) { }

    public void OnJump(InputAction.CallbackContext context) {

        if (context.started) {
            Debug_.Log("Jump!!", Colors.Orange);
            _jump.Jump();
        }
    }

    public void OnDash(InputAction.CallbackContext context) {

        if (context.started) {
            Debug_.Log("Dash!!", Colors.DarkCyan);

            var force = transform.forward * 15;
            _extraForce.AddForce(force);
        }
    
    
    }


    private void Update() {
        //_animator.SetFloat("Speed", _move.CurrentSpeed);

        var inputMoveAxis = _input.CharacterActions.movement.value;
        _move.Move(inputMoveAxis);

        if (_input.CharacterActions.attack1.Started) {
            //Debug_.Log("Jump!!", Colors.Orange);
            _jump.Jump();
        }
    }

    //private void Update() {
    //    var velociy = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

    //    _move.Move(velociy);
    //}

}
