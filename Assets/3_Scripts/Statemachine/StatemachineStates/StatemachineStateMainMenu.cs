using UnityEngine;
using System.Collections;

namespace Game
{
	[System.Serializable]
	public class StatemachineStateMainMenu : StatemachineState
	{
		#region Fields

		private MainMenuState state = MainMenuState.None;

		//private UiMainMenuScreen uiMainMenu = null;
	
		//private static readonly string uiMainMenuPrefabName = "UiMainMenuScreen";

		#endregion
		#region Methods

		public override bool initialize()
		{
			// Find/Spawn and initialize main menu UI group:
			/*
			uiMainMenu = GameObject.FindObjectOfType<UiMainMenuScreen>();
			if(uiMainMenu == null)
			{
				UiMainMenuScreen uiMainPrefab = Resources.Load<UiMainMenuScreen>(uiMainMenuPrefabName);
				GameObject uiMainGO = Instantiate(uiMainPrefab.gameObject, uiCanvas) as GameObject;
				uiMainMenu = uiMainGO.GetComponent<UiMainMenuScreen>();
			}
			uiMainMenu.initialize();
			*/

			//...

			// Switch to main menu state right away:
			setState(MainMenuState.MainMenu);

			return true;
		}
		public override void shutdown ()
		{
			//...
		}

		public override Gamestate[] getAllowedStates ()
		{
			return allowedStatesMainMenu;
		}

		public bool setState(MainMenuState newState)
		{
			if(newState == state)
			{
				Debug.LogError("StateMachineMainMenu: Error! State '" + newState.ToString() + "' already active!");
				return false;
			}

			//...

			state = newState;

			// Update state in UI group as well:
			/*
			if(uiMainMenu != null)
			{
				uiMainMenu.setState(state);
			}
			*/

			return true;
		}

		#endregion
	}
}
