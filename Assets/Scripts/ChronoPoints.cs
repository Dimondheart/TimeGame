using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Chrono-Points for use by chrono-abilities.</summary>*/
	public class ChronoPoints : MonoBehaviour, IPrimaryValue
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

		float IPrimaryValue.MaxValue
		{
			get
			{
				return maxChronoPoints;
			}
		}

		float IPrimaryValue.MaxCurrentValue
		{
			get
			{
				return maxChronoPoints;
			}
		}

		float IPrimaryValue.CurrentValue
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
			if (ManipulableTime.IsGamePaused)
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
					ManipulableTime.ChangeTimePaused(false);
				}
			}
			else
			{
				if (chronoPoints >= maxChronoPoints)
				{
					return;
				}
				float regenAmount = ManipulableTime.deltaTime * rechargeRate;
				float newCP = chronoPoints + regenAmount;
				if (newCP > maxChronoPoints)
				{
					regenAmount = newCP - maxChronoPoints;
					newCP = maxChronoPoints;
				}
				double powerAvailable = GetComponent<StoredPower>().CurrentPP;
				if (regenAmount > powerAvailable)
				{
					regenAmount = (float)powerAvailable;
					GetComponent<StoredPower>().UsePP(powerAvailable, false);
					GetComponent<StoredPower>().RemoveMaxPP(powerAvailable * 0.25);
					newCP = chronoPoints + regenAmount;
				}
				else
				{
					GetComponent<StoredPower>().UsePP(regenAmount, false);
					GetComponent<StoredPower>().RemoveMaxPP(regenAmount * 0.25f);
				}
				chronoPoints = newCP;
			}
			if (chronoPoints >= rechargeRate)
			{
				recharging = false;
			}
		}
	}
}
