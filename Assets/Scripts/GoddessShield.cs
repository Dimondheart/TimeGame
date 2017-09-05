using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Shields the player from fatal hits, but drains SPP.</summary>*/
public class GoddessShield : MonoBehaviour, IHitTaker
{
	int IHitTaker.Priority
	{
		get
		{
			return 50;
		}
	}

	bool IHitTaker.TakeHit(HitInfo hit)
	{
		if (hit.hitBy == null)
		{
			return false;
		}
		double powerAvailable = GetComponent<StoredPower>().CurrentPP;
		if (GetComponent<Health>().CurrentHP - hit.damage < 1.0f)
		{
			if (hit.damage <= powerAvailable)
			{
				GetComponent<StoredPower>().UsePP(hit.damage, true);
				GetComponent<StoredPower>().RemoveMaxPP(hit.damage * 0.25);
				hit.damage = 0.0f;
				hit.permanentDamage = 0.0f;
				return true;
			}
			else
			{
				hit.damage -= (float)powerAvailable;
				GetComponent<StoredPower>().UsePP(powerAvailable, true);
				GetComponent<StoredPower>().RemoveMaxPP(powerAvailable * 0.25);
				return false;
			}
		}
		return false;
	}
}
