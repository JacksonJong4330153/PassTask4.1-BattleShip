using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// We do not actually need this library
/// </summary>
//using System.Data;
/// <summary>
/// The different AI levels.
/// </summary>
public enum AIOption
{
	/// <summary>
	/// Easy, total random shooting
	/// </summary>
	Easy,

	/// <summary>
	/// Medium, marks squares around hits
	/// </summary>
	Medium,

	/// <summary>
	/// As medium, but removes shots once it misses
	/// </summary>
	Hard
}
