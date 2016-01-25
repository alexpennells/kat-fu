using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kat_SolidCollider : SolidColliderObj {
  private bool bounced = false;

  public override void HandleCollisions (List<SolidObj> others) {
    base.HandleCollisions(others);
    if (bounced)
      ClearFooting();
  }

  protected override void FootingCollision(SolidObj footing) {
    if (Base.Physics.vspeed != 0)
      Base.Sound.Play("Land");

    base.FootingCollision(footing);

    bounced = false;
    if (Base.Sprite.IsPlaying("kat_pound"))
      BounceOffGround();

    Stitch.Kat.hasAirAttack = true;
    Stitch.Kat.hasUppercut = true;

    if (Base.Sprite.IsPlaying("kat_uppercut"))
      Base.Sprite.Play("Idle");
  }

  protected override void WallCollision (SolidObj wall) {
    Base.Physics.hspeed = 0;
  }

  private void BounceOffGround () {
    Stitch.Kat.stopPhysics = false;
    Stitch.Kat.AirKickTimer.Enabled = false;
    Stitch.Kat.GroundKickTimer.Enabled = false;
    Stitch.Kat.hasAirAttack = true;
    Stitch.Kat.hasUppercut = true;

    Base.Physics.hspeed = 0;
    Base.Physics.vspeed = 7;

    Base.Sprite.Play("Spin");
    bounced = true;
  }
}
