using System;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts
{
	public class Utils
	{
		public static bool GetKey_Down()
		{
			return Input.GetKey("down") || Input.GetKey("s") || Input.GetKey("k");
		}

		public static bool GetKey_Up()
		{
			return Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("i");
		}

		public static bool GetKey_Left()
		{
			return Input.GetKey("left") || Input.GetKey("a") || Input.GetKey("j");
		}

		public static bool GetKey_Right()
		{
			return Input.GetKey("right") || Input.GetKey("d") || Input.GetKey("l");
		}

		public static bool GetKey_Confirm()
		{
			return Input.GetKey("space") || Input.GetKey("enter");
		}

		public static bool GetKeyDown_Confirm()
		{
			return Input.GetKeyDown("space") || Input.GetKeyDown("enter");
		}

		public static bool GetKeyUp_Confirm()
		{
			return Input.GetKeyUp("space") || Input.GetKeyUp("enter");
		}

		public static bool GetKeyDown_Escape()
		{
			return Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Pause);
		}
	}
}
