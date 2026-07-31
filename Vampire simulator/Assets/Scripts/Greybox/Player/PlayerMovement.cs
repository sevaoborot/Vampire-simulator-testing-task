using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, VampSym_Actions.IPlayerActions //should move the input somewhere else
{
    //movement settings
    [SerializeField] private float _movementSpeed;
    private Vector2 _movementVector;

    //input actions settings
    private VampSym_Actions _actions;
    private VampSym_Actions.PlayerActions _player;

    public void Initialize()
    {
        _actions = new VampSym_Actions();
        _player = _actions.Player;
        _player.AddCallbacks(this);
    }

    private void Update()
    {
        Vector2 movementVector = _movementVector * _movementSpeed * Time.deltaTime;
        transform.position = (Vector2)transform.position + movementVector;
    }

    private void OnEnable()
    {
        _player.Enable();
    }

    private void OnDisable()
    {
        _player.Disable();
    }

    private void OnDestroy()
    {
        _actions.Dispose();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = context.ReadValue<Vector2>();
    }
}
