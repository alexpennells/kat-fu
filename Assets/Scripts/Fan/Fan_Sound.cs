using System;

public class Fan_Sound : SoundObj {
  public override Type Lib { get { return typeof(Fan_SFXLib); } }

  public void PlayBreak() {
    Game.SFX.Play("FanBreak", 0.4f);
  }

}
