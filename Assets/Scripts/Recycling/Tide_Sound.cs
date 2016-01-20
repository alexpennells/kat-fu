public class Tide_Sound : SoundObj {
  public void PlayBounce() {
    Game.SFX.Play("TideBounce", 0.4f);
  }

  public void PlayHurt() {
    Game.SFX.Play("TideHurt", 0.8f);
  }
}
