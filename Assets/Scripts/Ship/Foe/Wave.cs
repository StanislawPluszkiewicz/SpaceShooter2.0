namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Wave : MonoBehaviour
	{
		public float m_TimeToClear;
		public bool m_CanBeSkipedAfterDelay;
		public List<Foe> m_FoePrefabs;
		[HideInInspector] public List<Foe> m_Foes;
		public float m_TimeBetweenSpawns;

		private Vector3 m_SpawnPosition;

		public IEnumerator Spawn()
		{
			Transform player = GameManager.Instance.m_Player.transform;
			m_SpawnPosition = player.position + player.up * 10 + player.forward * 30;
			int i = 0;
			foreach(Foe foePrefab in m_FoePrefabs)
			{
				print(i);
				Foe foe = Instantiate(foePrefab, m_SpawnPosition, Quaternion.identity, transform) as Foe;
				foe.transform.LookAt(foe.transform.position - player.forward);
				foe.wave = this;
				m_Foes.Add(foe);
				yield return new WaitForSeconds(m_TimeBetweenSpawns);
			}
		}

		public bool isCleared()
		{
			return m_Foes.Count == 0;
		}

		public bool DestroyFoe(Foe foe)
		{
			return m_Foes.Remove(foe);
		}
	}
}
