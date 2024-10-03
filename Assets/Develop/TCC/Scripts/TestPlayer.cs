using UnityEngine;
using UnityEngine.InputSystem;
using nitou.LevelActors.Control;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] MoveControl _move;


    private void OnMove(InputValue value) {
        // MoveAction‚Ì“ü—Í’l‚ğæ“¾
        var axis = value.Get<Vector2>();

        // ˆÚ“®‘¬“x‚ğ•Û
        var velocity = new Vector3(axis.x, 0, axis.y);

        Debug.Log(velocity);
        _move.Move(velocity);
    }

}
