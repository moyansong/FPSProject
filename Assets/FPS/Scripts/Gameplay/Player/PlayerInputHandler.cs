using FPS.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityStandardAssets.Utility.TimedObjectActivator;

namespace FPS.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Tooltip("Sensitivity multiplier for moving the camera around")]
        public float lookSensitivity = 1f;

        [Tooltip("Additional sensitivity multiplier for WebGL")]
        public float webglLookSensitivityMultiplier = 0.25f;

        [Tooltip("Limit to consider an input when using a trigger on a controller")]
        public float triggerAxisThreshold = 0.4f;

        [Tooltip("Used to flip the vertical input axis")]
        public bool invertYAxis = false;

        [Tooltip("Used to flip the horizontal input axis")]
        public bool invertXAxis = false;

        bool m_FireInputWasHeld;

        // 没有严格执行顺序的输入事件可以在添加到这里,key-GameConstants.
        private static Dictionary<string, UnityAction> m_InputActionsDictionary = new Dictionary<string, UnityAction>();

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            foreach (var action in m_InputActionsDictionary.Values)
            {
                action.Invoke();
            }
        }

        void LateUpdate()
        {
            m_FireInputWasHeld = GetFireInputHeld();
        }

        public static void AddInputAction(string inputName, UnityAction action)
        {
            if (!m_InputActionsDictionary.ContainsKey(inputName))
            {
                m_InputActionsDictionary.TryAdd(inputName, new UnityAction(action));
            }
            else
            {
                m_InputActionsDictionary[inputName] += action;
            }
        }

        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }

        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                Vector3 move = new Vector3(Input.GetAxisRaw(Inputs.k_AxisNameHorizontal), 0f,
                    Input.GetAxisRaw(Inputs.k_AxisNameVertical));

                // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
                move = Vector3.ClampMagnitude(move, 1);

                return move;
            }

            return Vector3.zero;
        }

        public float GetLookInputsHorizontal()
        {
            return GetMouseOrStickLookAxis(Inputs.k_MouseAxisNameHorizontal,
                Inputs.k_AxisNameJoystickLookHorizontal);
        }

        public float GetLookInputsVertical()
        {
            return GetMouseOrStickLookAxis(Inputs.k_MouseAxisNameVertical,
                Inputs.k_AxisNameJoystickLookVertical);
        }

        public bool GetJumpInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Inputs.k_ButtonNameJump);
            }
            
            return false;
        }

        public bool GetJumpInputHeld()
        {
            if (CanProcessInput())
            {
                return Input.GetButton(Inputs.k_ButtonNameJump);
            }

            return false;
        }

        public bool GetFire1InputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Inputs.k_ButtonNameFire1);
            }
            return false;
        }

        public bool GetFire1InputUp()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonUp(Inputs.k_ButtonNameFire1);
            }
            return false;
        }

        public bool GetFireInputHeld()
        {
            if (CanProcessInput())
            {
                //bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) != 0f;
                bool isGamepad = false;
                if (isGamepad)
                {
                    return Input.GetAxis(Inputs.k_ButtonNameGamepadFire) >= triggerAxisThreshold;
                }
                else
                {
                    return Input.GetButton(Inputs.k_ButtonNameFire1);
                }
            }

            return false;
        }
        public bool GetReloadInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Inputs.k_ButtonNameReload);
            }
            return false;
        }

        public bool GetAimInputHeld()
        {
            //if (CanProcessInput())
            //{
            //    bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadAim) != 0f;
            //    bool i = isGamepad
            //        ? (Input.GetAxis(GameConstants.k_ButtonNameGamepadAim) > 0f)
            //        : Input.GetButton(GameConstants.k_ButtonNameAim);
            //    return i;
            //}

            return false;
        }

        public bool GetRunInputHeld()
        {
            if (CanProcessInput())
            {
                return Input.GetButton(Inputs.k_ButtonNameRun);
            }

            return false;
        }

        public bool GetCrouchInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Inputs.k_ButtonNameCrouch);
            }

            return false;
        }

        public bool GetCrouchInputUp()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonUp(Inputs.k_ButtonNameCrouch);
            }

            return false;
        }

        public bool GetReloadButtonDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(Inputs.k_ButtonNameReload);
            }

            return false;
        }

        public int GetSwitchWeaponInput()
        {
            if (CanProcessInput())
            {

                //bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadSwitchWeapon) != 0f;
                bool isGamepad = false;
                string axisName = isGamepad
                    ? Inputs.k_ButtonNameGamepadSwitchWeapon
                    : Inputs.k_ButtonNameSwitchWeapon;

                if (Input.GetAxis(axisName) > 0f)
                    return -1;
                else if (Input.GetAxis(axisName) < 0f)
                    return 1;
                else if (Input.GetAxis(Inputs.k_ButtonNameNextWeapon) > 0f)
                    return -1;
                else if (Input.GetAxis(Inputs.k_ButtonNameNextWeapon) < 0f)
                    return 1;
            }

            return 0;
        }

        public int GetSelectWeaponInput()
        {
            if (CanProcessInput())
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    return 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    return 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    return 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    return 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    return 5;
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                    return 6;
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                    return 7;
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                    return 8;
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                    return 9;
                else
                    return 0;
            }

            return 0;
        }

        float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
        {
            if (CanProcessInput())
            {
                // Check if this look input is coming from the mouse
                bool isGamepad = Input.GetAxis(stickInputName) != 0f;
                float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

                // handle inverting vertical input
                if (invertYAxis)
                    i *= -1f;

                // apply sensitivity multiplier
                i *= lookSensitivity;

                if (isGamepad)
                {
                    // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
                    i *= Time.deltaTime;
                }
                else
                {
                    // reduce mouse input amount to be equivalent to stick movement
                    i *= 0.01f;
#if UNITY_WEBGL
                    // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                    i *= WebglLookSensitivityMultiplier;
#endif
                }

                return i;
            }

            return 0f;
        }
    }
}
