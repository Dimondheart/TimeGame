using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TechnoWolf.DynamicInputSystem
{
	/**<summary></summary>*/
	[CustomEditor(typeof(DynamicInput))]
	[CanEditMultipleObjects]
	public class DynamicInputEditor : Editor
	{
		private void OnEnable()
		{
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			//serializedObject.ApplyModifiedProperties();
		}
	}
}
