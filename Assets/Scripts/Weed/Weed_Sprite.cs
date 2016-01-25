using UnityEngine;
using System.Collections;

public class Weed_Sprite : SpriteObj {

  public override void Step () {
    if (IsPlaying("weed_hurt"))
        HurtTurnSprite();

    if (IsPlaying("weed_hurt") && !(Base as Weed_Base).hurt)
      Play("Idle");

    if (IsPlaying("weed_hurt", "weed_die"))
      return;

    if (!Base.SolidPhysics.HasFooting)
      Play("Air");

    if (IsPlaying("weed_jump", "weed_land", "weed_air"))
      return;

    TurnSprite();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      FacingRight = true;
    else if (Stitch.Kat.x > Base.x)
      FacingLeft = true;
  }

  private void HurtTurnSprite() {
    if (Base.Physics.hspeed > 0)
      FacingRight = true;
    else if (Base.Physics.hspeed < 0)
      FacingLeft = true;
  }

  public void SpitAnimComplete() {
    if ((Base as Weed_Base).attackInterval != 0)
      (Base as Weed_Base).AttackTimer.Enabled = true;

    Play("Idle");
  }

  public void CreateEnergyBall() {
    BaseObj ball = Game.Create("EnergyBall", new Vector2(Base.x + (Stitch.Kat.x > Base.x ? 10 : -10), Base.Mask.Center.y - 5));
    ball.Physics.MoveTo(new Vector2(Stitch.Kat.x, Stitch.Kat.Mask.Center.y), 4f);
  }

  public void JumpAnimComplete() {
    Base.Physics.vspeed = 8;
  }

  public void LandAnimComplete() {
    if ((Base as Weed_Base).attackInterval != 0)
      (Base as Weed_Base).AttackTimer.Enabled = true;

    Play("Idle");
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    Animate("weed_idle", 0.5f);
  }

  public void PlayBite() {
    Animate("weed_bite", 1.5f);
  }

  public void PlaySpit() {
    Animate("weed_spit", 1f);
  }

  public void PlayJump() {
    Animate("weed_jump", 2f);
  }

  public void PlayAir() {
    Animate("weed_air", 1f);
  }

  public void PlayLand() {
    Animate("weed_land", 1f);
  }

  public void PlayHurt() {
    Animate("weed_hurt", 1f);
  }

  public void PlayDie() {
    Animate("weed_die", 1f);
  }
}
