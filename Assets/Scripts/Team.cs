﻿using System;
using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts
{
    public struct Team
    {      
        public Color Color;
        public Sprite AimSprite;
        public Sprite ThrowSprite;
        public Sprite SweepSprite;
        public Sprite PantsSprite;

        public Team(Color color, Sprite aimSprite, Sprite throwSprite, Sprite sweepSprite, Sprite pantsSprite)
        {
            Color = color;
            AimSprite = aimSprite;
            ThrowSprite = throwSprite;
            SweepSprite = sweepSprite;
            PantsSprite = pantsSprite;
        }

		public Team(Color color, string teamName) {
			Color = color;
			string path = "Teams/" + teamName + "/";
			AimSprite = Resources.Load<Sprite>(path + "Aim");
			ThrowSprite = Resources.Load<Sprite>(path + "Throw");
			SweepSprite = Resources.Load<Sprite>(path + "Sweep");
			PantsSprite = Resources.Load<Sprite>(path + "Pants");
		}
    }
}
