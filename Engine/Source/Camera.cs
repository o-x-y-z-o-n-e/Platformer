using Microsoft.Xna.Framework;

namespace Platformer;

public static class Camera {

	public static Vector2 Position = new Vector2(0, 0);
	public static int Width => width;
	public static int Height => height;

	private static int width;
	private static int height;

	public static void SetDimenions(int width, int height) {
		Camera.width = width;
		Camera.height = height;
		Core.Instance.AllocateFrame();
	}

	public static Matrix GetView() {
		return Matrix.CreateTranslation(-Position.X + width / 2, -Position.Y + height / 2, 0.0F);
	}

}