namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : Ship
    {
		private CameraController m_Camera;
        private Color m_GizmosColor;
        public Shield m_Shield;

        [HideInInspector] public Vector3 m_MovementDirection;

		[HideInInspector] public int m_iMoney;

		#region Init
		public static void Init()
		{
			if (GameManager.Instance.m_Player == null)
				GameManager.Instance.m_Player = Instantiate(GameManager.Instance.m_PlayerPrefab, GameManager.Instance.m_PlayerSpawnPosition, Quaternion.identity) as Player;
			GameManager.Instance.m_Player.InitPlayer();
		}
        #endregion

        void Start()
        {
			base.Start();
			InitPlayer();
		}

		public void InitPlayer()
		{
			transform.position = GameManager.Instance.m_PlayerSpawnPosition;
			transform.rotation = Quaternion.identity;
			this.m_iMoney = 0;
			this.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer = this.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer;
			m_Camera = FindObjectOfType<CameraController>();

			foreach (Weapon weapon in m_Settings.m_WeaponPrefabs)
			{
				weapon.belongsToPlayer = true;
			}

			m_Shield = Instantiate(m_Settings.m_ShieldPrefab, transform.position, Quaternion.identity, transform) as Shield;
		}

		void Update()
		{
			base.Update();
			// Movement
			m_MovementDirection = GetMovementDirectionRelativeToPlayer();
			Move(m_MovementDirection.normalized);

			// Shoot
			PlayerShoot();
        }

		#region Collisions
		private void OnCollisionEnter(Collision other)
		{
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			if (other.gameObject.layer == LayerMask.NameToLayer("undestructibleProps"))
			{
				CollislionHit(pos, rot);
				TakeDamage(GameManager.Instance.m_UndestructiblePropsDamage);
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("destructibleProps"))
			{
				CollislionHit(pos, rot);
				Destroy(other.gameObject);
				TakeDamage(GameManager.Instance.m_DestructiblePropsDamage);
			}
		}
		void CollislionHit(Vector3 position, Quaternion rotation)
		{
			if (hitPrefab != null)
			{
				var hitVFX = Instantiate(hitPrefab, position, rotation, GameManager.Instance.m_VFXParent.transform);
				var psHit = hitVFX.GetComponent<ParticleSystem>();
				Destroy(hitVFX, psHit.main.duration);
			}
		}
		#endregion
		#region Shoot
		private void PlayerShoot()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				Shoot(transform.position, transform.forward);
			}
		}
		public void TakeDamage(int amount)
		{
			base.TakeDamage(amount);
			if (m_Settings.m_CurrentHealthPoints <= 0)
			{
				GameManager.Instance.Lose();
			}
		}

		#endregion
		#region Movement
		private Vector3 GetMovementDirectionRelativeToCamera ()
		{
			Vector3 movementDirection = Vector3.zero;
			if (Input.GetAxisRaw("Vertical") > 0)
			{
				movementDirection += m_Camera.transform.up;
			}
			else if (Input.GetAxisRaw("Vertical") < 0)
			{
				movementDirection += -m_Camera.transform.up;
			}

			if (Input.GetAxisRaw("Horizontal") > 0)
			{
				movementDirection += m_Camera.transform.right;
			}
			else if (Input.GetAxisRaw("Horizontal") < 0)
			{
				movementDirection += -m_Camera.transform.right;
			}
			movementDirection.Normalize();
			return movementDirection;
		}
		private Vector3 GetMovementDirectionRelativeToPlayer ()
		{
			float hInput = Input.GetAxis("Horizontal");
			float vInput = Input.GetAxis("Vertical");

			return (Vector3.right * hInput) + (Vector3.forward *  vInput);
		}

		#endregion

        public void ShieldHit()
        {
            m_Shield.m_Settings.m_iCurrentLayer--;

            StartCoroutine(m_Shield.Recharge());

            if (m_Shield.m_Settings.m_iCurrentLayer == 0)
            {
                m_Shield.Hide();
            }
            else
            {
                m_Shield.Show();
            }
        }

		#region Debug
		private void OnDrawGizmos()
        {
			// FOR DEBUG PURPOSES
        }
		#endregion
	}
}