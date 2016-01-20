public class Kat_Sound : SoundObj {
  public void PlayFootstep() {
    Game.SFX.Play("Footstep" + Game.Random.Next(1, 5), 0.8f);
  }

  public void PlayLand() {
    Game.SFX.Play("Footstep" + Game.Random.Next(1, 5), 1f);
  }

  public void PlayPunch(){
    Game.SFX.Play("Punch" + Game.Random.Next(1, 4), 0.5f);
  }

  public void PlayKick(){
    Game.SFX.Play("Punch" + Game.Random.Next(1, 4), 1f);
  }

  public void PlayAttack(){
    Game.SFX.Play("CatAttack" + Game.Random.Next(1, 4), 0.1f);
  }

  public void PlayHiss(){
    Game.SFX.Play("CatHiss" + Game.Random.Next(1, 4), 0.25f);
  }

  public void PlayUppercut(){
    Game.SFX.Play("CatUppercut", 1f);
  }

  public void PlayDodge() {
    Game.SFX.Play("CatDodge", 1f);
  }

  public void PlayGunStart() {
    Game.SFX.Play("GunStart", 0.3f);
  }

  public void PlayGunEnd() {
    Game.SFX.Play("GunEnd", 0.3f);
  }

  public void PlayGunshot() {
    Game.SFX.Play("Gunshot" + Game.Random.Next(1, 4), 0.5f);
  }

  public void PlayHurt() {
    Game.SFX.Play("CatHurt", 0.5f);
  }
}
