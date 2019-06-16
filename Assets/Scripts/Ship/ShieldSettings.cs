namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu()]
    public class ShieldSettings : ScriptableObject
    {
        public enum Type { Ion, Laser }
        public int m_iTotalLayer;
        [HideInInspector] public int m_iCurrentLayer;

        public Type m_CurrentType;

        #region Cooldown
		public float m_Cooldown;
		[HideInInspector] public float m_CooldownTimeStamp;
		#endregion
    }
}