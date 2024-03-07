using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class Player : Entity {

	protected Sprite sprite;

    public Player() {
		sprite = new Sprite();

		transform = Matrix.CreateTranslation(0, 0, 0);
	}

	public override void Draw() {
		Core.SpriteBatch.Begin(
			SpriteSortMode.Deferred,
			BlendState.AlphaBlend,
			SamplerState.PointClamp,
			null,
			null,
			null,
			Camera.GetView() * transform
		);
        sprite.Draw();
		Core.SpriteBatch.End();
	}

}
