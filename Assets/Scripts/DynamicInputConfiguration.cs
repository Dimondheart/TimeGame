using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary></summary>*/
public static class DynamicInputConfiguration
{
	public static void ConfigureInput()
	{
		DynamicInput.ControllerDPad.dPadReadMode = DynamicInput.ControllerDPad.DPadReadMode.Axis;
		DynamicInput.ControllerDPad.horizontalAxisName = "6th axis (Joysticks)";
		DynamicInput.ControllerDPad.verticalAxisName = "7th axis (Joysticks)";

		DynamicInput.SetupAxisControl(
			"Move Horizontal",
			new DynamicInput.DynamicControlAxis(
				"Horizontal movement",
				new DynamicInput.VirtualAxisBasic("X axis"),
				new DynamicInput.VirtualAxisFromButtons(
					new DynamicInput.VirtualButtonBasic(KeyCode.A),
					new DynamicInput.VirtualButtonBasic(KeyCode.D)
					)
				)
			);
		DynamicInput.SetupAxisControl(
			"Move Vertical",
			new DynamicInput.DynamicControlAxis(
				"Vertical movement",
				new DynamicInput.VirtualAxisBasic("Y axis"),
				new DynamicInput.VirtualAxisFromButtons(
					new DynamicInput.VirtualButtonBasic(KeyCode.S),
					new DynamicInput.VirtualButtonBasic(KeyCode.W)
					)
				)
			);
		DynamicInput.SetupAxisControl(
			"Look Horizontal",
			new DynamicInput.DynamicControlAxis(
				"Horizontal look direction",
				new DynamicInput.VirtualAxisBasic("4th axis (Joysticks)"),
				new DynamicInput.VirtualAxisFromMouse(true)
				)
			);
		DynamicInput.SetupAxisControl(
			"Look Vertical",
			new DynamicInput.DynamicControlAxis(
				"Vertical look direction",
				new DynamicInput.VirtualAxisBasic("5th axis (Joysticks)"),
				new DynamicInput.VirtualAxisFromMouse(false)
				)
			);
		DynamicInput.SetupButtonControl(
			"Toggle Pause Menu",
			new DynamicInput.DynamicControlButton(
				"Open/Close the pause menu",
				new DynamicInput.VirtualButtonBasic(KeyCode.Joystick1Button6),
				new DynamicInput.VirtualButtonBasic(KeyCode.Escape),
				null
				)
			);
		DynamicInput.SetupButtonControl(
			"Melee",
			new DynamicInput.DynamicControlButton(
				"Melee",
				new DynamicInput.VirtualButtonBasic(KeyCode.Joystick1Button4),
				new DynamicInput.VirtualButtonBasic(KeyCode.Mouse0),
				null
				)
			);
		DynamicInput.SetupButtonControl(
			"Guard",
			new DynamicInput.DynamicControlButton(
				"Guard",
				new DynamicInput.VirtualButtonBasic(KeyCode.Joystick1Button5),
				new DynamicInput.VirtualButtonBasic(KeyCode.Mouse1),
				null
				)
			);
		DynamicInput.SetupButtonControl(
			"Dash",
			new DynamicInput.DynamicControlButton(
				"Dash in a direction",
				new DynamicInput.VirtualButtonFromAxis("10th axis (Joysticks)"),
				new DynamicInput.VirtualButtonBasic(KeyCode.LeftShift),
				null
				)
			);
		DynamicInput.SetupButtonControl(
			"Toggle Time Freeze",
			new DynamicInput.DynamicControlButton(
				"Toggle time freezing",
				new DynamicInput.VirtualButtonBasic(KeyCode.Joystick1Button3),
				new DynamicInput.VirtualButtonBasic(KeyCode.Q),
				null
				)
			);
		DynamicInput.SetupAxisControl(
			"Rewind/Replay",
			new DynamicInput.DynamicControlAxis(
				"Horizontal look direction",
				new DynamicInput.VirtualAxisBasic("6th axis (Joysticks)"),
				new DynamicInput.VirtualAxisFromButtons(
					new DynamicInput.VirtualButtonBasic(KeyCode.LeftArrow),
					new DynamicInput.VirtualButtonBasic(KeyCode.RightArrow)
					)
				)
			);
	}
}
