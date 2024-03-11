using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.World;

namespace Platformer.Data;

public class TileSet {

	private Texture2D texture;
	private Tile[] tiles;

	public static TileSet Load(string path) {
		FileStream stream = null;
		XmlTextReader reader = null;
		TileSet set = null;

		try {
			stream = new FileStream(Core.Instance.Content.RootDirectory + "/" + path, FileMode.Open);
			reader = new XmlTextReader(stream);
			set = new TileSet();

			while(reader.Read()) {
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "tileset") {
					set.ReadSet(reader);
				}
			}

			reader.Close();
			stream.Close();
		} catch {
			System.Diagnostics.Debug.WriteLine("failed_002");
			if(reader != null) reader.Close();
			if(stream != null) stream.Close();
			return null;
		}

		return set;
	}

	private void ReadSet(XmlTextReader reader) {
		int w = 0;
		int h = 0;
		int n = 0;
		int c = 0;

		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "tilewidth") {
				w = int.Parse(reader.Value);
			} else if(reader.Name == "tileheight") {
				h = int.Parse(reader.Value);
			} else if(reader.Name == "tilecount") {
				n = int.Parse(reader.Value);
			} else if(reader.Name == "columns") {
				c = int.Parse(reader.Value);
			}
		}

		while(reader.Read()) {
			if(reader.NodeType == XmlNodeType.Element) {
				if(reader.Name == "image") ReadImage(reader);
			} else if(reader.NodeType == XmlNodeType.EndElement) {
				if(reader.Name == "tileset") break;
			}
		}

		tiles = new Tile[n];
		for(int y = 0; y < n / c; y++) {
			for(int x = 0; x < c; x++) {
				if(texture != null) {
					tiles[x + c * y] = new Tile(texture, new Rectangle(x * w, y * h, w, h));
				}
			}
		}
	}

	private void ReadImage(XmlTextReader reader) {
		string path = "";
		int w = 0;
		int h = 0;

		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "source") {
				path = reader.Value;
			} else if(reader.Name == "width") {
				w = int.Parse(reader.Value);
			} else if(reader.Name == "height") {
				h = int.Parse(reader.Value);
			}
		}

		texture = Assets.Load<Texture2D>(path);
		if(texture.Width != w || texture.Height != h) {
			System.Diagnostics.Debug.WriteLine("failed_003");
			texture = null;
		}
	}

	public Tile GetTile(int i) {
		if(i < 0 || i >= tiles.Length) return null;
		return tiles[i];
	}

}