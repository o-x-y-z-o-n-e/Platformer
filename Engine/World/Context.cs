using System;

namespace Platformer.World;

public class Context {

    public Player Player => player;
    public Stage? CurrentStage => GetStage(currentStageIndex);

    public int StageCount => stages.Count;

    private Player player;
    private List<Stage> stages;
    private int currentStageIndex;

    public Context() {
        player = new Player();
        stages = new List<Stage>();
        currentStageIndex = -1;
    }

    public Stage? GetStage(int i) {
        return i >= 0 && i < stages.Count ? stages[i] : null;
    }
}