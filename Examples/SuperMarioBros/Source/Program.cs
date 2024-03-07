using Platformer;
using System;

Core.Create<SuperMarioBros>();

public class SuperMarioBros : Core {

	protected override void Initialize() {
		base.Initialize();

		Camera.SetDimenions(256, 240);
	}

	public override Type GetPlayerTpye() => typeof(Mario);
}