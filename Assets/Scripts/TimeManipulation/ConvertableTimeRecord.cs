using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Records an instant in time with both the Unity Time
 * and ManipulableTime.</summary>
 */
	public struct ConvertableTimeRecord
	{
		public static readonly ConvertableTimeRecord zeroTime = new ConvertableTimeRecord(0.0f, 0.0f, false);
		public static readonly ConvertableTimeRecord zeroFixedTime = new ConvertableTimeRecord(0.0f, 0.0f, true);
		/**<summary>The time of this record in Unity time.</summary>*/
		public float unityTime { get; private set; }
		/**<summary>The time of this record in manipulable time.</summary>*/
		public float manipulableTime { get; private set; }
		/**<summary>Defines if this record is for normal time or
		 * fixed (physics) time.</summary>
		 */
		public bool isFixedTime { get; private set; }

		private ConvertableTimeRecord(float unityTime, float manipulableTime, bool isFixedTime)
		{
			this.unityTime = unityTime;
			this.manipulableTime = manipulableTime;
			this.isFixedTime = isFixedTime;
		}

		/**<summary>Get the current Time.time & ManipulableTime.time.</summary>*/
		public static ConvertableTimeRecord GetTime()
		{
			return new ConvertableTimeRecord(Time.time, ManipulableTime.time, false);
		}

		/**<summary>Get the current Time.fixedTime & ManipulableTime.fixedTime.</summary>*/
		public static ConvertableTimeRecord GetFixedTime()
		{
			return new ConvertableTimeRecord(Time.fixedTime, ManipulableTime.fixedTime, true);
		}

		/**<summary>Set the time of this record.</summary>*/
		public void SetTime(float unityTime, float manipulableTime)
		{
			this.unityTime = unityTime;
			this.manipulableTime = manipulableTime;
		}

		/**<summary>Set this record to the current time or fixed time.</summary>*/
		public void SetToCurrent()
		{
			if (isFixedTime)
			{
				SetTime(Time.fixedTime, ManipulableTime.fixedTime);
			}
			else
			{
				SetTime(Time.time, ManipulableTime.time);
			}
		}
	}
}
