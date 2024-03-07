using System;
using System.IO;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.World;

namespace Platformer;

public class Core : Game {

	public static Core Instance => instance;
	public static SpriteBatch SpriteBatch => instance.spriteBatch;
	public static GraphicsDevice Graphics => instance.GraphicsDevice;

	private static Core instance;

	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private Context context;

	private RenderTarget2D frame;

	public static void Create<T>() where T : Core {
		if(instance != null) return;
		T ins = Activator.CreateInstance(typeof(T)) as T;
		ins.Run();
	}

	protected Core() {
		instance = this;
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Assets";
		IsMouseVisible = true;
	}

	protected override void Initialize() {
		spriteBatch = new SpriteBatch(GraphicsDevice);
		
		Camera.SetDimenions(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

		context = new Context();
	}

    protected override void Update(GameTime gameTime) {
		base.Update(gameTime);

        float deltaTime = gameTime.ElapsedGameTime.Seconds;

        if(context != null) {
			context.Player.Update(deltaTime);
        }
    }

    protected override void Draw(GameTime gameTime) {
		base.Draw(gameTime);

		GraphicsDevice.SetRenderTarget(frame);
		GraphicsDevice.Clear(Color.Black);

        if(context != null) {
			context.Player.Draw();
        }

		// TODO: Aspect Ratio
		GraphicsDevice.SetRenderTarget(null);
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);
		spriteBatch.Draw(
			frame,
			new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
			Color.White
		);
		spriteBatch.End();
	}

	public virtual Type GetPlayerTpye() => typeof(Player);

	internal void AllocateFrame() {
		frame = new RenderTarget2D(GraphicsDevice, Camera.Width, Camera.Height);
	}
}

public static class Assets {

	public static T Load<T>(string path) where T : class {
		path = Core.Instance.Content.RootDirectory + "/" + path;

		if(typeof(T) == typeof(Texture2D)) {
			try {
				Texture2D texture = null;
				using(FileStream fileStream = new FileStream(path, FileMode.Open)) {
					texture = Texture2D.FromStream(Core.Instance.GraphicsDevice, fileStream);
				}
				return texture as T;
			} catch {
				return null;
			}
		}

		return null;
	}

}

/*
Tilemap
	Read Data (Tiled Format)
	Construct Mesh

Physics

Player
	Input
	Movement
*/