using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 move;
    public bool jump, shoot, dash;

    public void OnMove(InputValue inputValue) => move = inputValue.Get<Vector2>();
    public void OnJump(InputValue inputValue) => jump = inputValue.isPressed;
    public void OnAttack(InputValue inputValue) => shoot = inputValue.isPressed;
    public void OnDash(InputValue inputValue) => dash = inputValue.isPressed;
}