using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	public static class DynamicInputConfiguration
	{
		public static void ConfigureInput()
		{
			ControllerDPad.dPadReadMode = ControllerDPad.DPadReadMode.Axis;
			ControllerDPad.horizontalAxisName = "6th axis (Joysticks)";
			ControllerDPad.verticalAxisName = "7th axis (Joysticks)";

			DynamicInput.SetupAxisControl(
				"Move Horizontal",
				new DynamicControlAxis(
					"Horizontal movement",
					new VirtualAxisBasic("X axis"),
					new VirtualAxisFromButtons(
						new VirtualButtonBasic(KeyCode.A),
						new VirtualButtonBasic(KeyCode.D)
						)
					)
				);
			DynamicInput.SetupAxisControl(
				"Move Vertical",
				new DynamicControlAxis(
					"Vertical movement",
					new VirtualAxisBasic("Y axis"),
					new VirtualAxisFromButtons(
						new VirtualButtonBasic(KeyCode.S),
						new VirtualButtonBasic(KeyCode.W)
						)
					)
				);
			DynamicInput.SetupAxisControl(
				"Look Horizontal",
				new DynamicControlAxis(
					"Horizontal look direction",
					new VirtualAxisBasic("4th axis (Joysticks)"),
					new VirtualAxisFromMouse(true)
					)
				);
			DynamicInput.SetupAxisControl(
				"Look Vertical",
				new DynamicControlAxis(
					"Vertical look direction",
					new VirtualAxisBasic("5th axis (Joysticks)"),
					new VirtualAxisFromMouse(false)
					)
				);
			DynamicInput.SetupButtonControl(
				"Toggle Pause Menu",
				new DynamicControlButton(
					"Open/Close the pause menu",
					new VirtualButtonBasic(KeyCode.Joystick1Button6),
					new VirtualButtonBasic(KeyCode.Escape),
					null
					)
				);
			DynamicInput.SetupButtonControl(
				"Melee",
				new DynamicControlButton(
					"Melee",
					new VirtualButtonBasic(KeyCode.Joystick1Button4),
					new VirtualButtonBasic(KeyCode.Mouse0),
					null
					)
				);
			DynamicInput.SetupButtonControl(
				"Guard",
				new DynamicControlButton(
					"Guard",
					new VirtualButtonBasic(KeyCode.Joystick1Button5),
					new VirtualButtonBasic(KeyCode.Mouse1),
					null
					)
				);
			DynamicInput.SetupButtonControl(
				"Dash",
				new DynamicControlButton(
					"Dash in a direction",
					new VirtualButtonFromAxis("10th axis (Joysticks)"),
					new VirtualButtonBasic(KeyCode.LeftShift),
					null
					)
				);
			DynamicInput.SetupButtonControl(
				"Toggle Time Freeze",
				new DynamicControlButton(
					"Toggle time freezing",
					new VirtualButtonBasic(KeyCode.Joystick1Button3),
					new VirtualButtonBasic(KeyCode.Q),
					null
					)
				);
			DynamicInput.SetupAxisControl(
				"Rewind/Replay",
				new DynamicControlAxis(
					"Horizontal look direction",
					new VirtualAxisBasic("6th axis (Joysticks)"),
					new VirtualAxisFromButtons(
						new VirtualButtonBasic(KeyCode.LeftArrow),
						new VirtualButtonBasic(KeyCode.RightArrow)
						)
					)
				);
		}
	}
}
