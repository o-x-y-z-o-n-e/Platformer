using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class Sprite {

	public Rectangle Section;
	public Vector2 Pivot;
	public Texture2D Texture {
		get => texture;
		set {
			texture = value;
			Section = value == null ? default : new Rectangle(0, 0, texture.Width, texture.Height);
			Pivot = Vector2.Zero;
		}
	}

	public bool FlipX;
	public bool FlipY;
	public int Depth;

	private Texture2D texture = null;

	public void Draw() {
		if(texture == null) return;
		Rectangle r = new Rectangle(0, 0, Section.Width, Section.Height);
		float rotation = 0.0F;
		SpriteEffects flip = SpriteEffects.None;
		if(FlipX) flip |= SpriteEffects.FlipHorizontally;
		if(FlipY) flip |= SpriteEffects.FlipVertically;
		Core.SpriteBatch.Draw(texture, r, Section, Color.White, rotation, Pivot, flip, Depth);
	}

}