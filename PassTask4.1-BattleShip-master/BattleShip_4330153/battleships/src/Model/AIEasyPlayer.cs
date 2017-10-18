using System;
using SwinGameSDK;

/// <summary>
/// AIEasyPlayer is a type of AIPlayer where it will fire randomly at grid position
/// </summary>
public class AIEasyPlayer : AIPlayer
{

	/// <summary>
	/// The default contructor for AIEasyPlayer
	/// </summary>
	/// <param name="game">Game.</param>
	public AIEasyPlayer (BattleShipsGame game) : base (game)
	{
	}

	/// <summary>
	/// GenerateCoordinates should generate random shooting coordinates
	/// </summary>
	/// <param name="row">the generated row</param>
	/// <param name="column">the generated column</param>
	protected override void GenerateCoords (ref int row, ref int column)
	{
		do {
			SearchCoords (ref row, ref column);

		} while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid [row, column] != TileView.Sea));
		//while inside the grid and not a sea tile do the search
	}

	/// <summary>
	/// SearchCoords will randomly generate shots within the grid as long as its not hit that tile already
	/// </summary>
	/// <param name="row">the generated row</param>
	/// <param name="column">the generated column</param>
	private void SearchCoords (ref int row, ref int column)
	{
		row = _Random.Next (0, EnemyGrid.Height);
		column = _Random.Next (0, EnemyGrid.Width);
	}

	/// <summary>
	/// Processes the shot.
	/// </summary>
	/// <param name="row">Row.</param>
	/// <param name="col">Col.</param>
	/// <param name="result">Result.</param>
	protected override void ProcessShot (int row, int col, AttackResult result)
	{
		if (result.Value == ResultOfAttack.Hit) {
			return;
		} else if (result.Value == ResultOfAttack.ShotAlready) {
			throw new ApplicationException ("Error in AI");
		}
	}

}