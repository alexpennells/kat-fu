using System;

public class BoxSpawner_Sound : SoundObj {
  public override Type Lib { get { return typeof(BoxSpawner_SFXLib); } }

  public void PlaySpawn() {
    Game.SFX.Play("BoxSpawn", 1f);
  }

  public void PlayHurt() {
    Game.SFX.Play("BoxSpawnerHurt", 0.8f);
  }
}
