using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Interface defining a max value and current value property
 * which can be considered the main value applying to that class.</summary>
 */
public interface IPrimaryValue
{
	float MaxValue
	{
		get;
	}
	float CurrentValue
	{
		get;
	}
}
