using System;

public class Manhole_Sound : SoundObj {
  public override Type Lib { get { return typeof(Manhole_SFXLib); } }

  public void PlayDown() {
    Game.SFX.Play("ManholeDown", 0.3f);
  }

  public void PlayUp() {
    Game.SFX.Play("ManholeUp", 0.3f);
  }

  public void PlayFlip() {
    Game.SFX.Play("ManholeFlip", 0.7f);
  }
}
