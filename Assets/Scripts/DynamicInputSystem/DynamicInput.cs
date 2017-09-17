using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TechnoWolf.DynamicInputSystem
{
	/**<summary>Interface for the Unity Input that can be dynamically changed
	 * during runtime. Use instead of the Unity Input class for virtual
	 * controls such as buttons and axes.</summary>
	 */
	public class DynamicInput : MonoBehaviour
	{
		/**<summary>Internal for GamepadModeEnabled.</summary>*/
		private static bool gamepadModeEnabled = false;
		/**<summary>Button-type controls</summary>*/
		private static Dictionary<string, DynamicControlButton> buttonControls =
			new Dictionary<string, DynamicControlButton>();
		private static Dictionary<string, DynamicControlAxis> axisControls =
			new Dictionary<string, DynamicControlAxis>();
		/**<summary>The current virtual controls.</summary>*/
		private static Dictionary<string, DynamicControl> specialControls =
			new Dictionary<string, DynamicControl>();

		public static bool GamepadModeEnabled
		{
			get
			{
				return gamepadModeEnabled;
			}
			set
			{
				if (value == gamepadModeEnabled)
				{
					return;
				}
				gamepadModeEnabled = value;
				Cursor.visible = !value;
				if (value)
				{
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
				}
			}
		}
		/**<summary>The current mouse position converted to the range of (-1, 1), where
		 * the original range is (-screen_size/2, screen_size/2).</summary>
		 */
		public static Vector2 MouseAsJoystick
		{
			get
			{
				return new Vector2(
					Input.mousePosition.x / Screen.width * 2.0f - 1.0f,
					Input.mousePosition.y / Screen.height * 2.0f - 1.0f
					);
			}
		}

		private void Awake()
		{
			// Only setup once, since this is a 'static' component
			if (buttonControls.Count > 0 || axisControls.Count > 0 || specialControls.Count > 0)
			{
				return;
			}
			DynamicInputConfiguration.ConfigureInput();
		}

		private void Update()
		{
			foreach (DynamicControl dc in buttonControls.Values)
			{
				dc.UpdateControlStates();
			}
			foreach (DynamicControl dc in axisControls.Values)
			{
				dc.UpdateControlStates();
			}
			foreach (DynamicControl dc in specialControls.Values)
			{
				dc.UpdateControlStates();
			}
		}

		/**<summary>Add the specified dynamic control as a standard button,
		 * replacing any control under the same name that was also added as a 
		 * standard button.</summary>
		 */
		public static void SetupButtonControl(string name, DynamicControlButton control)
		{
			buttonControls[name] = control;
		}

		/**<summary>Add the specified dynamic control as a standard axis,
		 * replacing any control under the same name that was also added as a 
		 * standard axis.</summary>
		 */
		public static void SetupAxisControl(string name, DynamicControlAxis control)
		{
			axisControls[name] = control;
		}

		/**<summary>Add the specified dynamic control as a special control,
		 * replacing any control under the same name that was also added as a 
		 * special control.</summary>
		 */
		public static void SetupSpecialControl(string name, DynamicControl control)
		{
			specialControls[name] = control;
		}

		/**<summary>Remove all registered controls.</summary>*/
		public static void RemoveAllControls()
		{
			buttonControls.Clear();
			axisControls.Clear();
			specialControls.Clear();
		}

		/**<summary>Input.GetButtonDown equivalent.</summary>*/
		public static bool GetButtonDown(string name)
		{
			return buttonControls[name].GetButtonDown();
		}

		/**<summary>Input.GetButton equivalent.</summary>*/
		public static bool GetButtonHeld(string controlName)
		{
			return buttonControls[controlName].GetButton();
		}

		/**<summary>Input.GetButtonUp equivalent.</summary>*/
		public static bool GetButtonUp(string controlName)
		{
			return buttonControls[controlName].GetButtonUp();
		}

		/**<summary>Input.GetAxisRaw equivalent.</summary>*/
		public static float GetAxisRaw(string controlName)
		{
			return axisControls[controlName].GetAxisRaw();
		}

		/**<summary>Input.GetAxis equivalent.</summary>*/
		public static float GetAxis(string controlName)
		{
			return axisControls[controlName].GetAxis();
		}

		/**<summary>Get the class for a button control in order to change it.</summary>*/
		public static DynamicControlButton GetButtonControl(string controlName)
		{
			if (buttonControls.ContainsKey(controlName))
			{
				return buttonControls[controlName];
			}
			return null;
		}

		/**<summary>Get the class for an axis control in order to change it.</summary>*/
		public static DynamicControlAxis GetAxisControl(string controlName)
		{
			if (axisControls.ContainsKey(controlName))
			{
				return axisControls[controlName];
			}
			return null;
		}

		/**<summary>Get a special/custom control that is not a standard button type or
		 * axis type.</summary>
		 */
		public static T GetSpecialControl<T>(string controlName) where T : DynamicControl
		{
			if (specialControls.ContainsKey(controlName))
			{
				return (T)specialControls[controlName];
			}
			return null;
		}
	}
}
