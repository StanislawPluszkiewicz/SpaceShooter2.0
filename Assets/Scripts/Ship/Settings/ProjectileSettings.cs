namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu()]
    public class ProjectileSettings : ScriptableObject
	{
		public enum Type { Straight, Manual }


		[Header("General")]
		public int m_Damage;
		public Projectile m_Projectile;
		public Transform m_SpawnTransform;




		public bool m_Random;

		public Type type;
		public Straight m_Straight;
        public Manual m_Manual;
		
		[System.Serializable]
        public class Straight
        {
            public float m_Frequency;
            public float m_Amplitude;
            public float m_AngularFrequency;

            public bool m_UsePhase;
            public float m_Phase;

        }

        [System.Serializable]
        public class Manual
        {
            public AnimationCurve m_MovementCurve;
        }
    }
}
