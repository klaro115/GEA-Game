using UnityEngine;
using System.Collections;

namespace Game
{
	public enum Gamestate
	{
		None,			// Default blank state. Only active briefly while launching the statemachine.

		Startup,		// When the program is starting up + splash screens.
		Quit,			// When the program is in the process of shutting down.

		MainMenu,		// Main menu screen, credits, settings, etc.
		Ingame,			// Actual game levels + ingame menu.
	}
}
