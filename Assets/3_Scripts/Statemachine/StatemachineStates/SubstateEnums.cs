using UnityEngine;
using System.Collections;

namespace Game
{
	/// <summary>
	/// Ingame statemachine states.
	/// </summary>
	public enum IngameState
	{
		None,

		Ingame,			// Player is actually playing the game.
		Paused,			// Ingame menu.
		NextLevel,		// Passage between levels, upgrades, score, etc.
		GameOver,		// Player has lost the game.
	}

	/// <summary>
	/// Main menu statemachine states.
	/// </summary>
	public enum MainMenuState
	{
		None,

		MainMenu,		// Main menu screen, allowing navigation to all other states.
		Credits,		// Credits screen.
		Settings		// Settings and options menu.
	}
}
