using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Elemental alignment is the amount of each element
	 * stored in something.</summary>
	 */
	public class ElementalAlignment : MonoBehaviour, ITimelineRecordable
	{
		/**<summary>Minimum value for an elemental alignment to be considered
		 * aligned with an element.</summary>
		 */
		public static readonly float minAlignmentValue = 0.08f;
		/**<summary>Minimum value to set an element value to, so it is rounded
		 * when it is set near 0 (except when applying a gain rate.)</summary>
		 */
		public static readonly float minElementValue = 0.008f;

		/**<summary>The minimum rate at which an element value will increase.</summary>*/
		public float minGainRate = 1.0f / 60.0f;
		/**<summary>The max rate at which an element value will increase.</summary>*/
		public float maxGainRate = 0.05f;
		public bool dynamicAlignment;

		private Element gainFocus = Element.None;
		private float temperature;
		private float moisture;

		public float temperatureGainRate { get; private set; }
		public float moistureGainRate { get; private set; }

		public Element GainFocus
		{
			get
			{
				return gainFocus;
			}
			set
			{
				if (ManipulableTime.IsApplyingRecords)
				{
					return;
				}
				if ((value ^ gainFocus) != Element.None)
				{
					gainFocus = value;
					UpdateTempGainRate();
					UpdateMoistureGainRate();
				}
			}
		}

		public float Temperature
		{
			get
			{
				return temperature;
			}
			set
			{
				if (ManipulableTime.IsApplyingRecords)
				{
					return;
				}
				if (Mathf.Abs(value) >= minElementValue)
				{
					temperature = value;
				}
				else
				{
					temperature = 0.0f;
				}
			}
		}

		public float Moisture
		{
			get
			{
				return moisture;
			}
			set
			{
				if (ManipulableTime.IsApplyingRecords)
				{
					return;
				}
				if (Mathf.Abs(value) >= minElementValue)
				{
					moisture = value;
				}
				else
				{
					moisture = 0.0f;
				}
			}
		}

		public Element alignment
		{
			get
			{
				Element v = Element.None;
				if (temperature >= minAlignmentValue)
				{
					v = Element.Hot;
				}
				else if (temperature <= -minAlignmentValue)
				{
					v = Element.Cold;
				}
				if (moisture >= minAlignmentValue)
				{
					v |= Element.Wet;
				}
				else if (moisture <= -minAlignmentValue)
				{
					v |= Element.Dry;
				}
				return v;
			}
		}

		public bool IsStable
		{
			get
			{
				return !dynamicAlignment
					|| (
						(Mathf.Abs(temperature) < minElementValue || Mathf.Abs(temperature) > 1.0f - minElementValue)
						&& (Mathf.Abs(moisture) < minElementValue || Mathf.Abs(moisture) > 1.0f - minElementValue)
					);
			}
		}

		TimelineRecordForComponent ITimelineRecordable.MakeTimelineRecord()
		{
			return new TimelineRecord_ElementalAlignment();
		}

		void ITimelineRecordable.RecordCurrentState(TimelineRecordForComponent record)
		{
			TimelineRecord_ElementalAlignment rec = (TimelineRecord_ElementalAlignment)record;
			rec.minGainRate = minGainRate;
			rec.maxGainRate = maxGainRate;
			rec.gainFocus = gainFocus;
			rec.temperature = temperature;
			rec.moisture = moisture;
			rec.temperatureGainRate = temperatureGainRate;
			rec.moistureGainRate = moistureGainRate;
		}

		void ITimelineRecordable.ApplyTimelineRecord(TimelineRecordForComponent record)
		{
			TimelineRecord_ElementalAlignment rec = (TimelineRecord_ElementalAlignment)record;
			minGainRate = rec.minGainRate;
			maxGainRate = rec.maxGainRate;
			gainFocus = rec.gainFocus;
			temperature = rec.temperature;
			moisture = rec.moisture;
			temperatureGainRate = rec.temperatureGainRate;
			moistureGainRate = rec.moistureGainRate;
		}

		private void Update()
		{
			if (ManipulableTime.IsApplyingRecords || ManipulableTime.IsTimeOrGamePaused || !dynamicAlignment)
			{
				return;
			}
			if (temperatureGainRate != 0.0f)
			{
				temperature = Mathf.Clamp(temperature + temperatureGainRate * ManipulableTime.deltaTime, -1.0f, 1.0f);
				UpdateTempGainRate();
			}
			if (moistureGainRate != 0.0f)
			{
				moisture = Mathf.Clamp(moisture + moistureGainRate * ManipulableTime.deltaTime, -1.0f, 1.0f);
				UpdateMoistureGainRate();
			}
		}

		public void UseTemperature(float use)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			use = Mathf.Clamp(use, 0.0f, Mathf.Abs(temperature));
			Temperature -= (temperature >= 0.0f) ? use : -use;
			UpdateTempGainRate();
		}

		public void UseMoisture(float use)
		{
			if (ManipulableTime.IsApplyingRecords)
			{
				return;
			}
			use = Mathf.Clamp(use, 0.0f, Mathf.Abs(moisture));
			Moisture -= (moisture >= 0.0f) ? use : -use;
			UpdateMoistureGainRate();
		}

		public void ReleaseTemperature()
		{
			Temperature = 0.0f;
		}

		public void ReleaseMoisture()
		{
			Moisture = 0.0f;
		}

		private void UpdateTempGainRate()
		{
			if ((gainFocus & Element.Hot) != Element.None || (gainFocus & Element.Cold) != Element.None || temperature != 0.0f)
			{
				temperatureGainRate =
					Mathf.Lerp(
						minGainRate,
						maxGainRate,
						Mathf.Abs(temperature)
						)
						* ((gainFocus == Element.Hot || gainFocus == Element.Cold) ? 2.0f : 1.0f)
						* (((gainFocus & Element.Cold) != Element.None || ((gainFocus & Element.Hot) == Element.None && temperature < 0.0f)) ? -1.0f : 1.0f)
						;
			}
			else
			{
				temperatureGainRate = 0.0f;
			}
		}

		private void UpdateMoistureGainRate()
		{
			if ((gainFocus & Element.Dry) != Element.None || (gainFocus & Element.Wet) != Element.None || moisture != 0.0f)
			{
				moistureGainRate =
					Mathf.Lerp(
						minGainRate,
						maxGainRate,
						Mathf.Abs(moisture)
						)
						* ((gainFocus == Element.Dry || gainFocus == Element.Wet) ? 2.0f : 1.0f)
						* (((gainFocus & Element.Dry) != Element.None || ((gainFocus & Element.Wet) == Element.None && moisture < 0.0f)) ? -1.0f : 1.0f)
						;
			}
			else
			{
				moistureGainRate = 0.0f;
			}
		}

		/**<summary>The different type of elements, including the possible
		 * combinations.</summary>
		 */
		public enum Element
		{
			None = 0,
			Hot = 1,
			Cold = 2,
			Dry = 4,
			Wet = 8,
			Fire = Hot | Dry,
			Steam = Hot | Wet,
			Wind = Cold | Dry,
			Ice = Cold | Wet,
			/**<summary>Never set an alignment value to this, only use as a shortcut for bit ops.</summary>*/
			Temp = Hot | Cold,
			/**<summary>Never set an alignment value to this, only use as a shortcut for bit ops.</summary>*/
			Moist = Dry | Wet
		}

		public class TimelineRecord_ElementalAlignment : TimelineRecordForComponent
		{
			public float minGainRate;
			public float maxGainRate;

			public Element gainFocus;
			public float temperature;
			public float moisture;

			public float temperatureGainRate;
			public float moistureGainRate;
		}
	}
}
