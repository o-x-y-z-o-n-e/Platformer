using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.World;

namespace Platformer;

public class Core : Game {

    public static readonly Core Instance = new Core();

    private GraphicsDeviceManager graphics;
    private SpriteBatch? spriteBatch;
    private Context? context;

    private Core() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
        base.Initialize();

        
    }

    protected override void LoadContent() {
        base.LoadContent();

        spriteBatch = new SpriteBatch(GraphicsDevice);

        context = new Context();
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        float deltaTime = gameTime.ElapsedGameTime.Seconds;

        if(context != null) {
            
        }
    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        if(context != null) {

        }
    }


}

// Camera
// Tilemap
// Player