using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**<summary>Chrono-Points for use by chrono-abilities.</summary>*/
public class ChronoPoints : MonoBehaviour, IMaxValue, ICurrentValue
{
	/**<summary>Max CP.</summary>*/
	public float maxChronoPoints;
	/**<summary>Current CP.</summary>*/
	public float chronoPoints { get; private set; }
	/**<summary>CP drain per second when the player is freezing
	 * time.</summary>
	 */
	public float timeFreezeDrainRate = 5.0f;
	/**<summary></summary>*/
	public float rechargeRate = 10.0f;
	/**<summary>If the current character is freezing time, and time is not frozen
	 * for some other reason/by another character.</summary>
	 */
	public bool isCharacterFreezingTime = false;
	/**<summary>When time freeze is active and CP fully drains, CP must
	 * recharge until it is at least timeFreezeDrainRate.</summary>
	 */
	private bool recharging = false;

	/**<summary>If the chrono-freeze ability can be activated right
	 * now based on CP only.</summary>
	 */
	public bool CanActivateChronoFreeze
	{
		get
		{
			if (recharging)
			{
				return chronoPoints >= rechargeRate;
			}
			return !Mathf.Approximately(0.0f, chronoPoints);
		}
	}

	float IMaxValue.MaxValue
	{
		get
		{
			return maxChronoPoints;
		}
	}

	float ICurrentValue.CurrentValue
	{
		get
		{
			return chronoPoints;
		}
	}

	private void Awake()
	{
		chronoPoints = maxChronoPoints;
	}

	private void Update()
	{
		if (ManipulableTime.IsGameFrozen)
		{
			return;
		}
		if (isCharacterFreezingTime)
		{
			chronoPoints = Mathf.Clamp(chronoPoints - Time.deltaTime * timeFreezeDrainRate, 0.0f, maxChronoPoints);
			if (Mathf.Approximately(0.0f, chronoPoints))
			{
				recharging = true;
				isCharacterFreezingTime = false;
				ManipulableTime.IsTimeFrozen = false;
			}
		}
		else
		{
			chronoPoints = Mathf.Clamp(chronoPoints + ManipulableTime.deltaTime * rechargeRate, 0.0f, maxChronoPoints);
		}
		if (chronoPoints >= rechargeRate)
		{
			recharging = false;
		}
	}
}
