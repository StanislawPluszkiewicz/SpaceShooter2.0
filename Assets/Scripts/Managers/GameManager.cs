﻿namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using SDD.Events;
	using System.Linq;

	public enum GameState { gameMenu, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory }

	public class GameManager : Manager<GameManager>
	{
		#region Player & Camera
		public Player m_PlayerPrefab;
		[HideInInspector] public Player m_Player;
		public Vector3 m_PlayerSpawnPosition;

		public CameraController m_CameraPrefab;
		[HideInInspector] public CameraController m_Camera;
		public Vector3 m_CameraSpawnPosition;
		#endregion


		#region Hierarchy
		[HideInInspector] public GameObject m_DynamicParent;
		[HideInInspector] public GameObject m_ProjectilesParent;
		[HideInInspector] public GameObject m_PlateformesParent;
		[HideInInspector] public GameObject m_OrbitalsParent;
		[HideInInspector] public GameObject m_VFXParent;
		#endregion

		#region Game State
		private GameState m_GameState;
		public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
		#endregion


		[Header("Props Damage")]
		public int m_UndestructiblePropsDamage;
		public int m_DestructiblePropsDamage;

		//LIVES
		#region Lives
		[Header("GameManager")]
		private int m_NStartLives;
		private int m_NLives;
		public int NLives { get { return m_NLives; } }
		void DecrementNLives(int decrement)
		{
			SetNLives(m_NLives - decrement);
		}

		void SetNLives(int nLives)
		{
			m_NLives = nLives;
			EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives});
		}
		#endregion

		#region Score
		private float m_Score;
		public float Score
		{
			get { return m_Score; }
			set
			{
				m_Score = value;
				BestScore = Mathf.Max(BestScore, value);
			}
		}

		public float BestScore
		{
			get { return PlayerPrefs.GetFloat("BEST_SCORE", 0); }
			set { PlayerPrefs.SetFloat("BEST_SCORE", value); }
		}

		void IncrementScore(float increment)
		{
			SetScore(m_Score + increment);
		}

		void SetScore(float score, bool raiseEvent = true)
		{
			Score = score;

			if (raiseEvent)
				EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives });
		}
		#endregion

		#region Time
		void SetTimeScale(float newTimeScale)
		{
			Time.timeScale = newTimeScale;
		}
		#endregion


		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
			
			//MainMenuManager
			EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

			//Score Item
			EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);
		}

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

			//MainMenuManager
			EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

			//Score Item
			EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);
		}
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			Menu();
			yield break;
		}

		private void InitDynamicsHierarchy()
		{
			if (m_DynamicParent != null)
				Destroy(m_DynamicParent);
			m_DynamicParent = new GameObject("Dynamics");

			createNewInHierarchy(ref m_ProjectilesParent, "Projectiles");
			createNewInHierarchy(ref m_PlateformesParent, "Plateformes");
			createNewInHierarchy(ref m_OrbitalsParent, "Orbitals");
			createNewInHierarchy(ref m_VFXParent, "VFXs");
		}

		private void createNewInHierarchy(ref GameObject obj, string name)
		{
			if (obj != null)
				Destroy(obj);
			obj = new GameObject(name);
			obj.transform.parent = m_DynamicParent.transform;
		}
		#endregion

		#region Game flow & Gameplay
		//Game initialization
		void InitNewGame(bool raiseStatsEvent = true)
		{
			SetScore(0);
            StartLevel(0);
		}

		void RestartGame(bool raiseStatsEvent = true)
		{
			SetScore(0);
			StartLevel(LevelManager.Instance.m_CurrentLevelIndex);
		}

		public void StartLevel(int id)
		{
			MenuManager.Instance.StartLevel(id);
			InitDynamicsHierarchy();
			Cursor.visible = false;
            Player.Init();
            CameraController.Init();
            LevelManager.Instance.StartLevel(id);
        }

		public void StartNextLevel()
		{
			LevelManager.Instance.m_CurrentLevelIndex++;
			if (LevelManager.Instance.m_CurrentLevelIndex == LevelManager.Instance.m_Levels.Count)
			{
				print("gg, you won.");
			}
			StartLevel(LevelManager.Instance.m_CurrentLevelIndex);
		}

		public void Lose()
		{
			Cursor.visible = true;
			LevelManager.Instance.m_CurrentLevelIndex = -1;
			Over();
		}

		public void EndLevel()
		{
			Cursor.visible = true;
			LevelManager.Instance.m_Levels[LevelManager.Instance.m_CurrentLevelIndex].isActive = false;
			Destroy(m_DynamicParent);
			ShopManager.Instance.ShowShop();
		}
        #endregion

        #region Callbacks to events issued by Score items
        private void ScoreHasBeenGained(ScoreItemEvent e)
		{
			if (IsPlaying)
				IncrementScore(e.eScore);
		}
		#endregion

		#region Callbacks to Events issued by MenuManager
		private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
		{
			Menu();
		}

		private void PlayButtonClicked(PlayButtonClickedEvent e)
		{
			Play();
		}

		private void ResumeButtonClicked(ResumeButtonClickedEvent e)
		{
			Resume();
		}

		private void EscapeButtonClicked(EscapeButtonClickedEvent e)
		{
			if (IsPlaying) Pause();
		}

		private void QuitButtonClicked(QuitButtonClickedEvent e)
		{
			Application.Quit();
		}
		#endregion

		#region GameState methods
		private void Menu()
		{
			SetTimeScale(1);
			m_GameState = GameState.gameMenu;
			if(MusicLoopsManager.Instance)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
			EventManager.Instance.Raise(new GameMenuEvent());
		}

		private void Play()
		{
            if (m_DynamicParent != null)
                DestroyImmediate(m_DynamicParent);

			InitNewGame();
			SetTimeScale(1);
			m_GameState = GameState.gamePlay;

			if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
			EventManager.Instance.Raise(new GamePlayEvent());
		}

		private void Pause()
		{
			Cursor.visible = true;
			if (!IsPlaying) return;

			SetTimeScale(0);
			m_GameState = GameState.gamePause;
			EventManager.Instance.Raise(new GamePauseEvent());
		}

		private void Resume()
		{
			Cursor.visible = false;
			if (IsPlaying) return;

			SetTimeScale(1);
			m_GameState = GameState.gamePlay;
			EventManager.Instance.Raise(new GameResumeEvent());
		}

		private void Over()
		{
			if (m_Player != null)
				Destroy(m_Player);
			if (m_Camera != null)
				Destroy(m_Camera);
			if (m_DynamicParent != null)
				Destroy(m_DynamicParent);
			SetTimeScale(0);
			m_GameState = GameState.gameOver;
			EventManager.Instance.Raise(new GameOverEvent());
			if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
		}
		#endregion
	}
}

