using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The menu controller handles the drawing and user interactions
/// from the menus in the game. These include the main menu, game
/// menu and the settings m,enu.
/// </summary>
static class MenuController
{

	/// <summary>
	/// The menu structure for the game.
	/// </summary>
	/// <remarks>
	/// These are the text captions for the menu items.
	/// </remarks>
	private static readonly string [] [] _menuStructure = {
		new string[] {
			"PLAY",
			/*Add Music*/
			"MUSIC",
			"SETUP",
			"THEME",
			"SCORES",
			"QUIT"
		},
		new string[] {
			"RETURN",
			"SURRENDER",
			"QUIT"
		},
		new string[] {
			"EASY",
			"MEDIUM",
			"HARD",

		},
		/*Add Sub Menu in Music*/
		new string[]{
			"MUTE",
			"MUSIC1",
			"MUSIC2"
		},
		/* Add sub menu for Theme*/
		new string []{
			"PINK",
			"GREEN",
			"BLUE"
		}

	};
	private const int MENU_TOP = 575;
	private const int MENU_LEFT = 30;
	private const int MENU_GAP = 0;
	private const int BUTTON_WIDTH = 75;
	private const int BUTTON_HEIGHT = 15;
	private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;

	private const int TEXT_OFFSET = 0;
	private const int MAIN_MENU = 0;
	private const int GAME_MENU = 1;
	private const int SETUP_MENU = 2;
	/*Added Music Menu*/
	private const int MUSIC_MENU = 3;
	/*Theme Menu*/
	private const int THEME_MENU = 4;

	private const int MAIN_MENU_PLAY_BUTTON = 0;
	/*Added Music Menu Button*/
	private const int MAIN_MENU_MUSIC_BUTTON = 1;
	private const int MAIN_MENU_SETUP_BUTTON = 2;
	/*Theme button*/
	private const int MAIN_MENU_THEME_BUTTON = 3;
	private const int MAIN_MENU_TOP_SCORES_BUTTON = 4;
	private const int MAIN_MENU_QUIT_BUTTON = 5;

	/*Added Sub Menu Button under Music*/
	private const int MUSIC_MENU_MUTE_BUTTON = 0;
	private const int MUSIC_MENU_MUSIC1_BUTTON = 1;
	private const int MUSIC_MENU_MUSIC2_BUTTON = 2;
	private const int MUSIC_MENU_EXIT_BUTTON = 3;

	private const int SETUP_MENU_EASY_BUTTON = 0;
	private const int SETUP_MENU_MEDIUM_BUTTON = 1;
	private const int SETUP_MENU_HARD_BUTTON = 2;
	private const int SETUP_MENU_TRAIN_BUTTON = 3;
	private const int SETUP_MENU_EXIT_BUTTON = 4;

	/*Theme colours button*/
	private const int THEME_MENU_PINK_BUTTON = 0;
	private const int THEME_MENU_GREEN_BUTTON = 1;
	private const int THEME_MENU_BLUE_BUTTON = 2;

	/*default theme*/
	private static ThemeColor clr = ThemeColor.Blue;

	private const int GAME_MENU_RETURN_BUTTON = 0;
	private const int GAME_MENU_SURRENDER_BUTTON = 1;

	private const int GAME_MENU_QUIT_BUTTON = 2;
	private static readonly Color MENU_COLOR = SwinGame.RGBAColor (2, 167, 252, 255);

	private static readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor (1, 57, 86, 255);

	/*True False boolean for mute music*/
	private static bool muted = true;
	/*GETTER SETTER for bool muted*/
	public static bool Set_muted {
		get { return muted; }
		set { muted = value; }
	}

