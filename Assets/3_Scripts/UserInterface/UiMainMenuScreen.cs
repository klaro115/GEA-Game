using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UiMainMenuScreen : MonoBehaviour
	{
		#region Types

		public enum MenuState
		{
			MainMenu,
			Credits,
			Settings,
			Quit,
			LoadingScreen,

			None
		}
		[System.Serializable]
		public struct UiLoadingScreen
		{
			public RectTransform parent;
			public Image uiProgressBar;
		}
		[System.Serializable]
		public struct UiCreditScreen
		{
			public RectTransform parent;
			public Text[] texts;
		}

		#endregion
		#region Fields

		private MenuState state = MenuState.None;

		public RectTransform groupMenu = null;
		public UiCreditScreen creditScreen = new UiCreditScreen() { parent=null, texts=null };
		public RectTransform groupSettings = null;
		public RectTransform groupQuit = null;
		public UiLoadingScreen loadingScreen = new UiLoadingScreen() { parent=null, uiProgressBar=null };
		//...

		private AudioClip audioButtonPress = null;
		private AudioClip audioNewGame = null;

		#endregion
		#region Methods

		public void initialize()
		{
			//...

			// Load sound effects:
			audioButtonPress = Resources.Load<AudioClip>("menu-button");
			audioNewGame = Resources.Load<AudioClip>("menu-new-game");

			// Set initial menu state:
			setMenuState(MenuState.MainMenu);
		}

		public void shutdown()
		{
			// Unload resources:
			if(audioButtonPress != null) Resources.UnloadAsset(audioButtonPress);
			if(audioNewGame != null) Resources.UnloadAsset(audioNewGame);

			//...
		}

		public void setMenuState(MenuState newState)
		{
			// Don't allow any more state changes after entering loading screen or while ending the game:
			if(state == MenuState.LoadingScreen || state == MenuState.Quit) return;

			// Set the new state active:
			state = newState;

			// Enable or disable UI elements according to new state:
			groupMenu.gameObject.SetActive(state == MenuState.MainMenu);
			creditScreen.parent.gameObject.SetActive(state == MenuState.Credits);
			groupSettings.gameObject.SetActive(state == MenuState.Settings);
			groupQuit.gameObject.SetActive(state == MenuState.Quit);
			loadingScreen.parent.gameObject.SetActive(state == MenuState.LoadingScreen);

			// Trigger some behaviours:
			switch (state)
			{
			case MenuState.LoadingScreen:
				StartCoroutine(loadGameCoroutine());
				break;
			case MenuState.Quit:
				StartCoroutine(quitGameCoroutine());
				break;
			case MenuState.Credits:
				StartCoroutine(rollCreditsCoroutine());
				break;
			default:
				break;
			}
		}

		#endregion
		#region Methods MainMenu

		public void uiButtonNewGame()
		{
			SoundHandler.playOneShot(audioNewGame);
			setMenuState(MenuState.LoadingScreen);
		}
		public void uiButtonCredits()
		{
			SoundHandler.playOneShot(audioButtonPress);
			setMenuState(MenuState.Credits);
		}
		public void uiButtonQuit()
		{
			SoundHandler.playOneShot(audioButtonPress);
			setMenuState(MenuState.Quit);
		}

		#endregion
		#region Methods Settings

		//TODO

		#endregion
		#region Methods Credits

		public void uiButtonBackToMain()
		{
			SoundHandler.playOneShot(audioButtonPress);
			setMenuState(MenuState.MainMenu);
		}

		private IEnumerator rollCreditsCoroutine()
		{
			if(creditScreen.texts == null || creditScreen.texts.Length == 0) yield break;

			foreach(Text txt in creditScreen.texts)
			{
				Color cTxt = txt.color;
				cTxt.a = 0.0f;
				txt.color = cTxt;
			}

			for(int i = 0; i < creditScreen.texts.Length * 2; ++i)
			{
				int txtIndex = i / 2;
				bool growing = i % 2 == 0;

				Text txt = creditScreen.texts[txtIndex];
				Color c = txt.color;

				if(growing)
				{
					txt.gameObject.SetActive(true);
					while(c.a < 1)
					{
						c.a += 0.4f * Time.deltaTime;
						txt.color = c;
						yield return null;
					}
				}
				else
				{
					while(c.a > 0)
					{
						c.a -= 0.4f * Time.deltaTime;
						txt.color = c;
						yield return null;
					}
					txt.gameObject.SetActive(false);
				}
				yield return new WaitForSeconds(0.2f);
			}

			// Return to main menu screen:
			setMenuState(MenuState.MainMenu);
		}

		#endregion
		#region Methods Quit

		private IEnumerator quitGameCoroutine()
		{
			// Wait a few seconds before actually shutting down the application:
			yield return new WaitForSeconds(2.0f);

			// Terminate the program:
			Statemachine.setState(Gamestate.Quit);
		}

		#endregion
		#region Methods LoadingScreen

		private IEnumerator loadGameCoroutine()
		{
			// Wait for next frame:
			yield return null;

			//...

			// Wait for a minimum of 0.5 seconds before starting game:
			yield return new WaitForSeconds(0.5f);

			// Start up the ingame state:
			Statemachine.setState(Gamestate.Ingame);
		}

		#endregion
	}
}
