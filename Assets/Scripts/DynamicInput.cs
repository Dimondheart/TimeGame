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
		return ((DynamicControlAxis)virtualControls[virtualName]).RawAxis;
	}

	/**<summary>Input.GetAxis equivalent.</summary>*/
	public static float GetAxis(string virtualName)
	{
		return ((DynamicControlAxis)virtualControls[virtualName]).Axis;
	}

	/**<summary>Base for the different virtual control types.</summary>*/
	public abstract class DynamicControl
	{
		public readonly string description;

		public DynamicControl(string description)
		{
			this.description = description;
		}

		public abstract void UpdateControlStates();
	}

	public class DynamicControlButton : DynamicControl
	{
		public VirtualButton gamepadButton { get; private set; }
		public VirtualButton keyMouseButton { get; private set; }
		public VirtualButton keyMouseButtonAlt;

		public DynamicControlButton(string description, VirtualButton gamepadButton, VirtualButton keyMouseButton, VirtualButton keyMouseButtonAlt) : base(description)
		{
			SetGamepadButton(gamepadButton);
			SetKeyMouseButton(keyMouseButton);
			SetKeyMouseButtonAlt(keyMouseButtonAlt);
		}

		public override void UpdateControlStates()
		{
			gamepadButton.UpdateState();
			keyMouseButton.UpdateState();
			keyMouseButtonAlt.UpdateState();
		}

		public void SetGamepadButton(VirtualButton button)
		{
			gamepadButton = VirtualPlaceholderButton.GetPlaceholderIfNull(button);
		}

		public void SetKeyMouseButton(VirtualButton button)
		{
			keyMouseButton = VirtualPlaceholderButton.GetPlaceholderIfNull(button);
		}

		public void SetKeyMouseButtonAlt(VirtualButton button)
		{
			keyMouseButtonAlt = VirtualPlaceholderButton.GetPlaceholderIfNull(button);
		}

		public bool GetButtonDown()
		{
			return gamepadButton.GetButtonDown() || keyMouseButton.GetButtonDown() || keyMouseButtonAlt.GetButtonDown();
		}

		public bool GetButton()
		{
			return gamepadButton.GetButton() || keyMouseButton.GetButton() || keyMouseButtonAlt.GetButton();
		}

		public bool GetButtonUp()
		{
			return gamepadButton.GetButtonUp() || keyMouseButton.GetButtonUp() || keyMouseButtonAlt.GetButtonUp();
		}
	}

	public abstract class VirtualControl
	{
		public virtual void UpdateState()
		{
		}
	}

	public abstract class VirtualButton : VirtualControl
	{
		public abstract bool GetButtonDown();
		public abstract bool GetButton();
		public abstract bool GetButtonUp();
	}

	public class VirtualPlaceholderButton : VirtualButton
	{
		public static readonly VirtualPlaceholderButton placeholder = new VirtualPlaceholderButton();

		public static VirtualButton GetPlaceholderIfNull(VirtualButton button)
		{
			return (button == null) ? placeholder : button;
		}

		private VirtualPlaceholderButton()
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

	public class VirtualSimpleButton : VirtualButton
	{
		public KeyCode keyCode;

		public VirtualSimpleButton(KeyCode keyCode)
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

	public abstract class VirtualButtonWithState : VirtualButton
	{
		private ButtonState currentState;

		public override void UpdateState()
		{
			base.UpdateState();
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

		private enum ButtonState : byte
		{
			NotPressed = 0,
			ButtonDown,
			ButtonHeld,
			ButtonUp
		}
	}

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

	public static class ControllerDPad
	{
		public static DPadReadMode dPadReadMode;
		public static string horizontalAxis;
		public static string verticalAxis;
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
				value = Input.GetAxis(horizontalAxis);
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
				value = Input.GetAxisRaw(verticalAxis);
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

	/**<summary>A virtual joystick-type control.</summary>*/
	public class DynamicControlAxis : DynamicControl
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

		public DynamicControlAxis(string description, string gamepadName, string keyboardMouseName) : base(description)
		{
			this.gamepadName = gamepadName;
			this.keyboardMouseName = keyboardMouseName;
		}

		public override void UpdateControlStates()
		{
		}
	}

	public class DynamicControlButtonFromJoystick : DynamicControl
	{
		public string gamepadName;
		public KeyCode keyboardMouseKeyCode;
		public float cutoff;

		public DynamicControlButtonFromJoystick(string description, float cutoff, string gamepadName, KeyCode keyboardMouseKeyCode) : base(description)
		{
			this.gamepadName = gamepadName;
			this.cutoff = cutoff;
			this.keyboardMouseKeyCode = keyboardMouseKeyCode;
		}

		public override void UpdateControlStates()
		{
		}
	}

	public class VirtualAxisWithButton : DynamicControlAxis
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
