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

    bool dontBounce = false;
    if (footing.SpecialType == eObjectType.MANHOLE && Base.Physics.vspeed < 0) {
      Manhole_Base manhole = footing as Manhole_Base;

      if (manhole.strength == 0 || (Base.Is("GroundPounding") && (manhole.strength == 1 || Stitch.canGroundBoom))) {
        manhole.StartSpinning();
        dontBounce = true;
      }
    }

    base.FootingCollision(footing);

    bounced = false;
    if (Base.Sprite.IsPlaying("kat_pound"))
      BounceOffGround();

    if (dontBounce)
      Base.Physics.vspeed = 0;

    Stitch.Kat.hasAirAttack = true;
    Stitch.Kat.hasUppercut = true;

    if (Base.Sprite.IsPlaying("kat_uppercut"))
      Base.Sprite.Play("Idle");
  }

  protected override void WallCollision (SolidObj wall) {
    Base.Physics.hspeed = 0;
  }

  protected override void RoofCollision (SolidObj roof) {
    if (roof.SpecialType == eObjectType.MANHOLE && Base.Physics.vspeed > 0)
      (roof as Manhole_Base).StartSpinning();
    else
      base.RoofCollision(roof);
  }

  private void BounceOffGround () {
    Stitch.Kat.stopPhysics = false;
    Stitch.Kat.AirKickTimer.Enabled = false;
    Stitch.Kat.GroundKickTimer.Enabled = false;
    Stitch.Kat.hasAirAttack = true;
    Stitch.Kat.hasUppercut = true;

    Base.Physics.hspeed = 0;
    Base.Physics.vspeed = 5;

    if (Stitch.canGroundBoom)
      Stitch.CreateGroundBoom(new Vector3(Base.x, Base.Mask.Bottom, Base.z));
    else
      Stitch.CreateGroundHit(new Vector3(Base.x, Base.Mask.Bottom, Base.z));

    Base.Sound.Play("Impact");
    Base.Sprite.Play("Spin");
    bounced = true;
  }
}
