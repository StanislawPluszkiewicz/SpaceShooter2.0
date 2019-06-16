namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Platform : MonoBehaviour
    {
        /// <summary>
        /// The prefab to use when the platform begins
        /// </summary>
        public GameObject m_BeginSection;
        /// <summary>
        /// The prefab to use in a loop while the platform is not ended
        /// </summary>
        public GameObject m_MiddleSection;
        /// <summary>
        /// The prefab to use when the platform ends
        /// </summary>
        public GameObject m_EndSection;


		void OnBecameInvisible()
		{
			Destroy(gameObject);
		}
    }
}