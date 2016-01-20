using System;

public class MilkJug_Sound : SoundObj {
  public override Type Lib { get { return typeof(MilkJug_SFXLib); } }

  public void PlayBounce() {
    Game.SFX.Play("MilkJugBounce", 0.4f);
  }

  public void PlayHurt() {
    Game.SFX.Play("MilkJugHurt", 0.8f);
  }
}
