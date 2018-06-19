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
				"The speed of the game.\n\nWe've tried to tune the physics to " +
				"be as realistic as possible, and curling games can take a long " +
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
			)}
		};

		public enum ID
		{
			GAME_SPEED,
			THROW_COUNT,
			END_COUNT
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
