using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary></summary>*/
public static class DynamicInputConfiguration
{
	public static void ConfigureInput(Dictionary<string, DynamicInput.DynamicControlButton> buttonControls, Dictionary<string, DynamicInput.DynamicControl> virtualControls)
	{
		DynamicInput.ControllerDPad.dPadReadMode = DynamicInput.ControllerDPad.DPadReadMode.Axis;
		DynamicInput.ControllerDPad.horizontalAxis = "6th axis (Joysticks)";
		DynamicInput.ControllerDPad.verticalAxis = "7th axis (Joysticks)";

		virtualControls.Add(
			"Move Horizontal",
			new DynamicInput.DynamicControlAxis("Horizontal movement", "X axis", "AD axis")
			);
		virtualControls.Add(
			"Move Vertical",
			new DynamicInput.DynamicControlAxis("Vertical movement", "Y axis", "SW axis")
			);
		virtualControls.Add(
			"Look Horizontal",
			new DynamicInput.DynamicControlAxis("Horizontal look direction", "4th axis (Joysticks)", DynamicInput.DynamicControlAxis.mouseXAsJoystickName)
			);
		virtualControls.Add(
			"Look Vertical",
			new DynamicInput.DynamicControlAxis("Vertical look direction", "5th axis (Joysticks)", DynamicInput.DynamicControlAxis.mouseYAsJoystickName)
			);
		buttonControls.Add(
			"Toggle Pause Menu",
			new DynamicInput.DynamicControlButton(
				"Open/Close the pause menu",
				new DynamicInput.VirtualSimpleButton(KeyCode.Joystick1Button6),
				new DynamicInput.VirtualSimpleButton(KeyCode.Escape),
				null
				)
			);
		buttonControls.Add(
			"Melee",
			new DynamicInput.DynamicControlButton(
				"Melee",
				new DynamicInput.VirtualSimpleButton(KeyCode.Joystick1Button4),
				new DynamicInput.VirtualSimpleButton(KeyCode.Mouse0),
				null
				)
			);
		buttonControls.Add(
			"Guard",
			new DynamicInput.DynamicControlButton(
				"Guard",
				new DynamicInput.VirtualSimpleButton(KeyCode.Joystick1Button5),
				new DynamicInput.VirtualSimpleButton(KeyCode.Mouse1),
				null
				)
			);
		buttonControls.Add(
			"Dash",
			new DynamicInput.DynamicControlButton(
				"Dash in a direction",
				new DynamicInput.VirtualButtonFromAxis("10th axis (Joysticks)"),
				new DynamicInput.VirtualSimpleButton(KeyCode.LeftShift),
				null
				)
			);
		buttonControls.Add(
			"Toggle Time Freeze",
			new DynamicInput.DynamicControlButton(
				"Toggle time freezing",
				new DynamicInput.VirtualSimpleButton(KeyCode.Joystick1Button3),
				new DynamicInput.VirtualSimpleButton(KeyCode.Q),
				null
				)
			);
		virtualControls.Add(
			"Rewind/Replay",
			new DynamicInput.DynamicControlAxis("Horizontal look direction", "6th axis (Joysticks)", "<> axis")
			);
		virtualControls.Add(
			"Temperature Element Adjust",
			new DynamicInput.VirtualAxisWithButton("Change the temperature elemental focus", "6th axis (Joysticks)", "AD axis", KeyCode.None, KeyCode.LeftAlt)
			);
		virtualControls.Add(
			"Moisture Element Adjust",
			new DynamicInput.VirtualAxisWithButton("Change the moisture elemental focus", "7th axis (Joysticks)", "SW axis", KeyCode.None, KeyCode.LeftAlt)
			);
	}
}
