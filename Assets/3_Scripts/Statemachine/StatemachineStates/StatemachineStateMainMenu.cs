using UnityEngine;
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

			//...

			return true;
		}
		public override void shutdown ()
		{
			if(uiMainMenu != null)
			{
				MonoBehaviour.Destroy(uiMainMenu.gameObject);
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
