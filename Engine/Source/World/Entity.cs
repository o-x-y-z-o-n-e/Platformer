using Microsoft.Xna.Framework;

namespace Platformer.World;

public class Entity {

    // Constants
    protected const int DEFAULT_TICK_RATE = 60;

    // Protected
    protected int tickRate = DEFAULT_TICK_RATE;
    protected float tickInterval => 1.0F / tickRate;
    protected Matrix transform = Matrix.Identity;

    // Private
    private Vector2 position = Vector2.Zero;
    private float tickTimer = 0.0F;
    

    public Entity() {
        LoadAssets();
    }

    public virtual void Update(float deltaTime) {
        tickTimer += deltaTime * tickRate;
        if(tickTimer > 1.0F) {
            tickTimer -= 1.0F;
            Tick();
        }
    }

    public virtual void Tick() {

    }

    public virtual void Draw() {

    }

    protected virtual void LoadAssets() {

    }

}