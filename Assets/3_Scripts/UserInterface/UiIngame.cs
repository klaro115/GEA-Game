using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class UiIngame : MonoBehaviour, IUiMouseHoverListener
	{
		#region Types

		[System.Serializable]
		public struct HUD
		{
			public RectTransform parent;
			public Text lives;
			public Text score;
			public Text wave;
			//...
		}

		#endregion
		#region Fields

		private IngameState state = IngameState.None;

		public HUD groupHUD = new HUD() { parent=null, lives=null, score=null, wave=null };
		public RectTransform groupMenu = null;
		public RectTransform groupGameOver = null;
		public RectTransform groupNextLevel = null;

		private AudioClip audioButtonHover = null;
		private AudioClip audioButtonPress = null;

		#endregion
		#region Methods

		public void initialize()
		{
			//...

			// Initialize UI elements and controls in children:
			UiButtonHoverDetector[] uiHovers = GetComponentsInChildren<UiButtonHoverDetector>(true);
			if (uiHovers != null)
			{
				foreach (UiButtonHoverDetector uiHover in uiHovers)
				{
					uiHover.setListener(this);
				}
			}

			// Load sound effects:
			audioButtonHover = Resources.Load<AudioClip>("menu-hover");
			audioButtonPress = Resources.Load<AudioClip>("menu-button");
		}

		public void shutdown()
		{
			//...
		}

		void Update()
		{
			if(state == IngameState.Ingame)
			{
				updateIngameHUD();
			}
		}

		public void setState(IngameState newState)
		{
			state = newState;

			// Activate the UI groups according to the current ingame state:
			groupHUD.parent.gameObject.SetActive(state == IngameState.Ingame);
			groupMenu.gameObject.SetActive(state == IngameState.Paused);
			groupGameOver.gameObject.SetActive(state == IngameState.GameOver);
			groupNextLevel.gameObject.SetActive(state == IngameState.NextLevel);
		}
		
		private void updateIngameHUD()
		{
			StatemachineStateIngame ingameState = StatemachineStateIngame.getStatemachine();
			Player player = ingameState.Player;

			groupHUD.wave.text = EnemySpawner.CurrentWave.ToString();
			groupHUD.lives.text = player.hitpoints.ToString();
			groupHUD.score.text = ingameState.Score.ToString();
		}

		private void restartGame()
		{
			// Tell the ingame statemachine to reset the game:
			StatemachineStateIngame ingameState = StatemachineStateIngame.getStatemachine();
			ingameState.restartGame();
		}

		public void notifyMouseEnter(PointerEventData eventData)
		{
			SoundHandler.playOneShot(audioButtonHover, 0.5f);
		}
		public void playMousePressEffects()
		{
			SoundHandler.playOneShot(audioButtonPress, 0.5f);
		}

		#endregion
		#region Methods UI Menu

		public void uiButtonMenuQuit()
		{
			playMousePressEffects();
			Statemachine.setState(Gamestate.MainMenu);
		}
		public void uiButtonMenuResume()
		{
			if (state == IngameState.Paused)
			{
				playMousePressEffects();
				StatemachineStateIngame ingameState = StatemachineStateIngame.getStatemachine();
				ingameState.setState(IngameState.Ingame);
			}
		}
		public void uiButtonMenuRestart()
		{
			playMousePressEffects();
			restartGame();
		}

		#endregion
		#region Methods UI GameOver

		public void uiButtonGameOverQuit()
		{
			playMousePressEffects();
			Statemachine.setState(Gamestate.MainMenu);
		}
		public void uiButtonGameOverRestart()
		{
			playMousePressEffects();
			restartGame();
		}

		#endregion
	}
}
