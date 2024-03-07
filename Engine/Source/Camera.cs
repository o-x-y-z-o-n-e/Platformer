using Microsoft.Xna.Framework;

namespace Platformer;

public static class Camera {

	public static Vector2 Position;
	public static int Width => width;
	public static int Height => height;

	private static int width;
	private static int height;

	public static void SetDimenions(int width, int height) {
		Camera.width = width;
		Camera.height = height;
		Core.Instance.AllocateFrame();
	}

}