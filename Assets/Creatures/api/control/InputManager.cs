using UnityEngine;
using System;

namespace Creatures.control
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public event Action<Vector2> OnBasicAttack;
        public event Action<Vector2> OnStartCharging;
        public event Action<Vector2> OnChargedAbility;
        public event Action<Vector2> OnCharacterAbility;
        public event Action<Vector2> OnMobilityAbility;
        public event Action<Vector2> OnCoreInfusionAbility;

        private static int LMB = 0;
        private static int RMB = 1;

        public Vector2 MousePosition => Input.mousePosition;

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
            Vector2 mousePosition = MousePosition;

            if (Input.GetMouseButtonDown(LMB))
            {
                OnBasicAttack?.Invoke(mousePosition);
            }

            // RMB press to start charging
            if (Input.GetMouseButtonDown(RMB))
            {
                OnStartCharging?.Invoke(mousePosition);
            }

            // RMB release to trigger the charged ability
            if (Input.GetMouseButtonUp(RMB))
            {
                OnChargedAbility?.Invoke(mousePosition);
            }
        }

        private void HandleKeyboardInput()
        {
            Vector2 mousePosition = MousePosition;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnCharacterAbility?.Invoke(mousePosition);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                OnMobilityAbility?.Invoke(mousePosition);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnCoreInfusionAbility?.Invoke(mousePosition);
            }
        }
    }
}

