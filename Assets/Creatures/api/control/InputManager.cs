using UnityEngine;
using System;

namespace Creatures.control
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public event Action OnBasicAttack;
        public event Action OnStartCharging;
        public event Action OnChargedAbility;
        public event Action OnCharacterAbility;
        public event Action OnMobilityAbility;
        public event Action OnCoreInfusionAbility;

        private static int LMB = 0;
        private static int RMB = 1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(LMB))
            {
                OnBasicAttack?.Invoke();
            }

            // RMB press to start charging
            if (Input.GetMouseButtonDown(RMB))
            {
                OnStartCharging?.Invoke();
            }

            // RMB release to trigger the charged ability
            if (Input.GetMouseButtonUp(RMB))
            {
                OnChargedAbility?.Invoke();
            }
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnCharacterAbility?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                OnMobilityAbility?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnCoreInfusionAbility?.Invoke();
            }
        }
    }
}

