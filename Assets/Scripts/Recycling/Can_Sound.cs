using System;

public class Can_Sound : SoundObj {
  public override Type Lib { get { return typeof(Can_SFXLib); } }

  public void PlayBounce() {
    Game.SFX.Play("CanBounce", 0.2f);
  }

  public void PlayHurt() {
    Game.SFX.Play("CanHurt", 0.8f);
  }
}
