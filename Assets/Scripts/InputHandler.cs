using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    [SerializeField]
    PlayerInput playerInput;

    public bool fire;
    public Vector2 move;
    public bool toggleScreenLock;
    public bool isScreenLocked;
    public bool tabPressed;
    public bool dPressed;
    public bool shouldReset;
    public bool togglePause;

    InputAction fireAction;
    InputAction moveAction;
    InputAction lockScreenAction;
    InputAction toggleLockScreenAction;
    InputAction tabAction;
    InputAction dAction;
    InputAction resetAction;
    InputAction pauseAction;

    private void Awake()
    {
        fireAction = playerInput.actions["Fire"];
        moveAction = playerInput.actions["Move"];
        lockScreenAction = playerInput.actions["LockScreen"];
        toggleLockScreenAction = playerInput.actions["ToggleLockScreen"];
        tabAction = playerInput.actions["TabAction"];
        dAction = playerInput.actions["DAction"];
        resetAction = playerInput.actions["ResetAction"];
        pauseAction = playerInput.actions["PauseAction"];
    }

    // Update is called once per frame
    void Update()
    {
        fire = fireAction.IsPressed();
        move = moveAction.ReadValue<Vector2>();
        isScreenLocked = lockScreenAction.IsPressed();
        toggleScreenLock = toggleLockScreenAction.WasPressedThisFrame();
        tabPressed = tabAction.WasPressedThisFrame();
        dPressed = dAction.WasPressedThisFrame();
        shouldReset = resetAction.WasPressedThisFrame();
        togglePause = pauseAction.WasPressedThisFrame();
    }
}
