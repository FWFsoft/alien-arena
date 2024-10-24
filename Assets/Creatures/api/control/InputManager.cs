using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.control
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        private Camera mainCam;

        public event Action<Vector2> OnMove;
        public event Action<Vector3> OnBasicAttack;
        public event Action<Vector3> OnStartCharging;
        public event Action<Vector3> OnChargedAbility;
        public event Action<Vector3> OnCharacterAbility;
        public event Action<Vector3> OnMobilityAbility;
        public event Action<Vector3> OnCoreInfusionAbility;

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

        private void Start()
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Update()
        {
            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleMouseInput()
        {
            Vector3 mousePosition = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            OnMove?.Invoke(moveInput);

            Vector3 mousePosition = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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

