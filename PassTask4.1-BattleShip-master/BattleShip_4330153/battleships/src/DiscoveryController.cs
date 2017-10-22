using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{

	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// GameController.Attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput ()
	{
		
		if (SwinGame.KeyTyped (KeyCode.vk_ESCAPE)) {
			GameController.AddNewState (GameState.ViewingGameMenu);
		}

		if (SwinGame.MouseClicked (MouseButton.LeftButton)) {

				DoAttack ();

		}
	}

	/// <summary>
	/// GameController.Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack ()
	{
		Point2D mouse = default (Point2D);

		mouse = SwinGame.MousePosition ();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32 (Math.Floor ((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32 (Math.Floor ((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack (row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery ()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 137;
		const int HITS_TOP = 174;
		const int SPLASH_TOP = 206;
		const int SHIPS_LEFT = 100;
		const int SHIPS_TOP = 261;

		//if ((SwinGame.KeyDown (KeyCode.vk_LSHIFT) | SwinGame.KeyDown (KeyCode.vk_RSHIFT)) & SwinGame.KeyDown (KeyCode.vk_c)) {
		//	UtilityFunctions.DrawField (GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		//} else {
		//	UtilityFunctions.DrawField (GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		//}

		UtilityFunctions.DrawSmallField (GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage ();

		UtilityFunctions.DrawDestroyField (GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);

		SwinGame.DrawText (GameController.HumanPlayer.Shots.ToString (), Color.White, GameResources.GameFont ("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText (GameController.HumanPlayer.Hits.ToString (), Color.White, GameResources.GameFont ("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText (GameController.HumanPlayer.Missed.ToString (), Color.White, GameResources.GameFont ("Menu"), SCORES_LEFT, SPLASH_TOP);

		SwinGame.DrawText (GameController.ComputerPlayer.Ship (ShipName.AircraftCarrier).Name, GameController.ComputerPlayer.Ship (ShipName.AircraftCarrier).IsDestroyed ? Color.Red : Color.White,
						  GameResources.GameFont ("Menu"), SHIPS_LEFT, SHIPS_TOP);
		SwinGame.DrawText (GameController.ComputerPlayer.Ship (ShipName.Battleship).Name, GameController.ComputerPlayer.Ship (ShipName.Battleship).IsDestroyed ? Color.Red : Color.White,
						  GameResources.GameFont ("Menu"), SHIPS_LEFT, SHIPS_TOP + 15);
		SwinGame.DrawText (GameController.ComputerPlayer.Ship (ShipName.Destroyer).Name, GameController.ComputerPlayer.Ship (ShipName.Destroyer).IsDestroyed ? Color.Red : Color.White,
						  GameResources.GameFont ("Menu"), SHIPS_LEFT, SHIPS_TOP + 30);
		SwinGame.DrawText (GameController.ComputerPlayer.Ship (ShipName.Submarine).Name, GameController.ComputerPlayer.Ship (ShipName.Submarine).IsDestroyed ? Color.Red : Color.White,
						  GameResources.GameFont ("Menu"), SHIPS_LEFT, SHIPS_TOP + 45);
		SwinGame.DrawText (GameController.ComputerPlayer.Ship (ShipName.Tug).Name, GameController.ComputerPlayer.Ship (ShipName.Tug).IsDestroyed ? Color.Red : Color.White,
						  GameResources.GameFont ("Menu"), SHIPS_LEFT, SHIPS_TOP + 60);

	}
}
