using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
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
}
