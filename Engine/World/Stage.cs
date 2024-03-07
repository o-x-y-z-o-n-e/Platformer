using System;
namespace Platformer.World;

public class Stage {

    private LinkedList<Entity> entities;

    public Stage() {
        entities = new LinkedList<Entity>();
    }
}