	public static bool Get_Set_muted {
		get { return muted; }	}

	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleMainMenuInput ()
	{
		HandleMenuInput (MAIN_MENU, 0, 0);
	}

	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleSetupMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (SETUP_MENU, 1, 1);

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);
		}
	}

	/*Added Music Menu Handler*/
	public static void HandleMusicMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (MUSIC_MENU, 1, 1);

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);

		}	}
	/*Theme handler*/
	public static void HandleThemeMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (THEME_MENU, 1, 1);

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);
		}	}
	/// <summary>
	/// Handle input in the game menu.
	/// </summary>
	/// <remarks>
	/// Player can return to the game, surrender, or quit entirely
	/// </remarks>
	public static void HandleGameMenuInput ()
	{
		HandleMenuInput (GAME_MENU, 0, 0);
	}



	/// <summary>
	/// Handles input for the specified menu.
	/// </summary>
	/// <param name="menu">the identifier of the menu being processed</param>
	/// <param name="level">the vertical level of the menu</param>
	/// <param name="xOffset">the xoffset of the menu</param>
	/// <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
	private static bool HandleMenuInput (int menu, int level, int xOffset)
	{
		if (SwinGame.KeyTyped (KeyCode.vk_ESCAPE)) {
			GameController.EndCurrentState ();
			return true;
		}

		if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
			Audio.PlaySoundEffect (GameResources.GameSound ("Click"));
			int i = 0;
			for (i = 0; i <= _menuStructure [menu].Length - 1; i++) {
				//IsMouseOver the i'th button of the menu
				if (IsMouseOverMenu (i, level, xOffset)) {
					PerformMenuAction (menu, i);
					return true;
				}
			}

			if (level > 0) {
				//none clicked - so end this sub menu
				GameController.EndCurrentState ();
			}
		}

		/*Added HotKey to Pause Music*/
		if (SwinGame.KeyTyped (KeyCode.vk_F1)) {
			muted = true;
			if (muted == true) {
				SwinGame.StopMusic ();
			};
		}
		return false;
	}

	/// <summary>
	/// Draws the main menu to the screen.
	/// </summary>
	public static void DrawMainMenu ()
	{
		DrawButtons (MAIN_MENU);
	}

	/// <summary>
	/// Draws the Game menu to the screen
	/// </summary>
	public static void DrawGameMenu ()
	{

		DrawButtons (GAME_MENU);
	}

	/// <summary>
	/// Draws the settings menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu
	/// </remarks>
	public static void DrawSettings ()
	{
		
		DrawButtons (MAIN_MENU);
		DrawButtons (SETUP_MENU, 1, 1);
	}

	/*Draw Sub Menu*/
	public static void MusicSettings ()
	{
		DrawButtons (MAIN_MENU);
		DrawButtons (MUSIC_MENU, 1, 1);	}

	/*Theme draw*/
	public static void ThemeSettings ()
	{
		DrawButtons (MAIN_MENU);
		DrawButtons (THEME_MENU, 1, 1);	}
	/// <summary>
	/// Draw the buttons associated with a top level menu.
	/// </summary>
	/// <param name="menu">the index of the menu to draw</param>
	private static void DrawButtons (int menu)
	{
		DrawButtons (menu, 0, 0);
	}

	/// <summary>
	/// Draws the menu at the indicated level.
	/// </summary>
	/// <param name="menu">the menu to draw</param>
	/// <param name="level">the level (height) of the menu</param>
	/// <param name="xOffset">the offset of the menu</param>
	/// <remarks>
	/// The menu text comes from the _menuStructure field. The level indicates the height
	/// of the menu, to enable sub menus. The xOffset repositions the menu horizontally
	/// to allow the submenus to be positioned correctly.
	/// </remarks>
	private static void DrawButtons (int menu, int level, int xOffset)
	{
		int btnTop = 0;

		btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int i = 0;
		for (i = 0; i <= _menuStructure [menu].Length - 1; i++) {
			int btnLeft = 0;
			btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
			SwinGame.DrawTextLines (_menuStructure [menu] [i], MENU_COLOR, Color.Black, GameResources.GameFont ("Menu"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);

			if (SwinGame.MouseDown (MouseButton.LeftButton) & IsMouseOverMenu (i, level, xOffset)) {
				SwinGame.DrawRectangle (HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}

	/// <summary>
	/// Determined if the mouse is over one of the button in the main menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <returns>true if the mouse is over that button</returns>
	private static bool IsMouseOverButton (int button)
	{
		return IsMouseOverMenu (button, 0, 0);
	}

	/// <summary>
	/// Checks if the mouse is over one of the buttons in a menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <param name="level">the level of the menu</param>
	/// <param name="xOffset">the xOffset of the menu</param>
	/// <returns>true if the mouse is over the button</returns>
	private static bool IsMouseOverMenu (int button, int level, int xOffset)
	{
		int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);

		return UtilityFunctions.IsMouseInRectangle (btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
	}

	/// <summary>
	/// A button has been clicked, perform the associated action.
	/// </summary>
	/// <param name="menu">the menu that has been clicked</param>
	/// <param name="button">the index of the button that was clicked</param>
	private static void PerformMenuAction (int menu, int button)
	{
		switch (menu) {
		case MAIN_MENU:
			PerformMainMenuAction (button);
			break;
		case SETUP_MENU:
			PerformSetupMenuAction (button);
			break;
		case GAME_MENU:
			PerformGameMenuAction (button);
			break;
			/*Call Theme performance action*/
		case THEME_MENU:
            PerformThemeMenuAction (button);
			break;
			/*Added PerformMusicMenuAction*/
		case MUSIC_MENU:
            PerformMusicMenuAction (button);
			break;

		}
	}

	/// <summary>
	/// The main menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformMainMenuAction (int button)
	{
		switch (button) {
		case MAIN_MENU_PLAY_BUTTON:
			GameController.StartGame ();
			break;
		case MAIN_MENU_SETUP_BUTTON:
			GameController.AddNewState (GameState.AlteringSettings);
			break;
		case MAIN_MENU_TOP_SCORES_BUTTON:
			GameController.AddNewState (GameState.ViewingHighScores);
			break;
			/*Add Themecolor*/
		case MAIN_MENU_THEME_BUTTON :
			GameController.AddNewState (GameState.ThemeColors);
			break;
		case MAIN_MENU_QUIT_BUTTON:
			GameController.EndCurrentState ();
			break;
			/*Added NewState for MusicSetting*/
		case MAIN_MENU_MUSIC_BUTTON:
			GameController.AddNewState (GameState.MusicSettings);
			break;
		};
	}

	/// <summary>
	/// The setup menu was clicked, perform the button's actiCHAon.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformSetupMenuAction (int button)
	{
		switch (button) {
		case SETUP_MENU_EASY_BUTTON:
			GameController.SetDifficulty (AIOption.Easy);
			break;
		case SETUP_MENU_MEDIUM_BUTTON:
			GameController.SetDifficulty (AIOption.Medium);
			break;
		case SETUP_MENU_HARD_BUTTON:
			GameController.SetDifficulty (AIOption.Hard);
			break;
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState ();
	}


	/* Add method for PerformThemeMenuAction*/
	public static void PerformThemeMenuAction (int button)
	{
		switch (button) {
		case THEME_MENU_PINK_BUTTON:
			GameController.DrawScreen (ThemeColor.Pink);
			clr = ThemeColor.Pink;
			break;
		case THEME_MENU_GREEN_BUTTON:
			GameController.DrawScreen (ThemeColor.Green);
			clr = ThemeColor.Green;
			break;
		case THEME_MENU_BLUE_BUTTON:
			GameController.DrawScreen (ThemeColor.Blue);
			clr = ThemeColor.Blue;

			break;
		}

	}


	/*Add Method PerformMusicMenuAction*/
	public static void PerformMusicMenuAction (int button)
	{
		switch (button) {
		case MUSIC_MENU_MUSIC1_BUTTON:
			Audio.PlayMusic (GameResources.GameMusic ("Background"));
			break;

		case MUSIC_MENU_MUSIC2_BUTTON:
			Audio.PlayMusic (GameResources.GameMusic ("Background1"));
			break;

		case MUSIC_MENU_MUTE_BUTTON:
			if (muted == true) {
				SwinGame.StopMusic ();
				muted = false;
				break;
			} else {
				Audio.PlayMusic (GameResources.GameMusic ("Background"));
				muted = true;
				break;
			}
		}

		GameController.EndCurrentState ();	}

	/// <summary>
	/// The game menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformGameMenuAction (int button)
	{
		switch (button) {
		case GAME_MENU_RETURN_BUTTON:
			GameController.EndCurrentState ();
			break;
		case GAME_MENU_SURRENDER_BUTTON:
			GameController.EndCurrentState ();
			//end game menu
			GameController.EndCurrentState ();
			//end game
			break;
		case GAME_MENU_QUIT_BUTTON:
			GameController.AddNewState (GameState.Quitting);
			break;
		}
	}

	/*Return current theme color*/
	public static ThemeColor TColor {
		get { return clr; }	}
}
