using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Interface defining a max value and current value property
 * which can be considered the main value applying to that class.</summary>
 */
public interface IPrimaryValue
{
	/**<summary>The highest value than MaxCurrentValue can be at. This will almost
	 * never change, only possibly in situations like leveling up a character.</summary>
	 */
	float MaxValue
	{
		get;
	}
	/**<summary>CurrentValue will never be greater than this value at any given
	 * moment.</summary>
	 */
	float MaxCurrentValue
	{
		get;
	}
	float CurrentValue
	{
		get;
	}
}
