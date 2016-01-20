public class MilkJug_Sound : SoundObj {
  public void PlayBounce() {
    Game.SFX.Play("MilkJugBounce", 0.4f);
  }

  public void PlayHurt() {
    Game.SFX.Play("MilkJugHurt", 0.8f);
  }
}
