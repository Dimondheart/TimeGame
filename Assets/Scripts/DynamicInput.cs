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
	private static Dictionary<string, DynamicControlButton> buttonControls =
		new Dictionary<string, DynamicControlButton>();
	/**<summary>The current virtual controls.</summary>*/
	private static Dictionary<string, DynamicControl> virtualControls =
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
		if (virtualControls.Count > 0 || buttonControls.Count > 0)
		{
			return;
		}
		DynamicInputConfiguration.ConfigureInput(buttonControls, virtualControls);
	}

	private void Update()
	{
		foreach (DynamicControl dc in buttonControls.Values)
		{
			dc.UpdateControlStates();
		}
		foreach (DynamicControl dc in virtualControls.Values)
		{
			dc.UpdateControlStates();
		}
	}

	/**<summary>Input.GetButtonDown equivalent.</summary>*/
	public static bool GetButtonDown(string virtualName)
	{
		return buttonControls[virtualName].GetButtonDown();
	}

	/**<summary>Input.GetButton equivalent.</summary>*/
	public static bool GetButton(string virtualName)
	{
		return buttonControls[virtualName].GetButton();
	}

	/**<summary>Input.GetButtonUp equivalent.</summary>*/
	public static bool GetButtonUp(string virtualName)
	{
		return buttonControls[virtualName].GetButtonUp();
	}

	/**<summary>Input.GetAxisRaw equivalent.</summary>*/
	public static float GetAxisRaw(string virtualName)
	{
		return ((DynamicControlAxis2)virtualControls[virtualName]).RawAxis;
	}

	/**<summary>Input.GetAxis equivalent.</summary>*/
	public static float GetAxis(string virtualName)
	{
		return ((DynamicControlAxis2)virtualControls[virtualName]).Axis;
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
			gamepadInput.UpdateState();
			keyMouseInput.UpdateState();
			keyMouseAltInput.UpdateState();
		}

		public virtual void SetGamepadInput(T newInput)
		{
			if (newInput == null)
			{
				Debug.LogError("Attempted to set a dynamic control input to null." +
					" Use a designated placeholder instance instead of null."
					);
				return;
			}
			gamepadInput = newInput;
		}

		public virtual void SetKeyMouseInput(T newInput)
		{
			if (newInput == null)
			{
				Debug.LogError("Attempted to set a dynamic control input to null." +
					" Use a designated placeholder instance instead of null."
					);
				return;
			}
			keyMouseInput = newInput;
		}

		public virtual void SetKeyMouseAltInput(T newInput)
		{
			if (newInput == null)
			{
				Debug.LogError("Attempted to set a dynamic control input to null." +
					" Use a designated placeholder instance instead of null."
					);
				return;
			}
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

	/**<summary></summary>*/
	public abstract class VirtualAxis : VirtualInput
	{
		public abstract float GetAxisRaw();
		public abstract float GetAxis();
	}

	/**<summary></summary>*/
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
	 * them to be updated multiple times in one cycle.</summary>*/
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

	public class VirtualAxisFromMouse : VirtualAxisWithBuffer
	{
		public bool useHorizontal { get; private set; }

		public VirtualAxisFromMouse(bool useHorizontal)
		{
			this.useHorizontal = useHorizontal;
		}

		public override void UpdateState()
		{
			// TODO buffer state
			base.UpdateState();
		}
	}

	/**<summary>A virtual joystick-type control.</summary>*/
	public class DynamicControlAxis2 : DynamicControl
	{
		public static string mouseXAsJoystickName = "Mouse X As Joystick";
		public static string mouseYAsJoystickName = "Mouse Y As Joystick";

		public string gamepadName;
		public bool isMouseAsJoy { get; private set; }
		public bool isMouseXAsJoy { get; private set; }
		public float mouseAsJoystickDeadzone = 0.05f;
		private string keyboardMouseNameInternal;

		/**<summary>The name of the axis in Input for the keyboard/mouse
		 * controls.</summary>
		 */
		public string keyboardMouseName
		{
			get
			{
				return keyboardMouseNameInternal;
			}
			set
			{
				keyboardMouseNameInternal = value;
				if (value == mouseXAsJoystickName)
				{
					isMouseAsJoy = true;
					isMouseXAsJoy = true;
				}
				else if (value == mouseYAsJoystickName)
				{
					isMouseAsJoy = true;
					isMouseXAsJoy = false;
				}
			}
		}
		public virtual float RawAxis
		{
			get
			{
				float axisValue = Input.GetAxisRaw(gamepadName);
				if (!gamepadModeEnabled && Mathf.Approximately(0.0f, axisValue))
				{
					if (isMouseAsJoy)
					{
						float value = 0.0f;
						if (isMouseXAsJoy)
						{
							value = Input.mousePosition.x / Screen.width * 2.0f - 1.0f;
						}
						else
						{
							value = Input.mousePosition.y / Screen.height * 2.0f - 1.0f;
						}
						if (value <= mouseAsJoystickDeadzone)
						{
							return 0.0f;
						}
						return value;
					}
					return Input.GetAxisRaw(keyboardMouseName);
				}
				return axisValue;
			}
		}
		public virtual float Axis
		{
			get
			{
				float axisValue = Input.GetAxis(gamepadName);
				if (!gamepadModeEnabled && Mathf.Approximately(0.0f, axisValue))
				{
					if (isMouseAsJoy)
					{
						float value = 0.0f;
						if (isMouseXAsJoy)
						{
							value = Input.mousePosition.x / Screen.width * 2.0f - 1.0f;
						}
						else
						{
							value = Input.mousePosition.y / Screen.height * 2.0f - 1.0f;
						}
						if (Mathf.Abs(value) <= mouseAsJoystickDeadzone)
						{
							return 0.0f;
						}
						return value;
					}
					return Input.GetAxis(keyboardMouseName);
				}
				return axisValue;
			}
		}

		public DynamicControlAxis2(string description, string gamepadName, string keyboardMouseName) : base(description)
		{
			this.gamepadName = gamepadName;
			this.keyboardMouseName = keyboardMouseName;
		}

		public override void UpdateControlStates()
		{
		}
	}

	public class VirtualAxisWithButton : DynamicControlAxis2
	{
		public KeyCode gamepadButton;
		public KeyCode keyboardMouseButton;

		public override float RawAxis
		{
			get
			{
				float value = base.RawAxis;
				if (gamepadButton == KeyCode.None && !Mathf.Approximately(0.0f, Input.GetAxisRaw(gamepadName)))
				{
					return value;
				}
				if (gamepadButton != KeyCode.None && Input.GetKey(gamepadButton))
				{
					return value;
				}
				if (keyboardMouseButton == KeyCode.None)
				{
					return value;
				}
				if (keyboardMouseButton != KeyCode.None && Input.GetKey(keyboardMouseButton))
				{
					return value;
				}
				return 0.0f;
			}
		}

		public override float Axis
		{
			get
			{
				float value = base.Axis;
				if (gamepadButton == KeyCode.None && !Mathf.Approximately(0.0f, Input.GetAxis(gamepadName)))
				{
					return value;
				}
				if (gamepadButton != KeyCode.None && Input.GetKey(gamepadButton))
				{
					return value;
				}
				if (keyboardMouseButton == KeyCode.None)
				{
					return value;
				}
				if (keyboardMouseButton != KeyCode.None && Input.GetKey(keyboardMouseButton))
				{
					return value;
				}
				return 0.0f;
			}
		}

		public VirtualAxisWithButton(string description, string gamepadName, string keyboardMouseName, KeyCode gamepadButton, KeyCode keyboardMouseButton) : base (description, gamepadName, keyboardMouseName)
		{
			this.gamepadButton = gamepadButton;
			this.keyboardMouseButton = keyboardMouseButton;
		}
	}
}
