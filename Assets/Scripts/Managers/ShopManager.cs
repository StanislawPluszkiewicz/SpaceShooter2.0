namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using SDD.Events;

    public class ShopManager : Manager<ShopManager>
    {
        #region Labels & Values
        public Text m_PriceUpgradeShieldText;
        private int m_PriceUpgradeShieldValue = 100;

        public Button m_UpgradeShieldButton;

        public Text m_PriceChangeTypeShieldText;
        private int m_PriceChangeTypeShieldValue = 100;

        public Text m_PriceRepairShipText;
        private int m_PriceRepairShipValue = 10;

        public Button m_RepairShipButton;

        public Image m_WeaponLeftImage;
        public Text m_WeaponLeftPrice;
        private int m_PriceWeaponLeft = 150;

        public Image m_WeaponCenterImage;
        public Text m_WeaponCenterPrice;
        private int m_PriceWeaponCenter = 200;

        public Image m_WeaponRightImage;
        public Text m_WeaponRightPrice;
        private int m_PriceWeaponRight = 250;
        #endregion

        protected override IEnumerator InitCoroutine()
        {
            yield return null;
        }

        #region Update Functions

        private void CheckIfButtonActif()
        {
            if(GameManager.Instance.m_Player.m_Settings.m_CurrentHealthPoints == GameManager.Instance.m_Player.m_Settings.m_MaxHealthPoints)
            {
                m_RepairShipButton.enabled = false;
            }
            else
            {
                m_RepairShipButton.enabled = true;
            }

            if(GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer == 4)
            {
                m_UpgradeShieldButton.enabled = false;
            }
            else
            {
                m_UpgradeShieldButton.enabled = true;
            }
        }


        public void ShowShop()
        {
            CheckIfButtonActif();

            m_PriceUpgradeShieldText.text = m_PriceUpgradeShieldValue.ToString();
            m_PriceChangeTypeShieldText.text = m_PriceChangeTypeShieldValue.ToString();
            m_PriceRepairShipText.text = m_PriceRepairShipValue.ToString();

            m_WeaponLeftPrice.text = m_WeaponLeftPrice.ToString();
            m_WeaponCenterPrice.text = m_WeaponCenterPrice.ToString();
            m_WeaponRightPrice.text = m_WeaponRightPrice.ToString();

			//EventManager.Instance.Raise(new GameShopEvent()); // c:
			MenuManager.Instance.GameShop(null); // :c
		}

        public void BuyChangeShieldType()
        {
            GameManager.Instance.m_Player.m_iMoney -= m_PriceChangeTypeShieldValue;
            if(GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_CurrentType == ShieldSettings.Type.Ion)
            {
                GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_CurrentType = ShieldSettings.Type.Laser;
            }
            else
            {
                GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_CurrentType = ShieldSettings.Type.Ion;
            }
        }

        public void BuyAddShield()
        {
            if(GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer < 4)
            {
                GameManager.Instance.m_Player.m_iMoney -= m_PriceUpgradeShieldValue;
                GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer += 1;
                GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iCurrentLayer = GameManager.Instance.m_Player.m_Settings.m_ShieldPrefab.m_Settings.m_iTotalLayer;
            }
                
        }

        public void BuyRepairShip()
        {
            GameManager.Instance.m_Player.m_iMoney -= m_PriceRepairShipValue;
            GameManager.Instance.m_Player.m_Settings.m_CurrentHealthPoints += 10;
        }
        #endregion
    }
}
