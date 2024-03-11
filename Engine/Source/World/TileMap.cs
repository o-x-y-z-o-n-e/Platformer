using System.IO;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Data;

namespace Platformer.World;

public class TileMap {

	private int width = 4;
	private int height = 4;

	private int xScale = 16;
    private int yScale = 16;

    private int xOffset = 0;
	private int yOffset = 0;

	private TileSet set;
	private List<Layer> layers;

	private TileMap() {
		layers = new List<Layer>();
	}

	public static TileMap Load(string path) {
		FileStream stream = null;
		XmlTextReader reader = null;
		TileMap map = null;

		try {
			stream = new FileStream(Core.Instance.Content.RootDirectory + "/" + path, FileMode.Open);
			reader = new XmlTextReader(stream);
			map = new TileMap();

			while(reader.Read()) {
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "map") {
					map.ReadMap(reader);
				}
			}

			reader.Close();
			stream.Close();
		} catch {
			System.Diagnostics.Debug.WriteLine("failed_001");
			if(reader != null) reader.Close();
			if(stream != null) stream.Close();
			return null;
		}

		return map;
	}

	private void ReadMap(XmlTextReader reader) {
		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "width") {
				int.TryParse(reader.Value, out width);
			} else if(reader.Name == "height") {
				int.TryParse(reader.Value, out height);
			} else if(reader.Name == "tilewidth") {
				int.TryParse(reader.Value, out xScale);
			} else if(reader.Name == "tileheight") {
				int.TryParse(reader.Value, out yScale);
			}
		}

		while(reader.Read()) {
			if(reader.NodeType == XmlNodeType.Element) {
				if(reader.Name == "tileset") ReadTileset(reader);
				else if(reader.Name == "layer") ReadLayer(reader);
			} else if(reader.NodeType == XmlNodeType.EndElement) {
				if(reader.Name == "map") return;
			}
		}
	}

	private void ReadTileset(XmlTextReader reader) {
		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "source") {
				set = TileSet.Load(reader.Value);
			}
		}
	}

	private Layer ReadLayer(XmlTextReader reader) {
		Layer layer = new Layer(this);

		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "width") {
				layer.Width = reader.ReadContentAsInt();
			} else if(reader.Name == "Height") {
				layer.Height = reader.ReadContentAsInt();
			}
		}

		layer.Tiles = new Tile[layer.Width * layer.Height];

		while(reader.Read()) {
			if(reader.NodeType == XmlNodeType.Element) {
				if(reader.Name == "data") ReadData(reader, ref layer);
			} else if(reader.NodeType == XmlNodeType.EndElement) {
				if(reader.Name == "layer") return layer;
			}
		}

		layer.Generate();
		layers.Add(layer);

		return layer;
	}

	private void ReadData(XmlTextReader reader, ref Layer layer) {
		bool csv = false;

		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "csv") {
				csv = true;
			}
		}

		while(reader.Read()) {
			if(reader.NodeType == XmlNodeType.Text) {
				if(csv) ReadCSV(reader, ref layer);
			} else if(reader.NodeType == XmlNodeType.EndElement) {
				if(reader.Name == "data") return;
			}
		}
	}

	private void ReadCSV(XmlTextReader reader, ref Layer layer) {
		if(reader.NodeType == XmlNodeType.Text) {
			int m;
			string[] str = reader.Value.Split(',');
			for(int i = 0; i < layer.Width * layer.Height && i < str.Length; i++) {
				if(layer.Tiles[i] == null && int.TryParse(str[i], out m) && m > 0) {
					layer.Tiles[i] = set.GetTile(m - 1);
				}
			}
		}
	}

	public void Draw() {
		foreach(Layer layer in layers) {
			layer.Draw();
		}
	}

	public class Layer {

		public readonly TileMap Map;

		public int Width;
		public int Height;

		public Tile[] Tiles;
		private List<RenderBatch> renderBatches;

		public Layer(TileMap map) {
			Map = map;
			renderBatches = new List<RenderBatch>();
		}

		public void Generate() {
			int m = -1;
			while((m = GetNextTile(m)) >= 0) {
				Tile tile = Tiles[m];

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
				if(i >= Tiles.Length)
					return -1;

				if(Tiles[i] != null)
					return i;
			}
		}

	}

	public class RenderBatch {

		public readonly Layer Layer;
		public TileMap Map => Layer.Map;

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

        public RenderBatch(Layer layer, Texture2D texture) {
			Layer = layer;
            Tiles = new List<int>();
			Vertices = null;
			Indices = null;
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
				Tile tile = Layer.Tiles[m];
                int x = m % Layer.Width;
                int y = m / Layer.Height;

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