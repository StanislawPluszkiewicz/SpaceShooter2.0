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

		private Vector3 m_SpawnPosition;

		public void Spawn()
		{
			Transform player = GameManager.Instance.m_Player.transform;
			print(GameManager.Instance.m_Player);
			m_SpawnPosition = player.position + player.up * 10 + player.forward * 30;
			foreach(Foe foePrefab in m_FoePrefabs)
			{
				Foe foe = Instantiate(foePrefab, m_SpawnPosition, Quaternion.identity, transform) as Foe;
				foe.transform.LookAt(foe.transform.position - player.forward);
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
