using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Standard MP concept.</summary>*/
	public class MagicPoints : RecordableMonoBehaviour<TimelineRecord_MagicPoints>, IPrimaryValue
	{
		public float initialMaxMP = 100.0f;
		public float regenRate = 10.0f;
		private float absoluteMaxMP;
		private float maxMP = float.PositiveInfinity;
		private float currentMP = float.PositiveInfinity;

		public float AbsoluteMaxMP
		{
			get
			{
				return absoluteMaxMP;
			}
			private set
			{
				absoluteMaxMP = value;
				// Easy auto adjust of the 2 dependent values
				CurrentMaxMP = CurrentMaxMP;
			}
		}

		public float CurrentMaxMP
		{
			get
			{
				return maxMP;
			}
			private set
			{
				maxMP = Mathf.Clamp(value, 0.0f, absoluteMaxMP);
				// Easy auto adjust
				CurrentMP = CurrentMP;
			}
		}

		public float CurrentMP
		{
			get
			{
				return currentMP;
			}
			private set
			{
				float val = Mathf.Clamp(value, 0.0f, maxMP);
				if (val < 0.95f)
				{
					currentMP = 0.0f;
				}
				else
				{
					currentMP = val;
				}
			}
		}

		float IPrimaryValue.MaxValue
		{
			get
			{
				return absoluteMaxMP;
			}
		}

		float IPrimaryValue.MaxCurrentValue
		{
			get
			{
				return maxMP;
			}
		}

		float IPrimaryValue.CurrentValue
		{
			get
			{
				return currentMP;
			}
		}

		protected override void RecordCurrentState(TimelineRecord_MagicPoints record)
		{
			record.absoluteMaxMP = absoluteMaxMP;
			record.maxMP = maxMP;
			record.regenRate = regenRate;
			record.currentMP = currentMP;
		}

		protected override void ApplyRecordedState(TimelineRecord_MagicPoints record)
		{
			absoluteMaxMP = record.absoluteMaxMP;
			maxMP = record.maxMP;
			regenRate = record.regenRate;
			currentMP = record.currentMP;
		}

		private void Awake()
		{
			AbsoluteMaxMP = initialMaxMP;
			currentMP = 0.0f;
		}

		protected override void FlowingUpdate()
		{
			if (currentMP >= maxMP)
			{
				return;
			}
			float regenAmount = regenRate * ManipulableTime.deltaTime;
			if (!GetComponent<ElementalAlignment>().IsStable)
			{
				regenAmount *= 0.5f;
			}
			float newMP = currentMP + regenAmount;
			if (newMP > maxMP)
			{
				regenAmount = newMP - maxMP;
				newMP = maxMP;
			}
			double powerAvailable = GetComponent<StoredPower>().CurrentPP;
			if (regenAmount > powerAvailable)
			{
				regenAmount = (float)powerAvailable;
				GetComponent<StoredPower>().UsePP(powerAvailable, true);
				GetComponent<StoredPower>().RemoveMaxPP(powerAvailable * 0.25);
				newMP = currentMP + regenAmount;
			}
			else
			{
				GetComponent<StoredPower>().UsePP(regenAmount, true);
				GetComponent<StoredPower>().RemoveMaxPP(regenAmount * 0.25f);
			}
			currentMP = newMP;
		}
	}

	public class TimelineRecord_MagicPoints : TimelineRecordForBehaviour
	{
		public float mpUsed;
		public float absoluteMaxMP;
		public float maxMP;
		public float regenRate;
		public float currentMP;
	}
}
