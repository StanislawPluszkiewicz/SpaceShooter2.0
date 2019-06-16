
namespace ESIEE_UNITY_ETS
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using SDD.Events;

	public class MenuManager : Manager<MenuManager>
	{

		[Header("MenuManager")]

		#region Panels
		[Header("Panels")]
		[SerializeField] GameObject m_PanelMainMenu;
		[SerializeField] GameObject m_PanelInGameMenu;
		[SerializeField] GameObject m_PanelGameOver;
		[SerializeField] GameObject m_PanelChooseLevel;
        [SerializeField] GameObject m_PanelMainMenuMainPanel;
        [SerializeField] GameObject m_PanelConfirmQuit;
		[SerializeField] GameObject m_Hud;
        [SerializeField] GameObject m_PanelCredits;

        List<GameObject> m_AllPanels;

        private bool isOnMainMenu = false;

		#endregion

		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
		}

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();
		}
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Monobehaviour lifecycle
		protected override void Awake()
		{
			base.Awake();
			RegisterPanels();
		}

		private void Update()
		{
			if (Input.GetButtonDown("Cancel"))
			{
				EscapeButtonHasBeenClicked();
			}
		}
		#endregion

		#region Panel Methods
		void RegisterPanels()
		{
			m_AllPanels = new List<GameObject>();
			m_AllPanels.Add(m_PanelMainMenu);
			m_AllPanels.Add(m_PanelInGameMenu);
			m_AllPanels.Add(m_PanelGameOver);
			m_AllPanels.Add(m_PanelChooseLevel);
			m_AllPanels.Add(m_PanelConfirmQuit);
			m_AllPanels.Add(m_Hud);
            m_AllPanels.Add(m_PanelCredits);
		}

		void OpenPanel(GameObject panel)
		{
			foreach (var item in m_AllPanels)
				if (item) item.SetActive(item == panel);
		}
		#endregion

		#region UI OnClick Events
		public void EscapeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new EscapeButtonClickedEvent());
		}

		public void PlayButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new PlayButtonClickedEvent());
		}

		public void ResumeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new ResumeButtonClickedEvent());
		}

		public void MainMenuButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		}

		public void QuitButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new QuitButtonClickedEvent());
		}

		#endregion

		#region Callbacks to GameManager events
		protected override void GameMenu(GameMenuEvent e)
		{
			OpenPanel(m_PanelMainMenu);
		}

		protected override void GamePlay(GamePlayEvent e)
		{
			OpenPanel(m_Hud);
		}

		protected override void GamePause(GamePauseEvent e)
		{
			OpenPanel(m_PanelInGameMenu);
		}

		protected override void GameResume(GameResumeEvent e)
		{
			OpenPanel(m_Hud);
		}

		protected override void GameOver(GameOverEvent e)
		{
			OpenPanel(m_PanelGameOver);
		}
		#endregion

		#region Submenus
		public void DisplayChooseLevelMenu()
		{
            m_PanelMainMenuMainPanel.SetActive(false);
			m_PanelChooseLevel.SetActive(true);
		}

        public void HideChooseLevelMenu()
        {
            m_PanelMainMenuMainPanel.SetActive(true);
            m_PanelChooseLevel.SetActive(false);
        }

        public void DisplayConfirmQuitMenu()
        {
            if(m_PanelMainMenuMainPanel.activeSelf)
            {
                m_PanelMainMenuMainPanel.SetActive(false);
                isOnMainMenu = true;
            }
            m_PanelConfirmQuit.SetActive(true);
        }

        public void HideConfirmQuitMenu()
        {
            if (isOnMainMenu)
            {
                m_PanelMainMenuMainPanel.SetActive(true);
                isOnMainMenu = false;
            }
            m_PanelConfirmQuit.SetActive(false);
        }

        public void DisplayCreditsMenu()
        {
            m_PanelMainMenuMainPanel.SetActive(false);
            m_PanelCredits.SetActive(true);
        }

        public void HideCreditsMenu()
        {
            m_PanelMainMenuMainPanel.SetActive(true);
            m_PanelCredits.SetActive(false);
        }

        #endregion


    }

}
