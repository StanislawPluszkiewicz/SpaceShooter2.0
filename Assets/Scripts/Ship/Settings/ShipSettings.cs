namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu()]
	public class ShipSettings : ScriptableObject
	{

		#region Stats
        [HideInInspector] public int m_CurrentHealthPoints;
        public int m_MaxHealthPoints;
		#endregion


		public List<Weapon> m_WeaponPrefabs;
		public Shield m_ShieldPrefab;

		#region Movement
		public float m_AutoSpeedForward = 2.5f;
		public float m_MoveSpeed = 10;
		public float m_LIRotation = 3;
		public float m_LIPosition = 3;
		#endregion
	}
}
