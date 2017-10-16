using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.TimeManipulation
{
	/**<summary>Base for the generic class of the same name, allows all timeline
	 * records to be used without knowing the generic type.</summary>
	 */
	public abstract class TimelineRecord
	{
		/**<summary>Write the current state of the recordable into this record.</summary>*/
		public abstract void WriteRecord(object recordable);
		/**<summary>Apply this record over the current state of the recordable.</summary>*/
		public abstract void ApplyRecord(object recordable);
	}

	/**<summary>Base class for all timeline records which follows the record-centric
	 * algorithm. The record-centric algorithm requires records to be declared nested
	 * inside the class they are recording, and each class that overides the write
	 * and record methods must also call the base implementation for that method.
	 * Abstract classes must define a record that is both abstract and generic (for
	 * inheritance). Sealed classes must define a sealed record. Classes that are not
	 * abstract and not sealed must define one generic/abstract record and one sealed
	 * record. A sealed record in this situation should be empty and just inherit everything
	 * from the generic/abstract record. This setup allows for the recordable classes
	 * themselves to extend and implement without being declared generic, while allowing
	 * the record writing/applying functions to declare their parameters as the
	 * proper type so they do not have to be casted.</summary>
	 */
	public abstract class TimelineRecord<T> : TimelineRecord
	{
		/**<summary>Overriden by subclasses to apply the record to the given recordable.
		 * Must always call the base implementation of this method so inherited
		 * classes are also applied properly.</summary>
		 */
		protected virtual void ApplyRecord(T recordable)
		{
		}

		/**<summary>Overriden by subclasses to write the recordables state to this record.
		 * Must always call the base implementation of this method so inherited
		 * classes are also recorded properly.</summary>
		 */
		protected virtual void RecordState(T recordable)
		{
		}

		public override sealed void ApplyRecord(object recordable)
		{
			ApplyRecord((T)recordable);
		}

		public override sealed void WriteRecord(object recordable)
		{
			RecordState((T)recordable);
		}
	}
}
