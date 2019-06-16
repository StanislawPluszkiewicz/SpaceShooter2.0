namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using SDD.Events;

	public class HudManager : Manager<HudManager>
	{

		//[Header("HudManager")]
		#region Labels & Values
		#region LeftPanel
		public Slider m_LifeSlider;
		public Text m_MoneyText;
		#endregion

		#region RightPanel
		public Image m_WeaponImage;

		#region shield
		public Image m_ShieldTypeImage;
		public Image m_ShieldLayerOneImage;
		public Image m_ShieldLayerTwoImage;
		public Image m_ShieldLayerThreeImage;
		public Image m_ShieldLayerFourImage;

        public Image m_ShieldIon;
        public Image m_ShieldLaser;

        public Image m_ShieldBroken;
        public Image m_ShieldPowered;

		#endregion
		#endregion

		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
            StartCoroutine(UpdateHUD());
            yield break;
		}
		#endregion

		#region Callbacks to GameManager events
		protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
		}
        #endregion


        #region Update Functions
           
        private IEnumerator UpdateHUD()
        {
            while (true)
            {
                if(GameManager.Instance.m_Player != null)
                {
                    UpdateLifeBar();
                    UpdateMoneyValue();
                    UpdateWeaponType();
                    UpdateShieldType();
                    UpdateShieldLayer();
                }
                yield return null;
            }
        }

        private void UpdateLifeBar()
        {
            m_LifeSlider.value = GameManager.Instance.m_Player.m_Settings.m_CurrentHealthPoints * 1.0f / GameManager.Instance.m_Player.m_Settings.m_MaxHealthPoints * 1.0f;
        }

        private void UpdateMoneyValue()
        {
            m_MoneyText.text = GameManager.Instance.m_Player.m_iMoney.ToString();
        }

        private void UpdateWeaponType()
        {

        }

        private void UpdateShieldType()
        {
            if(GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_CurrentType == ShieldSettings.Type.Ion)
            {
                m_ShieldTypeImage.sprite = m_ShieldIon.sprite;
            }
            else
            {
                m_ShieldTypeImage.sprite = m_ShieldLaser.sprite;
            }
            
        }

        private void UpdateShieldLayer()
        {
            #region Max print
            if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer == 1)
            {
                m_ShieldLayerOneImage.enabled = true;
                m_ShieldLayerTwoImage.enabled = false;
                m_ShieldLayerThreeImage.enabled = false;
                m_ShieldLayerFourImage.enabled = false;
            }
            else if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer == 2)
            {
                m_ShieldLayerOneImage.enabled = true;
                m_ShieldLayerTwoImage.enabled = true;
                m_ShieldLayerThreeImage.enabled = false;
                m_ShieldLayerFourImage.enabled = false;
            }
            else if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer == 3)
            {
                m_ShieldLayerOneImage.enabled = true;
                m_ShieldLayerTwoImage.enabled = true;
                m_ShieldLayerThreeImage.enabled = true;
                m_ShieldLayerFourImage.enabled = false;
            }
            else
            {
                m_ShieldLayerOneImage.enabled = true;
                m_ShieldLayerTwoImage.enabled = true;
                m_ShieldLayerThreeImage.enabled = true;
                m_ShieldLayerFourImage.enabled = true;
            }
            #endregion

            #region Current print
            if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer == 1)
            {
                m_ShieldLayerOneImage.transform.Rotate(100 * Vector3.forward * Time.deltaTime);

                m_ShieldLayerOneImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerTwoImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerThreeImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerFourImage.sprite = m_ShieldBroken.sprite;
            }
            else if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer == 2)
            {
                m_ShieldLayerOneImage.transform.Rotate(100 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerTwoImage.transform.Rotate(70 * Vector3.forward * Time.deltaTime);

                m_ShieldLayerOneImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerTwoImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerThreeImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerFourImage.sprite = m_ShieldBroken.sprite;
            }
            else if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer == 3)
            {
                m_ShieldLayerOneImage.transform.Rotate(100 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerTwoImage.transform.Rotate(70 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerThreeImage.transform.Rotate(80 * Vector3.forward * Time.deltaTime);

                m_ShieldLayerOneImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerTwoImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerThreeImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerFourImage.sprite = m_ShieldBroken.sprite;
            }
            else if (GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer == 4)
            {
                m_ShieldLayerOneImage.transform.Rotate(100 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerTwoImage.transform.Rotate(70 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerThreeImage.transform.Rotate(80 * Vector3.forward * Time.deltaTime);
                m_ShieldLayerFourImage.transform.Rotate(75 * Vector3.forward * Time.deltaTime);

                m_ShieldLayerOneImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerTwoImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerThreeImage.sprite = m_ShieldPowered.sprite;
                m_ShieldLayerFourImage.sprite = m_ShieldPowered.sprite;
            }
            else
            {
                m_ShieldLayerOneImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerTwoImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerThreeImage.sprite = m_ShieldBroken.sprite;
                m_ShieldLayerFourImage.sprite = m_ShieldBroken.sprite;
            }
            #endregion
        }
        #endregion



    }
}