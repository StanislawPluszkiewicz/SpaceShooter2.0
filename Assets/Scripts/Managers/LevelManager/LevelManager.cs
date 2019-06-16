namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;
	using SDD.Events;

	public class LevelManager : Manager<LevelManager>
	{
		public float m_ShipDistanceToGround;
		[Range(0, 20)] public int m_VisibleNextPlatforms;
		public List<Level> m_Levels;
		[HideInInspector] public int m_CurrentLevelIndex;

		[HideInInspector] public PlatformManager m_SpawnedPlatforms;
		[HideInInspector] public bool m_SpawnNext = true;


		[HideInInspector] public List<Wave> m_Waves;

		public void Start()
		{
			m_CurrentLevelIndex = -1;
		}

		public void DestroyFoe(Foe foe)
		{
			foreach(Wave wave in m_Waves)
			{
				wave.DestroyFoe(foe);
			}
		}

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion
		public void StartLevel(int index)
		{
			if (gameObject.GetComponent<PlatformManager>() == null)
			{
				m_SpawnedPlatforms = gameObject.AddComponent<PlatformManager>();
			}
			m_CurrentLevelIndex = index;
			m_Levels[m_CurrentLevelIndex].isActive = true;
			m_SpawnedPlatforms.InitLevel();
			StartCoroutine(SpawnWaves());
		}
		public void EndLevel()
		{
			Destroy(GameManager.Instance.m_DynamicParent);
			Destroy(m_SpawnedPlatforms);
		}

		private IEnumerator SpawnWaves()
		{
			int index = 0;
			foreach (Wave wave in m_Levels[m_CurrentLevelIndex].m_WaveTemplates)
			{
				Wave instance = Instantiate(wave) as Wave;
				instance.transform.parent = GameManager.Instance.m_DynamicParent.transform;
				m_Waves.Add(instance);
				StartCoroutine(instance.Spawn());
				print("waiting for " + instance.m_TimeToClear + " seconds");
				yield return new WaitForSeconds(instance.m_TimeToClear);
				index++;
			}
			StartCoroutine(CheckLevelEnd());
		}

		private IEnumerator CheckLevelEnd()
		{
			//print("checking for level end");
			while (m_Waves.Count > 0)
			{
				//print("waves count:" + m_Waves.Count);
				for(int i = 0; i < m_Waves.Count; ++i)
				{
					//print("i:" + i);
					//print("wave is cleared" + m_Waves[i].isCleared());
					//print("foes in wave: " + m_Waves[i].m_Foes.Count);
					if (m_Waves[i].isCleared())
					{
						m_Waves.Remove(m_Waves[i]);
						--i;
					}
				}
				yield return null;
			}
			GameManager.Instance.EndLevel();
		}

		#region Manager stuff
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
		}

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();
		}

		protected override void GamePlay(GamePlayEvent e)
		{
		}

		protected override void GameMenu(GameMenuEvent e)
		{
		}
		#endregion
	}

	public class PlatformManager : MonoBehaviour
	{

        public Platform Previous
        {
            get
            {
                return platforms.First.Value;
            }
        }
        public Platform Current
        {
            get
            {
                return platforms.First.Next.Value;
            }
        }
        public LinkedList<Platform> platforms;

        LevelManager m_LevelController;
        int m_AllSpawnedPlatformsCount; // includes destroyed ones

		public void InitLevel()
		{
			m_LevelController = FindObjectOfType<LevelManager>();
			platforms = new LinkedList<Platform>();
			for (int i = 0; i < m_LevelController.m_Levels[m_LevelController.m_CurrentLevelIndex].m_PlatformTemplates.Count; i++)
			{
				platforms.AddLast(SpawnPlatform());
			}
		}

		public void AddNew(Platform p)
		{
            while (platforms.Count > m_LevelController.m_VisibleNextPlatforms)
            {
                Platform prev = Previous;
                platforms.RemoveFirst();
                Destroy(p, 1f);
            }

            platforms.AddLast(p);

            while (platforms.Count < m_LevelController.m_VisibleNextPlatforms)
            {
                platforms.AddLast(SpawnPlatform());
            }

            // if size is adjusted through execution
            // Debug.Log("platforms.Count:" + platforms.Count + ", m_LevelController.m_VisibleNextPlatforms:" + m_LevelController.m_VisibleNextPlatforms);
		}
        public void AddNew()
        {
            AddNew(SpawnPlatform());
        }
        public Platform SpawnPlatform()
        {
			int index = Random.Range(0, m_LevelController.m_Levels[m_LevelController.m_CurrentLevelIndex].m_PlatformTemplates.Count);
			if (m_AllSpawnedPlatformsCount <= 4)
			{
				index = 0;
			}
			Platform platformPrefab = m_LevelController.m_Levels[m_LevelController.m_CurrentLevelIndex].m_PlatformTemplates[index];
            Vector3 spawnPoint = transform.position + (Vector3.forward * platformPrefab.m_MiddleSection.transform.localScale.z * m_AllSpawnedPlatformsCount);
            m_AllSpawnedPlatformsCount++;

			Quaternion spawnRotation = Quaternion.Euler(0, 0, Random.Range(0, 3) * 90);
            Platform p = Instantiate(platformPrefab, spawnPoint, spawnRotation, GameManager.Instance.m_PlateformesParent.transform) as Platform;
			p.gameObject.AddComponent<SolarSystem>();
            return p;
        }
        private void OnDestroy()
        {
            foreach (Platform p in platforms)
            {
                Destroy(p);
            }
        }
    }
}

