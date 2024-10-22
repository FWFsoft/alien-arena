using UnityEngine;

using UnityEngine.InputSystem;

namespace Input.api {
    public class InputManager : MonoBehavior 
    { 

        private Camera mainCam;

        public Vector3 getMousePos(){
            return mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

    } 
}
