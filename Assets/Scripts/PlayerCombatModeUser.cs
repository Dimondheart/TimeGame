using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;
using System;

namespace TechnoWolf.Project1
{
	/**<summary></summary>*/
	public abstract class PlayerCombatModeUser : MonoBehaviour, ITimelineRecordable
	{
		public PlayerCombatMode.CombatMode currentCombatMode { get; private set; }

		protected abstract void ChangeToOffensive();
		protected abstract void ChangeToDefensive();
		protected abstract void ChangeToRanged();
		protected abstract void ChangeToUnarmed();

		protected virtual void Start()
		{
			ChangeCombatMode();
		}

		protected virtual void Update()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			if (GetComponent<PlayerCombatMode>().currentMode != currentCombatMode)
			{
				currentCombatMode = GetComponent<PlayerCombatMode>().currentMode;
				ChangeCombatMode();
			}
		}

		private void ChangeCombatMode()
		{
			switch (currentCombatMode)
			{
				case PlayerCombatMode.CombatMode.Offensive:
					ChangeToOffensive();
					break;
				case PlayerCombatMode.CombatMode.Defensive:
					ChangeToDefensive();
					break;
				case PlayerCombatMode.CombatMode.Ranged:
					ChangeToRanged();
					break;
				case PlayerCombatMode.CombatMode.Unarmed:
					ChangeToUnarmed();
					break;
			}
		}

		public abstract TimelineRecordForBehaviour MakeTimelineRecord();

		public virtual void RecordCurrentState(TimelineRecordForBehaviour record)
		{
			((TimelineRecord_PlayerCombatModeUser)record).currentCombatMode = currentCombatMode;
		}

		public virtual void ApplyTimelineRecord(TimelineRecordForBehaviour record)
		{
			currentCombatMode = ((TimelineRecord_PlayerCombatModeUser)record).currentCombatMode;
		}

		public abstract class TimelineRecord_PlayerCombatModeUser : TimelineRecordForBehaviour
		{
			public PlayerCombatMode.CombatMode currentCombatMode;
		}
	}
}
