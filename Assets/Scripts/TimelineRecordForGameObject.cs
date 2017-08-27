using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary></summary>*/
public class TimelineRecordForGameObject : TimelineRecord
{
	public bool activeSelf;

	public override void AddCommonData(Component component)
	{
		activeSelf = component.gameObject.activeSelf;
	}

	public override void ApplyCommonData(Component component)
	{
		component.gameObject.SetActive(activeSelf);
	}
}
