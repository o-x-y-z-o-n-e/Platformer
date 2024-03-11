using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Platformer.World;

public class Stage {

	public LinkedList<Entity> Entities => entities;
	public TileMap TileMap => tileMap;

    private LinkedList<Entity> entities;
	private TileMap tileMap;

    public Stage() {
        entities = new LinkedList<Entity>();
    }

	public static Stage Load(string path) {
		FileStream stream = null;
		XmlTextReader reader = null;
		Stage stage = null;

		try {
			stream = new FileStream(Core.Instance.Content.RootDirectory + "/" + path, FileMode.Open);
			reader = new XmlTextReader(stream);
			stage = new Stage();

			while(reader.Read()) {
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "stage") {
					stage.ReadStage(reader);
				}
			}

			reader.Close();
			stream.Close();
		} catch {
			System.Diagnostics.Debug.WriteLine("failed_004");
			if(reader != null) reader.Close();
			if(stream != null) stream.Close();
			return null;
		}

		return stage;
	}

	private void ReadStage(XmlTextReader reader) {
		while(reader.MoveToNextAttribute()) {
			
		}

		while(reader.Read()) {
			if(reader.NodeType == XmlNodeType.Element) {
				if(reader.Name == "tilemap") ReadTileMap(reader);
				else if(reader.Name == "portal") ReadPortal(reader);
			} else if(reader.NodeType == XmlNodeType.EndElement) {
				if(reader.Name == "stage") return;
			}
		}
	}

	private void ReadTileMap(XmlTextReader reader) {
		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "source") {
				tileMap = TileMap.Load(reader.Value);
			}
		}
	}

	private void ReadPortal(XmlTextReader reader) {
		while(reader.MoveToNextAttribute()) {
			if(reader.Name == "id") {
				
			} else if(reader.Name == "x") {

			} else if(reader.Name == "y") {

			} else if(reader.Name == "w") {

			} else if(reader.Name == "h") {

			}
		}
	}
}