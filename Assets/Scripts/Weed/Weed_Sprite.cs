using UnityEngine;
using System.Collections;

public class Weed_Sprite : SpriteObj {

  public override void Step () {
    if (IsPlaying("weed_hurt"))
        HurtTurnSprite();

    if (IsPlaying("weed_hurt") && !(Base as Weed_Base).hurt)
      Play("weed_idle", 0.5f);

    if (IsPlaying("weed_hurt", "weed_die"))
      return;

    if (!Base.SolidPhysics.HasFooting)
      Play("weed_air", 1f);

    if (IsPlaying("weed_jump", "weed_land", "weed_air"))
      return;

    TurnSprite();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      transform.localScale = new Vector3(1, 1, 1);
    else if (Stitch.Kat.x > Base.x)
      transform.localScale = new Vector3(-1, 1, 1);
  }

  private void HurtTurnSprite() {
    if (Base.Physics.hspeed > 0)
      transform.localScale = new Vector3(1, 1, 1);
    else if (Base.Physics.hspeed < 0)
      transform.localScale = new Vector3(-1, 1, 1);
  }

  public void SpitAnimComplete() {
    if ((Base as Weed_Base).attackInterval != 0)
      (Base as Weed_Base).AttackTimer.Enabled = true;

    Play("weed_idle", 0.5f);
  }

  public void CreateEnergyBall() {
    BaseObj ball = Game.Create("EnergyBall", new Vector2(Base.x + (Stitch.Kat.x > Base.x ? 10 : -10), Base.Mask.Center.y - 5));
    ball.Physics.MoveTo(new Vector2(Stitch.Kat.x, Stitch.Kat.Mask.Center.y), 4f);
  }

  public void JumpAnimComplete() {
    Base.Physics.vspeed = 8;
  }

  public void LandAnimComplete() {
    Play("weed_idle", 0.5f);
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }
}
