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
		Window.AllowUserResizing = true;
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
			context.Update(deltaTime);
        }
	}

    protected override void Draw(GameTime gameTime) {
		base.Draw(gameTime);

		if(frame != null) Graphics.SetRenderTarget(frame);

		Graphics.Clear(Color.Blue);

        if(context != null) {
			context.Draw();
        }

		if(frame != null) {
			DrawFrameToViewport();
		}
	}

	public virtual Type GetPlayerTpye() => typeof(Player);

	internal void AllocateFrame() {
		frame = new RenderTarget2D(GraphicsDevice, Camera.Width, Camera.Height);
	}

	private void DrawFrameToViewport() {
		Graphics.SetRenderTarget(null);
		Graphics.Clear(Color.Black);

		int w = Graphics.Viewport.Width;
		int h = Graphics.Viewport.Height;

		int bottom = 0;
		int top = h;
		int left = 0;
		int right = w;

		int v_reduction = 0;
		int h_reduction = 0;

		float h_ratio = w / (float)frame.Width;
		float v_ratio = w / (float)frame.Height;
		int h_scale = w / frame.Width;
		int v_scale = h / frame.Height;
		int min_scale = h_scale < v_scale ? h_scale : v_scale;

		int new_width = w;
		int new_height = h;

		bool preserve_aspect = true;
		bool pixel_perfect = true;

		if(pixel_perfect) {
			new_width = frame.Width * min_scale;
			new_height = frame.Height * min_scale;

			v_reduction = h - new_height;
			h_reduction = w - new_width;
		} else if(preserve_aspect) {
			new_height = (int)(frame.Height * h_ratio);
			new_width = (int)(frame.Width * v_ratio);

			if(h_ratio < v_ratio) {
				v_reduction = h - new_height;
			} else {
				h_reduction = w - new_width;
			}
		}

		bottom = v_reduction / 2;
		top = h - (v_reduction - bottom);
		left = h_reduction / 2;
		right = w - (h_reduction - left);

		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);
		spriteBatch.Draw(
			frame,
			new Rectangle(left, bottom, right - left, top - bottom),
			Color.White
		);
		spriteBatch.End();
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