using System;
using System.Collections.Generic;
using System.Linq;

namespace Platformer.World;

public class Context {

    public Player Player => player;
    public Stage CurrentStage => GetStage(currentStageIndex);

    public int StageCount => stages.Count;

    private Player player;
    private List<Stage> stages;
    private int currentStageIndex;

    public Context() {
        player = Activator.CreateInstance(Core.Instance.GetPlayerTpye()) as Player;
        stages = new List<Stage> {
			Stage.Load("stage_dev.xml")
		};
        currentStageIndex = 0;
    }

    public Stage GetStage(int i) {
        return i >= 0 && i < stages.Count ? stages[i] : null;
    }

	public void Update(float deltaTime) {
		Stage stage = GetStage(currentStageIndex);
		if(stage != null) {
			foreach(Entity entity in stage.Entities) {
				entity.Update(deltaTime);
			}
		}

		player.Update(deltaTime);
	}

	public void Draw() {
		Stage stage = GetStage(currentStageIndex);
		if(stage != null) {
			stage.TileMap.Draw();
			foreach(Entity entity in stage.Entities) {
				entity.Draw();
			}
			System.Diagnostics.Debug.WriteLine("hi");
		}

		player.Draw();
	}
}