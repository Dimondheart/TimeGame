using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Finite stored power available for use.</summary>*/
	public class StoredPower : RecordableMonoBehaviour<TimelineRecord_StoredPower>, IPrimaryValue
	{
		public double initialMaxPP = 1000.0;
		public double regenRate = 0.0;

		private double absoluteMaxPP;
		private double currentMaxPP = double.PositiveInfinity;
		private double currentPP = double.PositiveInfinity;
		/**<summary>PP that must be restored by regen, and cannot be restored
		 * by rewinding time.</summary>
		 */
		private double permanentlyUsedPP;

		public double AbsoluteMaxPP
		{
			get
			{
				return absoluteMaxPP;
			}
			private set
			{
				absoluteMaxPP = value;
				// Easy auto adjust of the 2 dependent values
				CurrentMaxPP = CurrentMaxPP;
			}
		}

		public double CurrentMaxPP
		{
			get
			{
				return currentMaxPP;
			}
			private set
			{
				if (value < 1.0)
				{
					currentMaxPP = 0.0;
				}
				else if (value > absoluteMaxPP)
				{
					currentMaxPP = absoluteMaxPP;
				}
				else
				{
					currentMaxPP = value;
				}
				// Easy adjust
				CurrentPP = currentPP;
			}
		}

		public double CurrentPP
		{
			get
			{
				return currentPP - permanentlyUsedPP;
			}
			private set
			{
				if (value < 1.0)
				{
					currentPP = 0.0;
				}
				else if (value > currentMaxPP)
				{
					currentPP = currentMaxPP;
				}
				else
				{
					currentPP = value;
				}
			}
		}

		float IPrimaryValue.MaxValue
		{
			get
			{
				return 100.0f;
			}
		}

		float IPrimaryValue.MaxCurrentValue
		{
			get
			{
				return (float)(CurrentMaxPP / AbsoluteMaxPP * 100.0);
			}
		}

		float IPrimaryValue.CurrentValue
		{
			get
			{
				return (float)(CurrentPP / AbsoluteMaxPP * 100.0);
			}
		}

		private void Awake()
		{
			absoluteMaxPP = initialMaxPP;
			CurrentMaxPP = absoluteMaxPP;
			CurrentPP = absoluteMaxPP;
		}

		protected override void FlowingUpdate()
		{
			if (regenRate == 0.0)
			{
				return;
			}
			double amountToRegen = regenRate * ManipulableTime.deltaTime;
			if (amountToRegen > permanentlyUsedPP)
			{
				amountToRegen -= permanentlyUsedPP;
				permanentlyUsedPP = 0.0;
				CurrentPP += amountToRegen;
			}
			else
			{
				permanentlyUsedPP -= amountToRegen;
			}
		}

		public void UsePP(double amount, bool restoredWithRewind)
		{
			if (!restoredWithRewind)
			{
				permanentlyUsedPP += amount;
			}
			else
			{
				currentPP -= amount;
			}
		}

		public void RemoveMaxPP(double amount)
		{
			CurrentMaxPP -= amount;
		}

		protected override void WriteCurrentState(TimelineRecord_StoredPower record)
		{
			record.currentMaxPP = currentMaxPP;
			record.currentPP = currentPP;
		}

		protected override void ApplyRecordedState(TimelineRecord_StoredPower record)
		{
			currentMaxPP = record.currentMaxPP;
			currentPP = record.currentPP;
		}
	}

	public class TimelineRecord_StoredPower : TimelineRecordForBehaviour
	{
		public double currentMaxPP;
		public double currentPP;
	}
}
