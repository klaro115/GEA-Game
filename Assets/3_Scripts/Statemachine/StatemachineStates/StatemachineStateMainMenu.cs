﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

using Game.UI;

namespace Game
{
	[System.Serializable]
	public class StatemachineStateMainMenu : StatemachineState
	{
		#region Fields

		private UiMainMenuScreen uiMainMenu = null;
	
		private static readonly string uiMainMenuPrefabName = "UiMainMenuScreen";

		#endregion
		#region Methods

		public override bool initialize()
		{
			// Reset event system UI selections:
			EventSystem.current.SetSelectedGameObject(null);

			// Find/Spawn and initialize main menu UI group:
			RectTransform uiCanvas = GameObject.FindObjectOfType<Canvas>().transform as RectTransform;
			uiMainMenu = GameObject.FindObjectOfType<UiMainMenuScreen>();
			if(uiMainMenu == null)
			{
				UiMainMenuScreen uiMainPrefab = Resources.Load<UiMainMenuScreen>(uiMainMenuPrefabName);
				Debug.Log(uiMainPrefab.name);
				GameObject uiMainGO = MonoBehaviour.Instantiate(uiMainPrefab.gameObject, uiCanvas) as GameObject;
				uiMainMenu = uiMainGO.GetComponent<UiMainMenuScreen>();
			}
			uiMainMenu.initialize();

      SoundHandler.playBackgroundMusic(Resources.Load<AudioClip>("music-menu"));

			//...

			return true;
		}
		public override void shutdown ()
		{
			// Reset event system UI selections:
			EventSystem.current.SetSelectedGameObject(null);

			if(uiMainMenu != null)
			{
				MonoBehaviour.Destroy(uiMainMenu.gameObject);
        SoundHandler.stop();
        uiMainMenu = null;
			}
		}

		public override Gamestate[] getAllowedStates ()
		{
			return allowedStatesMainMenu;
		}

		#endregion
	}
}
