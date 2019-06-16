namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class EndPlatformCollider : MonoBehaviour
	{
		LevelManager m_LevelManager;

		private void Start()
		{
			m_LevelManager = FindObjectOfType<LevelManager>();
		}
		private void OnTriggerEnter(Collider other)
		{

			if (m_LevelManager.m_SpawnNext)
			{
				m_LevelManager.m_SpawnNext = false;
				m_LevelManager.m_SpawnedPlatforms.AddNew();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			m_LevelManager.m_SpawnNext = true;
		}
	}
}
