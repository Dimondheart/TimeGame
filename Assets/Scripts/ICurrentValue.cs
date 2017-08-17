using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Interface declaring a class has a current float value representing
 * the most significant aspect of it.</summary>
 */
public interface ICurrentValue
{
	float CurrentValue
	{
		get;
	}
}
