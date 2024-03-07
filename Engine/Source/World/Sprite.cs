using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class Sprite {

	public Rectangle? Section;
	public Texture2D Texture {
		get => texture;
		set {
			texture = value;
			Section = null;
		}
	}

	private Texture2D texture = null;

	public void Draw() {
		if(texture == null) return;
		Rectangle r = new Rectangle(0, 0, 16, 16);
		float rotation = 0.0F;
		Vector2 pivot = Vector2.Zero;
		SpriteEffects flip = SpriteEffects.None;
		float depth = 0.0F;
		Core.SpriteBatch.Draw(texture, r, Section, Color.White, rotation, pivot, flip, depth);
	}

}