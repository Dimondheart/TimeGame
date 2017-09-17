using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	/**<summary>Get a special/custom control that is not a standard button type or
	 * axis type.</summary>
	 */
	public static T GetSpecialControl<T>(string controlName) where T : DynamicControl
	{
		return (T)specialControls[controlName];
	}

	/**<summary>Base class for for the different variants of dynamic control types.</summary>*/
	public abstract class DynamicControl
	{
		public readonly string description;

		public DynamicControl(string description)
		{
			this.description = description;
		}

		public abstract void UpdateControlStates();
	}

	/**<summary>Base class for dynamic controls with a gamepad input and 1-2
	 * keyboard/mouse inputs.</summary>
	 */
	public abstract class DynamicControl<T> : DynamicControl where T : VirtualInput
	{
		public T gamepadInput { get; private set; }
		public T keyMouseInput { get; private set; }
		public T keyMouseAltInput { get; private set; }

		public DynamicControl(string description) : base(description)
		{
		}

		public override void UpdateControlStates()
		{
			if (gamepadInput != null)
			{
				gamepadInput.UpdateState();
			}
			if (keyMouseInput != null)
			{
				keyMouseInput.UpdateState();
			}
			if (keyMouseAltInput != null)
			{
				keyMouseAltInput.UpdateState();
			}
		}

		public virtual void SetGamepadInput(T newInput)
		{
			gamepadInput = newInput;
		}

		public virtual void SetKeyMouseInput(T newInput)
		{
			keyMouseInput = newInput;
		}

		public virtual void SetKeyMouseAltInput(T newInput)
		{
			keyMouseAltInput = newInput;
		}
	}

	public class DynamicControlButton : DynamicControl<VirtualButton>
	{
		public DynamicControlButton(string description, VirtualButton gamepadButton, VirtualButton keyMouseButton, VirtualButton keyMouseButtonAlt) : base(description)
		{
			SetGamepadInput(gamepadButton);
			SetKeyMouseInput(keyMouseButton);
			SetKeyMouseAltInput(keyMouseButtonAlt);
		}

		public override void SetGamepadInput(VirtualButton newInput)
		{
			base.SetGamepadInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public override void SetKeyMouseInput(VirtualButton newInput)
		{
			base.SetKeyMouseInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public override void SetKeyMouseAltInput(VirtualButton newInput)
		{
			base.SetKeyMouseAltInput(VirtualButtonPlaceholder.GetPlaceholderIfNull(newInput));
		}

		public bool GetButtonDown()
		{
			return gamepadInput.GetButtonDown() || keyMouseInput.GetButtonDown() || keyMouseAltInput.GetButtonDown();
		}

		public bool GetButton()
		{
			return gamepadInput.GetButton() || keyMouseInput.GetButton() || keyMouseAltInput.GetButton();
		}

		public bool GetButtonUp()
		{
			return gamepadInput.GetButtonUp() || keyMouseInput.GetButtonUp() || keyMouseAltInput.GetButtonUp();
		}
	}

	public abstract class VirtualInput
	{
		public virtual void UpdateState()
		{
		}
	}

	public abstract class VirtualButton : VirtualInput
	{
		/**<summary>The button is pressed, and was not pressed the previous update.</summary>*/
		public abstract bool GetButtonDown();
		/**<summary>The button is pressed.</summary>*/
		public abstract bool GetButton();
		/**<summary>The button is not pressed, but was pressed the previous update.</summary>*/
		public abstract bool GetButtonUp();
	}

	public class VirtualButtonPlaceholder : VirtualButton
	{
		public static readonly VirtualButtonPlaceholder placeholder = new VirtualButtonPlaceholder();

		public static VirtualButton GetPlaceholderIfNull(VirtualButton button)
		{
			return (button == null) ? placeholder : button;
		}

		private VirtualButtonPlaceholder()
		{
		}

		public override bool GetButtonDown()
		{
			return false;
		}

		public override bool GetButton()
		{
			return false;
		}

		public override bool GetButtonUp()
		{
			return false;
		}
	}

	/**<summary>A simple button which reads the button state from the Unity
	 * Input class.</summary>
	 */
	public class VirtualButtonBasic : VirtualButton
	{
		public KeyCode keyCode;

		public VirtualButtonBasic(KeyCode keyCode)
		{
			this.keyCode = keyCode;
		}

		public override bool GetButtonDown()
		{
			return Input.GetKeyDown(keyCode);
		}

		public override bool GetButton()
		{
			return Input.GetKey(keyCode);
		}

		public override bool GetButtonUp()
		{
			return Input.GetKeyUp(keyCode);
		}
	}

	/**<summary>Virtual buttons that do not have an existing way to check all
	 * the button states (such as using a controller trigger as a button.) This
	 * class handles state updates and checking for the button down and button
	 * up states, using the GetButton() function implemented by child classes.
	 * </summary>
	 */
	public abstract class VirtualButtonWithState : VirtualButton
	{
		private ButtonState currentState;

		public override void UpdateState()
		{
			switch (currentState)
			{
				case ButtonState.NotPressed:
					if (GetButton())
					{
						currentState = ButtonState.ButtonDown;
					}
					break;
				case ButtonState.ButtonDown:
					if (GetButton())
					{
						currentState = ButtonState.ButtonHeld;
					}
					else
					{
						currentState = ButtonState.ButtonUp;
					}
					break;
				case ButtonState.ButtonHeld:
					if (!GetButton())
					{
						currentState = ButtonState.ButtonUp;
					}
					break;
				case ButtonState.ButtonUp:
					if (GetButton())
					{
						currentState = ButtonState.ButtonDown;
					}
					else
					{
						currentState = ButtonState.NotPressed;
					}
					break;
				default:
					Debug.LogWarning("Invalid button state (int):" + (int)currentState);
					break;
			}
		}

		public override bool GetButtonDown()
		{
			return currentState == ButtonState.ButtonDown;
		}

		public override bool GetButtonUp()
		{
			return currentState == ButtonState.ButtonUp;
		}

		/**<summary>The states a button can be in.</summary>*/
		private enum ButtonState : byte
		{
			NotPressed = 0,
			ButtonDown,
			ButtonHeld,
			ButtonUp
		}
	}

	/**<summary>A virtual button type for using an axis like a button. The button
	 * is considered pressed when the absolute value of the axis is greater than
	 * a minimum value.</summary>
	 */
	public class VirtualButtonFromAxis : VirtualButtonWithState
	{
		public static readonly float minPressValue = 0.2f;

		public string axisName { get; private set; }

		public VirtualButtonFromAxis(string axisName)
		{
			this.axisName = axisName;
		}

		public override bool GetButton()
		{
			return Mathf.Abs(Input.GetAxisRaw(axisName)) >= minPressValue;
		}
	}

	/**<summary>A virtual button type for using an axis like a button, but only
	 * in one direction (positive/negative.) The button is considered pressed when
	 * the axis value is at least minimumValue (or at most -minValue if in the
	 * negative direction.)</summary>
	 */
	public class VirtualButtonFromAxisDirection : VirtualButtonWithState
	{
		public static readonly float minPressValue = 0.2f;
		public static readonly float minPressValueNeg = -0.2f;

		public string axisName { get; private set; }
		public bool positiveDirection { get; private set; }

		public VirtualButtonFromAxisDirection(string axisName, bool positiveDirection)
		{
			this.axisName = axisName;
			this.positiveDirection = positiveDirection;
		}

		public override bool GetButton()
		{
			if (positiveDirection)
			{
				return Input.GetAxisRaw(axisName) >= minPressValue; 
			}
			else
			{
				return Input.GetAxisRaw(axisName) <= minPressValueNeg;
			}
		}
	}

	/**<summary>A static class for accessing controller DPad values independent of
	 * the current platform (however the static properties must be initialized for
	 * the current platform on startup.)</summary>
	 */
	public static class ControllerDPad
	{
		/**<summary>How DPad values need to be read for the current platform.</summary>*/
		public static DPadReadMode dPadReadMode;
		public static string horizontalAxisName;
		public static string verticalAxisName;
		public static KeyCode dPadUp;
		public static KeyCode dPadDown;
		public static KeyCode dPadRight;
		public static KeyCode dPadLeft;

		public static bool UseButtons()
		{
			return (dPadReadMode & DPadReadMode.Button) == DPadReadMode.Button;
		}

		public static bool UseAxes()
		{
			return (dPadReadMode & DPadReadMode.Axis) == DPadReadMode.Axis;
		}

		public static int HorizontalAxis()
		{
			float value = 0.0f;
			if (UseButtons())
			{
				if (Input.GetKey(dPadRight))
				{
					value = 1.0f;
				}
				else if (Input.GetKey(dPadLeft))
				{
					value = -1.0f;
				}
			}
			if (UseAxes() && Mathf.Approximately(0.0f, value))
			{
				value = Input.GetAxis(horizontalAxisName);
			}
			return Mathf.RoundToInt(value);
		}

		public static int VerticalAxis()
		{
			float value = 0.0f;
			if (UseButtons())
			{
				if (Input.GetKey(dPadUp))
				{
					value = 1.0f;
				}
				else if (Input.GetKey(dPadDown))
				{
					value = -1.0f;
				}
			}
			if (UseAxes() && Mathf.Approximately(0.0f, value))
			{
				value = Input.GetAxisRaw(verticalAxisName);
			}
			return Mathf.RoundToInt(value);
		}

		public enum DPadReadMode : byte
		{
			NotConfigured = 0,
			Axis = 1,
			Button = 2,
			Mixed = Axis | Button
		}
	}

	/**<summary>A virtual button from a single direction of the controller DPad.</summary>*/
	public class VirtualButtonFromDPad : VirtualButtonWithState
	{
		public bool horizontalAxis { get; private set; }
		public bool positiveDirection { get; private set; }

		public VirtualButtonFromDPad(bool horizontalAxis, bool positiveDirection)
		{
			this.horizontalAxis = horizontalAxis;
			this.positiveDirection = positiveDirection;
		}
		public override bool GetButton()
		{
			int axisValue = (horizontalAxis) ? ControllerDPad.HorizontalAxis() : ControllerDPad.VerticalAxis();
			if (positiveDirection)
			{
				return axisValue > 0;
			}
			else
			{
				return axisValue < 0;
			}
		}
	}

	/**<summary>Standard dynamic control for a set of virtual axes.</summary>*/
	public class DynamicControlAxis : DynamicControl<VirtualAxis>
	{
		public DynamicControlAxis(string description, VirtualAxis gamepadAxis, VirtualAxis keyMouseAxis) : base(description)
		{
			SetGamepadInput(gamepadAxis);
			SetKeyMouseInput(keyMouseAxis);
		}

		public float GetAxisRaw()
		{
			if (gamepadModeEnabled)
			{
				return gamepadInput.GetAxisRaw();
			}
			return keyMouseInput.GetAxisRaw();
		}

		public float GetAxis()
		{
			if (gamepadModeEnabled)
			{
				return gamepadInput.GetAxis();
			}
			return keyMouseInput.GetAxis();
		}
	}

	/**<summary>Base for any virtual input of an axis type, like a joystick axis.</summary>*/
	public abstract class VirtualAxis : VirtualInput
	{
		public abstract float GetAxisRaw();
		public abstract float GetAxis();
	}

	/**<summary>Basic virtual axis that reads from the Unity Input class.</summary>*/
	public class VirtualAxisBasic : VirtualAxis
	{
		public string axisName;

		public VirtualAxisBasic(string axisName)
		{
			this.axisName = axisName;
		}

		public override float GetAxisRaw()
		{
			return Input.GetAxisRaw(axisName);
		}

		public override float GetAxis()
		{
			return Input.GetAxis(axisName);
		}
	}

	/**<summary>A virtual axis that updates a stored axis value once each update,
	 * and will also (eventually) support automatic smoothing for the non-raw axis value,
	 * simmilar to the Unity Input axis smoothing.</summary>
	 */
	public abstract class VirtualAxisWithBuffer : VirtualAxis
	{
		protected float rawAxisValue;

		public override void UpdateState()
		{
			// Smooth axisValue here
		}

		public override float GetAxisRaw()
		{
			return rawAxisValue;
		}

		public override float GetAxis()
		{
			return rawAxisValue;
		}
	}

	/**<summary>Uses two virtual buttons to make an axis. The virtual buttons
	 * given should not be used within other controls, as this would cause
	 * them to be updated multiple times in one cycle.</summary>
	 */
	public class VirtualAxisFromButtons : VirtualAxisWithBuffer
	{
		public VirtualButton negative { get; private set; }
		public VirtualButton positive { get; private set; }

		public VirtualAxisFromButtons(VirtualButton negative, VirtualButton positive)
		{
			this.negative = negative;
			this.positive = positive;
		}

		public override void UpdateState()
		{
			negative.UpdateState();
			positive.UpdateState();
			if (positive.GetButton())
			{
				if (negative.GetButton())
				{
					rawAxisValue = 0.0f;
				}
				else
				{
					rawAxisValue = 1.0f;
				}
			}
			else if (negative.GetButton())
			{
				rawAxisValue = -1.0f;
			}
			else
			{
				rawAxisValue = 0.0f;
			}
			base.UpdateState();
		}
	}

	/**<summary>A virtual axis that calculates a value in the range [-1,1] using
	 * the position of the mouse relative to the center of the window, with the bounds
	 * being the edges of the player.</summary>
	 */
	public class VirtualAxisFromMouse : VirtualAxisWithBuffer
	{
		public static readonly float deadzone = 0.05f;

		public bool usingMouseX { get; private set; }

		public VirtualAxisFromMouse(bool usingMouseX)
		{
			this.usingMouseX = usingMouseX;
		}

		public override void UpdateState()
		{
			if (usingMouseX)
			{
				rawAxisValue = Input.mousePosition.x / Screen.width * 2.0f - 1.0f;
			}
			else
			{
				rawAxisValue = Input.mousePosition.y / Screen.height * 2.0f - 1.0f;
			}
			if (Mathf.Abs(rawAxisValue) < deadzone)
			{
				rawAxisValue = 0.0f;
			}
			base.UpdateState();
		}
	}
}
