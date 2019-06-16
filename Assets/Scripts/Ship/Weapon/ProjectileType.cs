namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectileType : ScriptableObject, IProjectileType
	{
		public Transform m_ShootPosition { get; private set; }
		private ProjectileSettings settings;

		public ProjectileType(ProjectileSettings settings)
		{
			this.settings = settings;
		}


		void SetShootPosition(Transform pos)
		{
			if (!m_ShootPosition)
				m_ShootPosition = pos;
		}
		public virtual IEnumerator MoveProjectile(Projectile projectile, Vector3 targetDirection)
        {
			Debug.Log("Not implemented");
            throw new UnityException("Not implemented");
        }


        public class Straight : ProjectileType
        {
			public Straight(ProjectileSettings settings) : base(settings)
			{
			}

			public override IEnumerator MoveProjectile(Projectile projectile, Vector3 targetDirection)
            {
				while (projectile && projectile.gameObject.activeInHierarchy)
				{
					SetShootPosition(projectile.transform);
					ProjectileSettings.Straight param = settings.m_Straight;

					//float sine = param.m_Amplitude * Mathf.Sin(2 * Mathf.PI * param.m_Frequency * Time.deltaTime + param.m_AngularFrequency);
					//float cosine = param.m_Amplitude * Mathf.Cos(2 * Mathf.PI * param.m_Frequency * Time.deltaTime + param.m_AngularFrequency);
					//Vector3 newPosition = new Vector3(targetDirection.x * cosine, 0, targetDirection.z * sine);
					projectile.m_Speed = Mathf.Clamp(projectile.m_Speed*projectile.m_Acceleration, 0, projectile.m_MaxSpeed);

					Vector3 newPosition = projectile.transform.position + targetDirection * Time.deltaTime * projectile.m_Speed;
					projectile.transform.position = newPosition;
					yield return null; // 1 frame

				}
            }
        }

		public class Manual : ProjectileType
		{
			public Manual(ProjectileSettings settings) : base(settings)
			{
			}

			public override IEnumerator MoveProjectile(Projectile projectile, Vector3 targetDirection)
			{
				throw new UnityException("Not implemented");
			}
		}
    }
}
