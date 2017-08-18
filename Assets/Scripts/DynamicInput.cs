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
	/**<summary>The current virtual controls.</summary>*/
	private static Dictionary<string, VirtualControl> virtualControls =
		new Dictionary<string, VirtualControl>();

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
		if (virtualControls.Count > 0)
		{
			return;
		}
		virtualControls.Add(
			"Move Horizontal",
			new VirtualAxis("Horizontal movement", "X axis", "AD axis")
			);
		virtualControls.Add(
			"Move Vertical",
			new VirtualAxis("Vertical movement", "Y axis", "SW axis")
			);
		virtualControls.Add(
			"Toggle Pause Menu",
			new VirtualButton("Open/Close the pause menu", KeyCode.Joystick1Button6, KeyCode.Escape)
			);
		virtualControls.Add(
			"Melee",
			new VirtualButton("Melee", KeyCode.Joystick1Button4, KeyCode.Mouse0)
			);
		virtualControls.Add(
			"Guard",
			new VirtualButton("Guard", KeyCode.Joystick1Button5, KeyCode.Mouse1)
			);
		virtualControls.Add(
			"Dash",
			new VirtualButtonFromJoystick("Dash in a direction", 0.1f, "10th axis (Joysticks)", KeyCode.LeftShift)
			);
	}

	/**<summary>Input.GetButtonDown equivalent.</summary>*/
	public static bool GetButtonDown(string virtualName)
	{
		VirtualControl control = virtualControls[virtualName];
		if (control.GetType() == typeof(VirtualButtonFromJoystick))
		{
			return Input.GetKeyDown(((VirtualButtonFromJoystick)control).keyboardMouseKeyCode);
		}
		return
			Input.GetKeyDown(((VirtualButton)control).gamepadKeyCode)
			|| Input.GetKeyDown(((VirtualButton)control).keyboardMouseKeyCode);
	}

	/**<summary>Input.GetButton equivalent.</summary>*/
	public static bool GetButton(string virtualName)
	{
		VirtualControl control = virtualControls[virtualName];
		if (control.GetType() == typeof(VirtualButtonFromJoystick))
		{
			return Mathf.Abs(Input.GetAxisRaw(((VirtualButtonFromJoystick)control).gamepadName)) >= ((VirtualButtonFromJoystick)control).cutoff
				|| Input.GetKey(((VirtualButtonFromJoystick)control).keyboardMouseKeyCode);
		}
		return
			Input.GetKey(((VirtualButton)control).gamepadKeyCode)
			|| Input.GetKey(((VirtualButton)control).keyboardMouseKeyCode);
	}

	/**<summary>Input.GetButtonUp equivalent.</summary>*/
	public static bool GetButtonUp(string virtualName)
	{
		VirtualControl control = virtualControls[virtualName];
		if (control.GetType() == typeof(VirtualButtonFromJoystick))
		{
			return Input.GetKeyUp(((VirtualButtonFromJoystick)control).keyboardMouseKeyCode);
		}
		return
			Input.GetKeyUp(((VirtualButton)control).gamepadKeyCode)
			|| Input.GetKeyUp(((VirtualButton)control).keyboardMouseKeyCode);
	}

	/**<summary>Input.GetAxisRaw equivalent.</summary>*/
	public static float GetAxisRaw(string virtualName)
	{
		return ((VirtualAxis)virtualControls[virtualName]).RawAxis;
	}

	/**<summary>Input.GetAxis equivalent.</summary>*/
	public static float GetAxis(string virtualName)
	{
		return ((VirtualAxis)virtualControls[virtualName]).Axis;
	}

	/**<summary>Base for the different virtual control types.</summary>*/
	public abstract class VirtualControl
	{
		public readonly string description;

		public VirtualControl(string description)
		{
			this.description = description;
		}
	}

	/**<summary>A virtual button-type control.</summary>*/
	public class VirtualButton : VirtualControl
	{
		public KeyCode gamepadKeyCode;
		public KeyCode keyboardMouseKeyCode;

		public VirtualButton(string description, KeyCode gamepadKeyCode, KeyCode keyboardMouseKeyCode) : base(description)
		{
			this.gamepadKeyCode = gamepadKeyCode;
			this.keyboardMouseKeyCode = keyboardMouseKeyCode;
		}
	}

	/**<summary>A virtual joystick-type control.</summary>*/
	public class VirtualAxis : VirtualControl
	{
		public static string mouseXAsJoystickName = "Mouse X As Joystick";
		public static string mouseYAsJoystickName = "Mouse Y As Joystick";

		public string gamepadName;
		public bool isMouseAsJoy { get; private set; }
		public bool isMouseXAsJoy { get; private set; }
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
		public float RawAxis
		{
			get
			{
				float axisValue = Input.GetAxisRaw(gamepadName);
				if (Mathf.Approximately(0.0f, axisValue))
				{
					if (isMouseAsJoy)
					{
						if (isMouseXAsJoy)
						{
							return Input.GetAxisRaw("Mouse X") / Screen.width * 2.0f - 1.0f;
						}
						return Input.GetAxisRaw("Mouse Y") / Screen.height * 2.0f - 1.0f;
					}
					return Input.GetAxisRaw(keyboardMouseName);
				}
				return axisValue;
			}
		}
		public float Axis
		{
			get
			{
				float axisValue = Input.GetAxis(gamepadName);
				if (Mathf.Approximately(0.0f, axisValue))
				{
					if (isMouseAsJoy)
					{
						if (isMouseXAsJoy)
						{
							return Input.GetAxis("Mouse X") / Screen.width * 2.0f - 1.0f;
						}
						return Input.GetAxis("Mouse Y") / Screen.height * 2.0f - 1.0f;
					}
					return Input.GetAxis(keyboardMouseName);
				}
				return axisValue;
			}
		}

		public VirtualAxis(string description, string gamepadName, string keyboardMouseName) : base(description)
		{
			this.gamepadName = gamepadName;
			this.keyboardMouseName = keyboardMouseName;
		}
	}

	public class VirtualButtonFromJoystick : VirtualControl
	{
		public string gamepadName;
		public KeyCode keyboardMouseKeyCode;
		public float cutoff;

		public VirtualButtonFromJoystick(string description, float cutoff, string gamepadName, KeyCode keyboardMouseKeyCode) : base(description)
		{
			this.gamepadName = gamepadName;
			this.cutoff = cutoff;
			this.keyboardMouseKeyCode = keyboardMouseKeyCode;
		}
	}
}
