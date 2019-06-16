namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Weapon : MonoBehaviour
	{

		[HideInInspector] public bool belongsToPlayer;

		#region Cooldown
		public float m_Cooldown;
		[HideInInspector] public float m_CooldownTimeStamp = 0f;
		#endregion


		public Projectile m_Projectile;
		//public string m_Name;
		public float m_Cost;
		public void Start()
		{
			m_CooldownTimeStamp = 0f;
		}

		public void Shoot(Vector3 shooterPosition, Vector3 direction)
		{
			if (LevelManager.Instance.m_Levels[LevelManager.Instance.m_CurrentLevelIndex].isActive)
			{

				if (m_CooldownTimeStamp <= Time.time)
				{
					Projectile projectilePrefab = m_Projectile.m_Settings.m_Projectile;
					Projectile projectile = Instantiate(projectilePrefab, shooterPosition,
						Quaternion.identity, GameManager.Instance.m_ProjectilesParent.transform) as Projectile;
					projectile.transform.LookAt(projectile.transform.position + direction);
					projectile.InitMove(direction);
					if (belongsToPlayer)
					{
						projectile.gameObject.layer = 16;
					}
					else
					{
						projectile.gameObject.layer = 17;
					}
					m_CooldownTimeStamp = Time.time + m_Cooldown;
				}
			}
		}
	}
}