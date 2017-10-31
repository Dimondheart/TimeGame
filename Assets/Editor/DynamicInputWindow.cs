using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TechnoWolf.SystemConfiguration;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	public class DynamicInputWindow : EditorWindow
	{
		private int maxPlayers = 1;
		private bool showButtons = false;
		private bool showAxes = false;
		private Dictionary<string, bool> showControlInfo = new Dictionary<string, bool>();
		private Dictionary<string, int> typeSelection1 = new Dictionary<string, int>();
		private Dictionary<string, int> typeSelection2 = new Dictionary<string, int>();
		private Dictionary<string, int> typeSelection3 = new Dictionary<string, int>();
		private Dictionary<string, Dictionary<string, object>> typeConfig1 =
			new Dictionary<string, Dictionary<string, object>>();
		private Dictionary<string, Dictionary<string, object>> typeConfig2 =
			new Dictionary<string, Dictionary<string, object>>();
		private Dictionary<string, Dictionary<string, object>> typeConfig3 =
			new Dictionary<string, Dictionary<string, object>>();
		private System.Type[] virtualButtonOptionsForKeyMouse =
		{
			typeof(VirtualButtonPlaceholder),
			typeof(VirtualButtonBasic),
			typeof(VirtualButtonFromAxis),
			typeof(VirtualButtonFromAxisDirection)
		};
		private System.Type[] virtualButtonOptionsForGamepad =
		{
			typeof(VirtualButtonPlaceholder),
			typeof(VirtualButtonBasic),
			typeof(VirtualButtonFromAxis),
			typeof(VirtualButtonFromAxisDirection),
			typeof(VirtualButtonFromDPad)
		};
		private System.Type[] virtualAxisOptionsForKeyMouse =
		{
			typeof(VirtualAxisPlaceholder),
			typeof(VirtualAxisBasic),
			typeof(VirtualAxisFromButtons),
			typeof(VirtualAxisFromMouse),
			typeof(VirtualAxisFromMouseMovement)
		};
		private System.Type[] virtualAxisOptionsForGamepad =
		{
			typeof(VirtualAxisPlaceholder),
			typeof(VirtualAxisBasic),
			typeof(VirtualAxisFromButtons),
			typeof(VirtualAxisFromDPad)
		};

		[MenuItem("Window/Dynamic Input")]
		public static void ShowWindow()
		{
			DynamicInputWindow win = EditorWindow.GetWindowWithRect<DynamicInputWindow>(
				new Rect(0, 0, 400, 200),
				false,
				"Dynamic Input"
				);
			win.ResetFoldouts();
		}

		private void OnGUI()
		{
			string[] buttonTypeOptionsKM = new string[virtualButtonOptionsForKeyMouse.Length];
			for (int i = 0; i < virtualButtonOptionsForKeyMouse.Length; i++)
			{
				buttonTypeOptionsKM[i] =
					virtualButtonOptionsForKeyMouse[i].Name.Substring(
						virtualButtonOptionsForKeyMouse[i].Name.IndexOf('n') + 1
						);
			}
			string[] buttonTypeOptionsG = new string[virtualButtonOptionsForGamepad.Length];
			for (int i = 0; i < virtualButtonOptionsForGamepad.Length; i++)
			{
				buttonTypeOptionsG[i] =
					virtualButtonOptionsForGamepad[i].Name.Substring(
						virtualButtonOptionsForGamepad[i].Name.IndexOf('n') + 1
						);
			}
			string[] axisTypeOptionsKM = new string[virtualAxisOptionsForKeyMouse.Length];
			for (int i = 0; i < virtualAxisOptionsForKeyMouse.Length; i++)
			{
				axisTypeOptionsKM[i] =
					virtualAxisOptionsForKeyMouse[i].Name.Substring(
						virtualAxisOptionsForKeyMouse[i].Name.IndexOf('s') + 1
						);
			}
			string[] axisTypeOptionsG = new string[virtualAxisOptionsForGamepad.Length];
			for (int i = 0; i < virtualAxisOptionsForGamepad.Length; i++)
			{
				axisTypeOptionsG[i] =
					virtualAxisOptionsForGamepad[i].Name.Substring(
						virtualAxisOptionsForGamepad[i].Name.IndexOf('s') + 1
						);
			}
			EditorGUILayout.Space();
			if (GUILayout.Button("Reset All Controls", GUILayout.MaxWidth(150.0f)))
			{
				ResetAll();
			}
			EditorGUILayout.Space();
			GUI.enabled = false;
			maxPlayers = EditorGUILayout.IntSlider("Max Players", maxPlayers, 1, 4);
			GUI.enabled = true;
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			showButtons = EditorGUILayout.Foldout(showButtons, "Buttons");
			if (showButtons)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				foreach (string buttonName in System.Enum.GetNames(typeof(DynamicInputButton)))
				{
					if (!showControlInfo.ContainsKey(buttonName))
					{
						DynamicInput.SetupButtonControl(buttonName);
						showControlInfo[buttonName] = false;
						typeSelection1[buttonName] = 0;
						typeSelection2[buttonName] = 0;
						typeSelection3[buttonName] = 0;
						typeConfig1[buttonName] = new Dictionary<string, object>();
						typeConfig2[buttonName] = new Dictionary<string, object>();
						typeConfig3[buttonName] = new Dictionary<string, object>();
					}
					showControlInfo[buttonName] =
						EditorGUILayout.Foldout(showControlInfo[buttonName], buttonName);
					if (showControlInfo[buttonName])
					{
						DynamicInput.GetButtonControl(buttonName).description =
							EditorGUILayout.DelayedTextField(
								"Description",
								DynamicInput.GetButtonControl(buttonName).description
								);
						EditorGUILayout.Space();
						typeSelection1[buttonName] =
							EditorGUILayout.Popup("Gamepad Button", typeSelection1[buttonName], buttonTypeOptionsG);
						HandleVirtualButtonOnGUI(
							virtualButtonOptionsForGamepad[typeSelection1[buttonName]],
							typeConfig1[buttonName],
							true
							);
						EditorGUILayout.Space();
						typeSelection2[buttonName] =
							EditorGUILayout.Popup("Key/Mouse Button", typeSelection2[buttonName], buttonTypeOptionsKM);
						HandleVirtualButtonOnGUI(
							virtualButtonOptionsForKeyMouse[typeSelection2[buttonName]],
							typeConfig2[buttonName],
							false
							);
						EditorGUILayout.Space();
						typeSelection3[buttonName] =
							EditorGUILayout.Popup("Key/Mouse Alt Button", typeSelection3[buttonName], buttonTypeOptionsKM);
						HandleVirtualButtonOnGUI(
							virtualButtonOptionsForKeyMouse[typeSelection3[buttonName]],
							typeConfig3[buttonName],
							false
							);
						EditorGUILayout.Space();
						EditorGUILayout.Space();
					}
				}
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
			}
			showAxes = EditorGUILayout.Foldout(showAxes, "Axes");
			if (showAxes)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				foreach (string axisName in System.Enum.GetNames(typeof(DynamicInputAxis)))
				{
					if (!showControlInfo.ContainsKey(axisName))
					{
						DynamicInput.SetupAxisControl(axisName);
						showControlInfo[axisName] = false;
						typeSelection1[axisName] = 0;
						typeSelection2[axisName] = 0;
						typeSelection3[axisName] = 0;
						typeConfig1[axisName] = new Dictionary<string, object>();
						typeConfig2[axisName] = new Dictionary<string, object>();
						typeConfig3[axisName] = new Dictionary<string, object>();
					}
					showControlInfo[axisName] =
						EditorGUILayout.Foldout(showControlInfo[axisName], axisName);
					if (showControlInfo[axisName])
					{
						DynamicInput.GetAxisControl(axisName).description =
							EditorGUILayout.DelayedTextField(
								"Description",
								DynamicInput.GetAxisControl(axisName).description
								);
						EditorGUILayout.Space();
						typeSelection1[axisName] =
							EditorGUILayout.Popup("Gamepad Axis", typeSelection1[axisName], axisTypeOptionsG);
						HandleVirtualAxisOnGUI(
							virtualAxisOptionsForGamepad[typeSelection1[axisName]],
							typeConfig1[axisName],
							true
							);
						EditorGUILayout.Space();
						typeSelection2[axisName] =
							EditorGUILayout.Popup("Key/Mouse Axis", typeSelection2[axisName], axisTypeOptionsKM);
						HandleVirtualAxisOnGUI(
							virtualAxisOptionsForKeyMouse[typeSelection2[axisName]],
							typeConfig2[axisName],
							false
							);
						EditorGUILayout.Space();
						EditorGUILayout.Space();
					}
				}
			}
		}

		private void ResetAll()
		{
			DynamicInput.RemoveAllControls();
			ResetFoldouts();
		}

		private void ResetFoldouts()
		{
			showButtons = false;
			showAxes = false;
			List<string> existingButtonNames = DynamicInput.GetButtonNames();
			foreach (string buttonName in System.Enum.GetNames(typeof(DynamicInputButton)))
			{
				if (!existingButtonNames.Contains(buttonName))
				{
					DynamicInput.SetupButtonControl(buttonName);
				}
				showControlInfo[buttonName] = false;
				typeSelection1[buttonName] = 0;
				typeSelection2[buttonName] = 0;
				typeSelection3[buttonName] = 0;
				typeConfig1[buttonName] = new Dictionary<string, object>();
				typeConfig2[buttonName] = new Dictionary<string, object>();
				typeConfig3[buttonName] = new Dictionary<string, object>();
			}
			List<string> existingAxisNames = DynamicInput.GetAxisNames();
			foreach (string axisName in System.Enum.GetNames(typeof(DynamicInputButton)))
			{
				if (!existingAxisNames.Contains(axisName))
				{
					DynamicInput.SetupAxisControl(axisName);
				}
				showControlInfo[axisName] = false;
				typeSelection1[axisName] = 0;
				typeSelection2[axisName] = 0;
				typeSelection3[axisName] = 0;
				typeConfig1[axisName] = new Dictionary<string, object>();
				typeConfig2[axisName] = new Dictionary<string, object>();
				typeConfig3[axisName] = new Dictionary<string, object>();
			}
		}

		private void HandleVirtualButtonOnGUI(System.Type type, Dictionary<string, object> config, bool isGamepad)
		{
			if (type == typeof(VirtualButtonPlaceholder))
			{
			}
			else if (type == typeof(VirtualButtonBasic))
			{
				if (isGamepad)
				{
					if (!config.ContainsKey("Button"))
					{
						config["Button"] = GamepadButton.A;
					}
					config["Button"] = EditorGUILayout.EnumPopup("Button", (GamepadButton)config["Button"]);
				}
				else
				{
					if (!config.ContainsKey("KeyCodeString"))
					{
						config["KeyCodeString"] = "None";
					}
					config["KeyCodeString"] = EditorGUILayout.TextField("KeyCodeString", (string)config["KeyCodeString"]);
				}
			}
			else if (type == typeof(VirtualButtonFromDPad))
			{
				if (!config.ContainsKey("Direction"))
				{
					config["Direction"] = DPadButton.Up;
				}
				config["Direction"] = EditorGUILayout.EnumPopup("Direction", (DPadButton)config["Direction"]);
			}
			else if (type == typeof(VirtualButtonFromAxis))
			{
				if (isGamepad)
				{
					if (!config.ContainsKey("Axis"))
					{
						config["Axis"] = GamepadAxis.LeftStickX;
					}
					config["Axis"] = EditorGUILayout.EnumPopup("Axis", (GamepadAxis)config["Axis"]);
				}
				else
				{
					if (!config.ContainsKey("AxisName"))
					{
						config["AxisName"] = "Horizontal";
					}
					config["AxisName"] = EditorGUILayout.TextField("AxisName", (string)config["AxisName"]);
				}
			}
			else if (type == typeof(VirtualButtonFromAxisDirection))
			{
				if (isGamepad)
				{
					if (!config.ContainsKey("Axis"))
					{
						config["Axis"] = GamepadAxis.LeftStickX;
					}
					config["Axis"] = EditorGUILayout.EnumPopup("Axis", (GamepadAxis)config["Axis"]);
				}
				else
				{
					if (!config.ContainsKey("AxisName"))
					{
						config["AxisName"] = "Horizontal";
					}
					config["AxisName"] = EditorGUILayout.TextField("AxisName", (string)config["AxisName"]);
				}
				if (!config.ContainsKey("Positive"))
				{
					config["Positive"] = false;
				}
				config["Positive"] = EditorGUILayout.Toggle("Positive", (bool)config["Positive"]);
			}
			else
			{
				EditorGUILayout.LabelField("No Options");
			}
		}

		private void HandleVirtualAxisOnGUI(System.Type type, Dictionary<string, object> config, bool isGamepad)
		{
			// TODO set config values
			if (type == typeof(VirtualAxisPlaceholder))
			{
			}
			else if (type == typeof(VirtualAxisBasic))
			{
				if (isGamepad)
				{
					EditorGUILayout.EnumPopup("Axis", GamepadAxis.LeftStickX);
				}
				else
				{
					EditorGUILayout.TextField("Axis Name", "Horizontal");
				}
			}
			else if (type == typeof(VirtualAxisFromButtons))
			{
				if (isGamepad)
				{
					EditorGUILayout.EnumPopup("Negative", GamepadButton.A);
					EditorGUILayout.EnumPopup("Positive", GamepadButton.B);
				}
				else
				{
					EditorGUILayout.TextField("Negative KeyCode String", "A");
					EditorGUILayout.TextField("Positive KeyCode String", "D");
				}
			}
			else if (type == typeof(VirtualAxisFromMouse))
			{
				EditorGUILayout.Toggle("Use Mouse Y", false);
			}
			else if (type == typeof(VirtualAxisFromMouseMovement))
			{
				EditorGUILayout.Toggle("Use Mouse Y", false);
			}
			else if (type == typeof(VirtualAxisFromDPad))
			{
				EditorGUILayout.Toggle("Use Vertical", false);
			}
			else
			{
				EditorGUILayout.LabelField("No Options");
			}
		}
	}
}
