﻿namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Foe : Ship
    {
		public List<Vector2> m_PlayerRelativePositions;
		public List<float> m_CooldownsBetweenPositionSwap;
		private float m_CooldownBetweenPositionsTimeStamp;
		private int m_iPlayerRelativePositionsIndex = 0;
        public int m_iReward;

		private Animator animator;
		[HideInInspector] public Wave wave;


		private void Awake()
		{
			base.Awake();
			animator = GetComponent<Animator>();
			m_CooldownBetweenPositionsTimeStamp = 0;
		}

		public void Start()
		{
			base.Start();
			foreach (Weapon weapon in m_Settings.m_WeaponPrefabs)
			{
				weapon.belongsToPlayer = false;
			}
		}

		public void Update()
		{
			base.Update();
			Player player = GameManager.Instance.m_Player;
			Vector3 playerDirection = transform.position - player.transform.position;

			// Movement
			if (m_CooldownBetweenPositionsTimeStamp <= Time.time)
			{
				m_iPlayerRelativePositionsIndex = Mathf.Clamp(m_iPlayerRelativePositionsIndex, 0, m_PlayerRelativePositions.Count);
				m_CooldownBetweenPositionsTimeStamp = Time.time + m_CooldownsBetweenPositionSwap[m_iPlayerRelativePositionsIndex];
			}


			Vector2 playerRelativePosition = m_PlayerRelativePositions[m_iPlayerRelativePositionsIndex];

			Vector3 movementDirection = player.m_MovementDirection + Vector3.forward * playerRelativePosition.y + Vector3.right * playerRelativePosition.x;
			Move(movementDirection);

			// Shoot
			Shoot(transform.position, -transform.forward);
		}

		public void TakeDamage(int amount)
		{
			base.TakeDamage(amount);
			//print("foe took " + amount + " damage");
			if (m_Settings.m_CurrentHealthPoints <= 0)
			{
				wave.DestroyFoe(this);
                GameManager.Instance.m_Player.m_iMoney += m_iReward;
				Destroy(gameObject);
			}
		}

	}
}
