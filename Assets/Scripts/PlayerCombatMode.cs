using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;
using TechnoWolf.DynamicInputSystem;
using System;

namespace TechnoWolf.Project1
{
	/**<summary>Changes the combat mode, used by other scripts.</summary>*/
	public class PlayerCombatMode : MonoBehaviour, ITimelineRecordable
	{
		/**<summary>The current combat mode setting.</summary>*/
		public CombatMode currentMode { get; private set; }
		/**<summary>The combat mode to set the current mode to during late update.</summary>*/
		private CombatMode nextMode;

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_PlayerCombatMode();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_PlayerCombatMode rec = (TimelineRecord_PlayerCombatMode)record;
			rec.currentMode = currentMode;
			rec.nextMode = nextMode;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_PlayerCombatMode rec = (TimelineRecord_PlayerCombatMode)record;
			currentMode = rec.currentMode;
			nextMode = rec.nextMode;
		}

		private void Update()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			if (DynamicInput.GetButtonDown("Offensive Combat Mode"))
			{
				SetCombatMode(CombatMode.Offensive);
			}
			else if (DynamicInput.GetButtonDown("Defensive Combat Mode"))
			{
				SetCombatMode(CombatMode.Defensive);
			}
			else if (DynamicInput.GetButtonDown("Ranged Combat Mode"))
			{
				SetCombatMode(CombatMode.Ranged);
			}
			else if (DynamicInput.GetButtonDown("Unarmed Combat Mode"))
			{
				SetCombatMode(CombatMode.Unarmed);
			}
		}

		private void LateUpdate()
		{
			if (ManipulableTime.IsTimeOrGamePaused)
			{
				return;
			}
			currentMode = nextMode;
		}

		/**<summary>Change the combat mode. Change is not applied until LateUpdate
		 * to synchronize all behaviours that are affected by the combat mode.</summary>
		 */
		public void SetCombatMode(CombatMode mode)
		{
			nextMode = mode;
		}

		public enum CombatMode : byte
		{
			Unarmed = 0,
			Offensive,
			Defensive,
			Ranged
		}

		public class TimelineRecord_PlayerCombatMode : TimelineRecordForComponent
		{
			public CombatMode currentMode;
			public CombatMode nextMode;
		}
	}
}
