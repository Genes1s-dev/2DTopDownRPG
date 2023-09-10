using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    private PlayerInputActions playerInputActions;
    public event EventHandler OnAimingStarted;
    public event EventHandler OnAimingCanceled;
    public event EventHandler OnHittingPerformed;
    public event EventHandler OnInteractPerformed;

    public event EventHandler OnWeaponSwitchPerformed;
    private bool cooldown = false;

    private void Awake()
    {
        if(Instance != null)
        {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMap.Enable();

        playerInputActions.PlayerMap.Aiming.started += Aiming_starting;
        playerInputActions.PlayerMap.Aiming.canceled += Aiming_canceled;
        playerInputActions.PlayerMap.Hitting.performed += Hitting_performed;
        playerInputActions.PlayerMap.WeaponSwitch.performed += WeaponSwitch_performed;
        playerInputActions.PlayerMap.Interact.performed += Interact_performed;

    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.PlayerMap.Move.ReadValue<Vector2>(); //ассет - action map - actions - чтение значения типа вектор2

        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Aiming_starting(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

            OnAimingStarted?.Invoke(this, EventArgs.Empty);
            //cooldown = true;
            //StartCoroutine(nameof(StartCooldown));
 
    }

    private void Aiming_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAimingCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Hitting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnHittingPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void WeaponSwitch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnWeaponSwitchPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);
    }
    
}
