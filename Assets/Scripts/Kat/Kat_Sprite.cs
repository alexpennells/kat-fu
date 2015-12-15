using System;
using UnityEngine;

public class Kat_Sprite : SpriteObj {

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  public override void Step() {
    if (Kat.Hurt || IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (IsPlaying("kat_dodge", "kat_gun_dodge")) {
      if (Kat.stopPhysics) {
        if (Base.Physics.hspeed > 0)
          transform.localScale = new Vector3(-1, 1, 1);
        else
          transform.localScale = new Vector3(1, 1, 1);
        return;
      }
      else
        Play("Idle");
    }

    if (Kat.Stance != eKat_Stance.GUN || !Game.AttackHeld)
      TurnSprite();

    if (IsPlaying("kat_lunge", "kat_kick") && Kat.stopPhysics == false) {
      if (Base.HasFooting)
        Play("Walk");
      else
        Play("Jump");
    }

    if (IsPlaying("kat_jump", "kat_gun_jump", "kat_gun_jump_shoot"))
      Play("Jump");

    if (Base.HasFooting && !IsPlaying("kat_punch_1", "kat_punch_2", "kat_lunge", "kat_uppercut")) {
      if (!Game.LeftHeld && !Game.RightHeld)
        Play("Idle");
      else {
        Play("Walk");
        SetSpeed(Math.Abs(Base.Physics.hspeed / 4f));
      }
    }

    if (!Kat.preventAlphaChange && GetAlpha() < 1f) {
      SetAlpha(GetAlpha() + 0.025f);
    }
  }

  private void TurnSprite() {
    if (Base.Physics.hspeed < 0)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Base.Physics.hspeed > 0)
      transform.localScale = new Vector3(1, 1, 1);
  }

  public void PlayFootstepSound() { Base.Sound.Play("Footstep"); }
  public void PlayLandSound() { Base.Sound.Play("Land"); }

  public void AnimationComplete (string animation) {
    switch (animation) {
      case "kat_pound":
        Base.Physics.vspeed = -8;
        Kat.stopPhysics = false;
        break;
      case "kat_punch_1":
        if (Kat.playNextPunch) {
          Play("Punch2");
          Base.Sound.Play("Punch");
        }
        else
          Play("Idle");
        break;
      case "kat_punch_2":
        Play("Idle");
        break;
      case "kat_recover":
        Kat.StartInvincible();
        break;
      case "kat_gun_recover":
        Kat.StartInvincible();
        break;
      case "kat_gun_start":
        Animate("kat_gun_idle", 1f);
        break;
      case "kat_gun_end":
        Animate("kat_idle", 1f);
        break;
      default:
        break;
    }
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_idle", 1f);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Animate("kat_gun_idle", 1f);
      else {
        if (IsPlaying("kat_gun_walk_shoot"))
          Animate("kat_gun_idle_shoot", 1f, 0.13f);
        else
          Animate("kat_gun_idle_shoot", 1f);
      }
    }
  }

  public void PlayWalk() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    float animSpeed = Math.Abs(Base.Physics.hspeed / 4f);

    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_walk", animSpeed);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Animate("kat_gun_walk", animSpeed);
      else {
        if (IsPlaying("kat_gun_idle_shoot"))
          Animate("kat_gun_walk_shoot", animSpeed, 0.13f);
        else
          Animate("kat_gun_walk_shoot", animSpeed);
      }
    }
  }

  public void PlayJump() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_jump", 1f);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Animate("kat_gun_jump", 1f);
      else
        Animate("kat_gun_jump_shoot", 1f);
    }
  }

  public void PlayDodge() {
    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_dodge", 1f);
    else
      Animate("kat_gun_dodge", 1f);
    SetAlpha(0.5f);
  }

  public void PlaySwitchStance() {
    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_gun_start", 1f);
    else
      Animate("kat_gun_end", 1f);
  }

  public void PlayPunch1() {
    Animate("kat_punch_1", 1f);
  }

  public void PlayPunch2() {
    Animate("kat_punch_2", 1f);
  }

  public void PlayKick() {
    if (Base.HasFooting)
      Animate("kat_lunge", 2f);
    else
      Animate("kat_kick", 2f);
  }

  public void PlayUppercut() {
    Animate("kat_uppercut", 2f);
  }

  public void PlayGroundPound() {
    Animate("kat_pound", 1f);
  }

  public void PlaySpin() {
    Animate("kat_spin", 2f);
  }

  public void PlayHurt() {
    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_hurt", 1f);
    else
      Animate("kat_gun_hurt", 1f);
  }

  public void PlayRecover() {
    if (Kat.Stance == eKat_Stance.KATFU)
      Animate("kat_recover", 1f);
    else
      Animate("kat_gun_recover", 1f);
  }
}
