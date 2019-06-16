namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Shield : MonoBehaviour
    {
        public ShieldSettings m_Settings;

        public void OnCollisionEnter(Collision collision)
        {
            print("Collison with shield");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            m_Settings.m_CooldownTimeStamp = 0;
        }

        public IEnumerator Recharge()
        {
            print("Current layer:" + m_Settings.m_iCurrentLayer);
            yield return new WaitForSeconds(Time.time + m_Settings.m_Cooldown);
            m_Settings.m_iCurrentLayer = Mathf.Clamp(m_Settings.m_iCurrentLayer++,0,m_Settings.m_iTotalLayer);
        }

    }

}
