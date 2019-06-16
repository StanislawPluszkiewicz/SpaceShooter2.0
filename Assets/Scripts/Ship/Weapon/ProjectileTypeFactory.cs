namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ProjectileTypeFactory : ScriptableObject
	{
		public static ProjectileType Create(ProjectileSettings settings)
		{
			switch (settings.type)
			{
				case ProjectileSettings.Type.Straight:
					return new ProjectileType.Straight(settings);
				case ProjectileSettings.Type.Manual:
					return new ProjectileType.Manual(settings);
			}
			return null;
		}
	}
}
