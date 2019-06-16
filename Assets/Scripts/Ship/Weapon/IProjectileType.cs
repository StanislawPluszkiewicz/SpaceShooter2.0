namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IProjectileType
    {
		IEnumerator MoveProjectile(Projectile projectile, Vector3 targetDirection);
    }
}