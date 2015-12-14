using UnityEngine;
using System.Collections;

public class Bat_Sprite : SpriteObj {
  public override void Step () {
    if (IsPlaying("bat_hurt")) {
      if (!(Base as Bat_Base).hurt)
        Play("Fly");
      else
        return;
    }

    TurnSprite();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Stitch.Kat.x > Base.x)
      transform.localScale = new Vector3(1, 1, 1);
  }

  public void AttackAnimComplete() {
    if ((Base as Bat_Base).attackInterval != 0)
      (Base as Bat_Base).AttackTimer.Enabled = true;

    Play("Fly");
  }

  public void CreateEnergyBall() {
    BaseObj ball = Game.Create("EnergyBall", new Vector2(Base.x, Base.y + 10));
    ball.Physics.MoveTo(new Vector2(Stitch.Kat.x, Stitch.Kat.Mask.Center.y), 3f);
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayFly() {
    Animate("bat_fly", 1f);
  }

  public void PlayAttack() {
    Animate("bat_attack", 1f);
  }

  public void PlayHurt() {
    Animate("bat_hurt", 1f);
  }

  public void PlayDie() {
    Animate("bat_die", 1f);
  }
}
