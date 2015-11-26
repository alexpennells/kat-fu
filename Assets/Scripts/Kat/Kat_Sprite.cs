using System;
using UnityEngine;

public class Kat_Sprite : SpriteObj {

  [Tooltip("The speed that the sprite turns around")]
  public float turnSpeed = 0.2f;

  public override void Step() {
    if ((Base as Kat_Base).Hurt)
      return;

    if (IsPlaying("kat_dodge")) {
      if ((Base as Kat_Base).stopPhysics) {
        if (Base.Physics.hspeed > 0)
          transform.localScale = new Vector3(-1, 1, 1);
        else
          transform.localScale = new Vector3(1, 1, 1);
        return;
      }
      else
        Play("kat_idle", 1f);
    }

    TurnSprite();

    if (IsPlaying("kat_lunge", "kat_kick") && (Base as Kat_Base).stopPhysics == false) {
      if (Base.HasFooting)
        Play("kat_walk");
      else
        Play("kat_jump", 1f);
    }

    if (Base.SolidPhysics.HasFooting && !IsPlaying("kat_punch_1", "kat_punch_2", "kat_lunge", "kat_uppercut")) {
      if (!Game.LeftHeld && !Game.RightHeld)
        Play("kat_idle", 1f);
      else {
        Play("kat_walk");
        SetSpeed(Math.Abs(Base.Physics.hspeed / 4f));
      }
    }

    if (!(Base as Kat_Base).preventAlphaChange && GetAlpha() < 1f) {
      SetAlpha(GetAlpha() + 0.025f);
    }
  }

  private void TurnSprite() {
    if (Base.Physics.hspeed < 0)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Base.Physics.hspeed > 0)
      transform.localScale = new Vector3(1, 1, 1);

    // if (Base.Physics.hspeed < 0 && transform.localScale.x > -1)
    //   transform.localScale = transform.localScale - new Vector3(turnSpeed, 0, 0);
    // else if (Base.Physics.hspeed > 0 && transform.localScale.x < 1)
    //   transform.localScale = transform.localScale + new Vector3(turnSpeed, 0, 0);
    // else if (Base.Physics.hspeed == 0) {
    //   if (transform.localScale.x > 0 && transform.localScale.x < 1)
    //     transform.localScale = transform.localScale + new Vector3(turnSpeed, 0, 0);
    //   else if (transform.localScale.x < 0 && transform.localScale.x > -1)
    //     transform.localScale = transform.localScale - new Vector3(turnSpeed, 0, 0);
    // }

    // if (transform.localScale.x > 1)
    //   transform.localScale = new Vector3(1, 1, 1);
    // else if (transform.localScale.x < -1)
    //   transform.localScale = new Vector3(-1, 1, 1);
  }

  public void AnimationComplete (string animation) {
    switch (animation) {
      case "kat_pound":
        Base.Physics.vspeed = -8;
        (Base as Kat_Base).stopPhysics = false;
        break;
      case "kat_punch_1":
        if ((Base as Kat_Base).playNextPunch)
          Play("kat_punch_2", 1f);
        else
          Play("kat_idle", 1f);
        break;
      case "kat_punch_2":
        Play("kat_idle", 1f);
        break;
      case "kat_recover":
        (Base as Kat_Base).StartInvincible();
        break;
      default:
        break;
    }
  }
}
