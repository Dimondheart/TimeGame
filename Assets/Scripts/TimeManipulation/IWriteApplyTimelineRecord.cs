using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Interface for writing timeline records and applying them.</summary>*/
	public interface IWriteApplyTimelineRecord<T>
	{
		void WriteCurrentState(T recordable);
		void ApplyRecordedState(T recordable);
	}
}
