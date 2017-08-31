using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	 * when it is near 0 (except when applying a gain rate.)</summary>
	 */
	public static readonly float minElementValue = 0.008f;

	public float minGainRate = 1.0f / 60.0f;
	public float maxGainRate = 0.05f;
	public float useToMaxGainRate = 1.0f;

	private Element gainFocus = Element.None;
	private float temperatureUsed = 0.0f;
	private float moistureUsed = 0.0f;
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
			if (ManipulableTime.ApplyingTimelineRecords)
			{
				return;
			}
			if ((value ^ gainFocus) != Element.None)
			{
				if ((value & Element.Hot) != Element.None || (value & Element.Cold) != Element.None)
				{
					temperatureUsed = 0.0f;
				}
				if ((value & Element.Wet) != Element.None || (value & Element.Dry) != Element.None)
				{
					moistureUsed = 0.0f;
				}
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
			if (ManipulableTime.ApplyingTimelineRecords)
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
			if (ManipulableTime.ApplyingTimelineRecords)
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

	TimelineRecord ITimelineRecordable.MakeTimelineRecord()
	{
		TimelineRecord_ElementalAlignment record = new TimelineRecord_ElementalAlignment();
		return record;
	}

	void ITimelineRecordable.ApplyTimelineRecord(TimelineRecord record)
	{
	}

	private void Awake()
	{
		GainFocus = Element.Ice;
	}

	private void Update()
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		if (ManipulableTime.IsTimeFrozen)
		{
			return;
		}
		if (temperatureGainRate != 0.0f)
		{
			temperature = Mathf.Clamp(temperature + temperatureGainRate * Time.deltaTime, -1.0f, 1.0f);
			UpdateTempGainRate();
		}
		if (moistureGainRate != 0.0f)
		{
			moisture = Mathf.Clamp(moisture + moistureGainRate * Time.deltaTime, -1.0f, 1.0f);
			UpdateMoistureGainRate();
		}
	}

	public void UseTemperature(float use)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		use = Mathf.Clamp(use, 0.0f, Mathf.Abs(temperature));
		Temperature -= (temperature >= 0.0f) ? use : -use;
		temperatureUsed += Mathf.Abs(use);
		UpdateTempGainRate();
	}

	public void UseMoisture(float use)
	{
		if (ManipulableTime.ApplyingTimelineRecords)
		{
			return;
		}
		use = Mathf.Clamp(use, 0.0f, Mathf.Abs(moisture));
		Moisture -= (moisture >= 0.0f) ? use : -use;
		moistureUsed += Mathf.Abs(use);
		UpdateMoistureGainRate();
	}

	public void ReleaseAll()
	{
		Temperature = 0.0f;
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
		Ice = Cold | Wet
	}

	public class TimelineRecord_ElementalAlignment : TimelineRecordForComponent
	{
	}
}
