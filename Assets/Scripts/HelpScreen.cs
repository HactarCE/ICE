using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts
{
	public struct HelpScreen
	{
		public static readonly Dictionary<ID, HelpScreen> HelpScreens = new Dictionary<ID, HelpScreen>
		{
			{ID.GAME_SPEED, new HelpScreen(
				"Game speed",
				"The speed of the game.\n\nWe've tried to tune the game to feel" +
				"as realistic as possible, and curling games can take a long " +
				"time. This option remedies that by making all physics in the " +
				"game run at 1.5× or 2× the normal speed. Faster game speeds " +
				"also demand better reaction time."
			)},
			{ID.THROW_COUNT, new HelpScreen(
				"Throws",
				"The total number of throws per end. Teams alternate after each " +
				"throw.\n\nIn real curling, there are 16 throws in an end (8 " +
				"per team)."
			)},
			{ID.END_COUNT, new HelpScreen(
				"Ends",
				"The number of ends per game.\n\nAt the conclusion of each end, " +
				"points are tallied and all rocks are removed from play. The " +
				"team with the closest rock to the center of the house (the " +
				"target at the far end of the ice) is the first to throw in the " +
				"next end.\n\nIn real curling, there are 8 or 10 ends."
			)},
			{ID.SCORING, new HelpScreen(
				"Objective",
				"Welcome to the Incredible Curling Extravaganza!\n\nThe goal in " +
				"curling is to score the most points by sliding large (~40 kg) " +
				"granite rocks towards a target, called the house. The game is " +
				"split into ends, during which teams take turns \"throwing\" " +
				"their rocks. At the conclusion of an end, teams are given a " +
				"point for each of their rocks closer to the center of the " +
				"house than their opponent's closest."
			)},
			{ID.INTERFACE, new HelpScreen(
				"Interface",
				"At the top of the screen is a map of the whole ice sheet with " +
				"indicators for each rock. Each throw starts on the left and " +
				"moves towards the target on the right, called the house.\n\nOn " +
				"the bottom of the screen is a bar with lots of information.",
				Resources.Load<Sprite>("Help/BottomBar")
			)},
			{ID.AIMING_AND_THROWING, new HelpScreen(
				"Aiming and throwing",
				"The blue dotted line shows your current aim. Adjust it by " +
				"holding the up or down arrow keys, or W/S.\n\nTap the " +
				"spacebar to start \"throwing\" (sliding) the rock."
			)},
			{ID.SPIN, new HelpScreen(
				"Spin",
				"Now that your rock is in motion, it's time to put the \"curl\" " +
				"into \"curling\"! Hold the up or down arrow keys or W/S to add " +
				"spin to your rock, which will cause it to gradually curve.\n\n" +
				"The thrower will automatically release the rock at the first " +
				"red \"hogline,\" but you can tap the spacebar to release the " +
				"rock early."
			)},
			{ID.SWEEPING, new HelpScreen(
				"Sweeping",
				"You've released your rock, and you're ready to sweep! Hold the " +
				"spacebar to sweep the ice in front of the rock, reducing " +
				"friction on the ice and allowing the rock to travel farther. " +
				"Hold the up or down arrow keys or W/S to move the sweeper up " +
				"and down; sweeping above or below the rock will slightly " +
				"redirect the rock.\n\nSweeping for roughly half of the rock's " +
				"travel will get it pretty close to the house. If a rock goes " +
				"out of bounds or stops moving before the second red " +
				"\"hogline\" then it is removed from play."
			)},
			{ID.FINAL, new HelpScreen(
				"Final",
				"Hopefully your rock landed near the middle of the house and " +
				"earned you a point! If it did, though, that point might not " +
				"stand for long—your opponent can throw their rock into yours, " +
				"knocking it away."
			)}
		};

		public enum ID
		{
			GAME_SPEED,
			THROW_COUNT,
			END_COUNT,
			SCORING,
			INTERFACE,
			AIMING_AND_THROWING,
			SPIN,
			SWEEPING,
			FINAL,
			NONE
		}

		public readonly string Title;
		public readonly string Text;
		public readonly Sprite Image;

		public HelpScreen(string title, string text)
		{
			Title = title;
			Text = text;
			Image = null;
		}

		public HelpScreen(string title, string text, Sprite image)
		{
			Title = title;
			Text = text;
			Image = image;
		}
	}
}

