namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Foe : Ship
    {
		Animator animator;

		private void Awake()
		{
			base.Awake();
			animator = GetComponent<Animator>();
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

			// Movement
			Vector3 movementDirection = player.m_MovementDirection;
			float playerSpeed = player.m_Settings.m_MoveSpeed + player.m_Settings.m_AutoSpeedForward;
			Move(movementDirection);

			// Shoot
			Vector3 playerDirection = transform.position - player.transform.position;
			Shoot(transform.position, -transform.forward);
		}



	}
}
