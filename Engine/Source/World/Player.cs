using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class Player : Entity {

	protected Sprite sprite;

    public Player() {
		sprite = new Sprite();
	}

	public override void Draw() {
		sprite.Draw();
	}

}
