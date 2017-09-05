using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>An interface for something that can take a hit or change how
 * it affects the health.</summary>
 */
public interface IHitTaker
{
	/**<summary>Process and/or modify the hit, and return true if this 
	 * has completely negated the hit.</summary>
	 */
	bool TakeHit(HitInfo hit);
	int Priority
	{
		get;
	}
}
