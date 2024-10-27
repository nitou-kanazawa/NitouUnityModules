using UnityEngine;
using UnityEngine.InputSystem;
using nitou;
using nitou.LevelActors.Control;
using nitou.LevelActors.Effect;

public class TestPlayer : MonoBehaviour
{

    [SerializeField] PlayerInput _input;

    [Space]

    //[SerializeField] Animator _animator;
    [SerializeField] MoveControl _move;
    [SerializeField] JumpControl _jump;
    [SerializeField] ExtraForce _extraForce;


    private InputAction move;

    void Start() {

        move = _input.actions["Move"];
    }


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

        var inputMoveAxis = move.ReadValue<Vector2>();
        _move.Move(inputMoveAxis);
    }

    //private void Update() {
    //    var velociy = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

    //    _move.Move(velociy);
    //}

}
