using System.IO;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.World;

public class TileMap {

	private int width = 4;
	private int height = 4;

	private int xScale = 16;
    private int yScale = 16;

    private int xOffset = 0;
	private int yOffset = 0;

	private Tile[] tiles;
	private List<RenderBatch> renderBatches;

	private TileMap() {
        renderBatches = new List<RenderBatch>();
	}

	public static TileMap Create(string path) {
		TileMap map = new TileMap();

		FileStream stream = null;
		XmlReader reader = XmlReader.Create(stream);

		map.Generate();

		return map;
	}

	private void Generate() {
		int m = -1;
        while((m = GetNextTile(m)) >= 0) {
			Tile tile = tiles[m];

			bool found = false;
			foreach(RenderBatch batch in renderBatches) {
				if(batch.Material.Texture == tile.Texture) {
					batch.Tiles.Add(m);
					found = true;
					break;
				}
			}

			if(!found) {
				renderBatches.Add(new RenderBatch(this, tile.Texture));
				renderBatches[renderBatches.Count - 1].Tiles.Add(m);
			}
		}

        foreach(RenderBatch batch in renderBatches) {
			batch.Apply();
        }
    }

	public void Draw() {
        foreach(RenderBatch batch in renderBatches) {
            foreach(EffectPass pass in batch.Material.CurrentTechnique.Passes) {
                pass.Apply();

                Core.Graphics.Indices = batch.Indices;
                Core.Graphics.SetVertexBuffer(batch.Vertices);
                Core.Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, batch.Vertices.VertexCount);
            }
        }
	}

	private int GetNextTile(int i) {
		while(true) {
			i++;
			if(i >= tiles.Length)
				return -1;

			if(tiles[i] != null)
				return i;
		}
	}

	public struct RenderBatch {

		public readonly TileMap Map;

        public List<int> Tiles;
        public BasicEffect Material;

        public VertexBuffer Vertices {
			get;
			private set;
		}

		public IndexBuffer Indices {
			get;
			private set;
		}

        public RenderBatch(TileMap map, Texture2D texture) {
			Map = map;
            Tiles = new List<int>();
            Material = new BasicEffect(Core.Graphics);
            Material.LightingEnabled = false;
            Material.TextureEnabled = true;
			Material.Texture = texture;
		}

		public void Apply() {
            VertexPositionTexture[] vertices = new VertexPositionTexture[Tiles.Count * 4];
            int[] indices = new int[Tiles.Count * 6];
            Vector3 s = new Vector3(Map.xScale, Map.yScale, 0);
            for(int i = 0; i < Tiles.Count; i++) {
				int m = Tiles[i];
				Tile tile = Map.tiles[m];
                int x = m % Map.width;
                int y = m / Map.height;

                x += Map.xOffset;
                y += Map.yOffset;

                int i_bl = (i + 0) * 4;
                int i_br = (i + 1) * 4;
                int i_tr = (i + 2) * 4;
                int i_tl = (i + 3) * 4;


                Vector3 v_bl = new Vector3(x, y, 0) * s;
                Vector3 v_br = new Vector3(x + 1, y, 0) * s;
                Vector3 v_tr = new Vector3(x + 1, y + 1, 0) * s;
                Vector3 v_tl = new Vector3(x, y + 1, 0) * s;

                vertices[i_bl] = new VertexPositionTexture(v_bl, tile.UV[0]);
                vertices[i_br] = new VertexPositionTexture(v_br, tile.UV[1]);
                vertices[i_tr] = new VertexPositionTexture(v_tr, tile.UV[2]);
                vertices[i_tl] = new VertexPositionTexture(v_tl, tile.UV[3]);

                indices[(i + 0) * 6] = i_bl;
                indices[(i + 1) * 6] = i_br;
                indices[(i + 2) * 6] = i_tl;
                indices[(i + 3) * 6] = i_tr;
                indices[(i + 4) * 6] = i_br;
                indices[(i + 5) * 6] = i_tl;
            }

            Vertices = new VertexBuffer(Core.Graphics, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
            Vertices.SetData(vertices);

            Indices = new IndexBuffer(Core.Graphics, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
            Indices.SetData(indices);
        }
	}
}

public class Tile {
	public readonly Texture2D Texture;
	public readonly Rectangle Section;
	public readonly Vector2[] UV;

	public Tile(Texture2D texture, Rectangle section) {
		Texture = texture;
		Section = section;
		float w = texture.Width;
		float h = texture.Height;
		UV = new Vector2[] {
			new Vector2(section.Left / w, section.Top / h),
            new Vector2(section.Right / w, section.Top / h),
            new Vector2(section.Right / w, section.Bottom / h),
            new Vector2(section.Left / w, section.Bottom / h),
        };
	}
}