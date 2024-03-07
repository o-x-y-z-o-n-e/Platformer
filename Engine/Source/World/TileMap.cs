using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class TileMap {

	private int width;
	private int height;

	private int xOffset;
	private int yOffset;

	private int[] tiles;

	private VertexBuffer vertices;
	private IndexBuffer indices;

	public TileMap() {
		

	}

	private void Generate() {
		// TODO: Create tiles.

		int tileCount = GetTileCount();

		VertexPositionTexture[] vertices = new VertexPositionTexture[tileCount * 4];
		int[] indices = new int[tileCount * 4];

		int m;
		for(int i = 0; i < tileCount; i++) {
			m = GetNextTile(-1);

			vertices[(i + 0) * 4] = new VertexPositionTexture();
			vertices[(i + 1) * 4] = new VertexPositionTexture();
			vertices[(i + 2) * 4] = new VertexPositionTexture();
			vertices[(i + 3) * 4] = new VertexPositionTexture();
		}

		this.vertices = new VertexBuffer(Core.Graphics, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
		this.vertices.SetData(vertices);

		this.indices = new IndexBuffer(Core.Graphics, IndexElementSize.ThirtyTwoBits, )
	}

	public void Draw() {

		Core.Graphics.SetVertexBuffer()
		Core.Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, );

	}

	private int GetNextTile(int i) {
		while(true) {
			i++;
			if(i >= tiles.Length)
				return -1;

			if(tiles[i] > 0)
				return i;
		}
	}

	private int GetTileCount() {
		int n = 0;
		for(int i = 0; i < tiles.Length; i++) {
			if(tiles[i] > 0) n++;
		}
		return n;
	}
}