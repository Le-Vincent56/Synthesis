using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Synthesis.Input.GameInputActions;

namespace Synthesis.Input
{
    [CreateAssetMenu(fileName = "Game Input Reader", menuName = "Input/Game Input Reader")]
    public class GameInputReader : ScriptableObject, IUIActions
    {
        public UnityAction<Vector2> Navigate = delegate { };
        public UnityAction<Vector2> Point = delegate { };
        public UnityAction<Vector2> Scrollwheel = delegate { };
        public UnityAction<bool> SpeedUp = delegate { };
        public UnityAction<bool> Submit = delegate { };
        public UnityAction<bool> Cancel = delegate { };
        public UnityAction<bool> Click = delegate { };
        public UnityAction<bool> RightClick = delegate { };
        public UnityAction<bool> MiddleClick = delegate { };
        public UnityAction<bool> AltModifier = delegate { };
        public UnityAction<bool> ShiftModifier = delegate { };

        private GameInputActions inputActions;

        private void OnEnable() => Enable();

        private void OnDisable() => Disable();

        /// <summary>
        /// Enable the input actions
        /// </summary>
        public void Enable()
        {
            // Check if the input actions have been initialized
            if (inputActions == null)
            {
                // Initialize the input actions and set callbacks
                inputActions = new GameInputActions();
                inputActions.UI.SetCallbacks(this);
            }

            // Enable the input actions
            inputActions.Enable();
        }

        /// <summary>
        /// Disable the input actions
        /// </summary>
        public void Disable() => inputActions.Disable();

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Navigate.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            // Invoke the event and pass in the read value
            Point.Invoke(context.ReadValue<Vector2>());
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            // Invoke the event and pass in the read value
            Scrollwheel.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSpeedUp(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    SpeedUp.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    SpeedUp.Invoke(false);
                    break;
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    Submit.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    Submit.Invoke(false);
                    break;
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    Cancel.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    Cancel.Invoke(false);
                    break;
            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    Click.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    Click.Invoke(false);
                    break;
            }
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    RightClick.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    RightClick.Invoke(false);
                    break;
            }
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    MiddleClick.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    MiddleClick.Invoke(false);
                    break;
            }
        }

        public void OnAltModifier(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    AltModifier.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    AltModifier.Invoke(false);
                    break;
            }
        }

        public void OnShiftModifier(InputAction.CallbackContext context)
        {
            // Check the context phase
            switch (context.phase)
            {
                // If starting, invoke with true
                case InputActionPhase.Started:
                    ShiftModifier.Invoke(true);
                    break;

                // If canceled, invoke with false
                case InputActionPhase.Canceled:
                    ShiftModifier.Invoke(false);
                    break;
            }
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
    }
}
