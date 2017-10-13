using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnoWolf.TimeManipulation;

namespace TechnoWolf.Project1
{
	/**<summary>Track statistics and other data on all characters currently in
	 * the scene.</summary>
	 */
	public class CharacterTracker : RecordableMonoBehaviour<TimelineRecord_CharacterTracker>
	{
		private List<PlayerTrackerHelper> players = new List<PlayerTrackerHelper>();
		private List<EnemyTrackerHelper> enemies = new List<EnemyTrackerHelper>();

		/**<summary>Total number of characters being tracked.</summary>*/
		public int TotalCharacterCount
		{
			get
			{
				return TotalPlayerCount + TotalEnemyCount;
			}
		}
		/**<summary>Total number of players being tracked.</summary>*/
		public int TotalPlayerCount
		{
			get
			{
				return players.Count;
			}
		}
		/**<summary>Total number of enemies being tracked.</summary>*/
		public int TotalEnemyCount
		{
			get
			{
				return enemies.Count;
			}
		}
		/**<summary>Number of characters left alive.</summary>*/
		public int LiveCharacterCount
		{
			get
			{
				return LivePlayerCount + LiveEnemyCount;
			}
		}
		/**<summary>Number of players left alive.</summary>*/
		public int LivePlayerCount
		{
			get
			{
				int count = 0;
				foreach (Component t in players)
				{
					if (t.GetComponent<Health>().IsAlive)
					{
						count++;
					}
				}
				return count;
			}
		}
		/**<summary>Number of enemies left alive.</summary>*/
		public int LiveEnemyCount
		{
			get
			{
				int count = 0;
				foreach (Component t in enemies)
				{
					if (t.GetComponent<Health>().IsAlive)
					{
						count++;
					}
				}
				return count;
			}
		}

		protected override void WriteCurrentState(TimelineRecord_CharacterTracker record)
		{
			record.players = players.ToArray();
			record.enemies = enemies.ToArray();
		}

		protected override void ApplyRecordedState(TimelineRecord_CharacterTracker record)
		{
			players.Clear();
			players.AddRange(record.players);
			enemies.Clear();
			enemies.AddRange(record.enemies);
		}

		/**<summary>Add a player character to the tracking system.</summary>*/
		public void AddPlayer(PlayerTrackerHelper helper)
		{
			if (players.Contains(helper))
			{
				return;
			}
			players.Add(helper);
		}

		/**<summary>Add an enemy to the tracking system.</summary>*/
		public void AddEnemy(EnemyTrackerHelper helper)
		{
			if (enemies.Contains(helper))
			{
				return;
			}
			enemies.Add(helper);
		}
	}

	public class TimelineRecord_CharacterTracker : TimelineRecordForBehaviour
	{
		public PlayerTrackerHelper[] players;
		public EnemyTrackerHelper[] enemies;
	}
}
