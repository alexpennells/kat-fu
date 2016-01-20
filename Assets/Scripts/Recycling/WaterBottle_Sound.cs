public class WaterBottle_Sound : SoundObj {
  public void PlayBounce() {
    Game.SFX.Play("WaterBottleBounce", 0.4f);
  }

  public void PlayHurt() {
    Game.SFX.Play("WaterBottleHurt", 0.8f);
  }
}
