namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu()]
	public class Level : ScriptableObject
	{
		public List<Platform> m_PlatformTemplates;
		public List<Wave> m_WaveTemplates;
        public Material m_CurrentMaterial;
		[HideInInspector] public bool isActive = false;
	}
}