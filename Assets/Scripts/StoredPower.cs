using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

/**<summary>Finite stored power available for use.</summary>*/
public class StoredPower : MonoBehaviour, IPrimaryValue, ITimelineRecordable
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

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords || ManipulableTime.IsTimeFrozen || regenRate == 0.0)
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

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_StoredPower record = new TimelineRecord_StoredPower();
		record.currentMaxPP = currentMaxPP;
		record.currentPP = currentPP;
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
		TimelineRecord_StoredPower rec = (TimelineRecord_StoredPower)record;
		currentMaxPP = rec.currentMaxPP;
		currentPP = rec.currentPP;
	}

	public class TimelineRecord_StoredPower : TimelineRecordForComponent
	{
		public double currentMaxPP;
		public double currentPP;
	}
}
