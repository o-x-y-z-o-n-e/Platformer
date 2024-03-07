using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer;
using Platformer.World;

public class Mario : Player {

	public Mario() {
		sprite.Texture = Assets.Load<Texture2D>("mario_and_luigi.png");
		sprite.Section = new Rectangle(0, 8, 16, 16);
	}

}