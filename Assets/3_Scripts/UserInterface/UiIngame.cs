﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class UiIngame : MonoBehaviour
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
		//...

		#endregion
		#region Methods

		public void initialize()
		{
			//...
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

		#endregion
	}
}
