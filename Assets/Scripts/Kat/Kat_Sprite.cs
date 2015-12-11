using System;
using UnityEngine;

public class Kat_Sprite : SpriteObj {

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  public override void Step() {
    if (Kat.Hurt || IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (IsPlaying("kat_dodge")) {
      if (Kat.stopPhysics) {
        if (Base.Physics.hspeed > 0)
          transform.localScale = new Vector3(-1, 1, 1);
        else
          transform.localScale = new Vector3(1, 1, 1);
        return;
      }
      else
        PlayIdle();
    }

    if (Kat.Stance != eKat_Stance.GUN || !Game.AttackHeld)
      TurnSprite();

    if (IsPlaying("kat_lunge", "kat_kick") && Kat.stopPhysics == false) {
      if (Base.HasFooting)
        PlayWalk();
      else
        PlayJump();
    }

    if (IsPlaying("kat_jump", "kat_gun_jump", "kat_gun_jump_shoot"))
      PlayJump();

    if (Base.HasFooting && !IsPlaying("kat_punch_1", "kat_punch_2", "kat_lunge", "kat_uppercut")) {
      if (!Game.LeftHeld && !Game.RightHeld)
        PlayIdle();
      else {
        PlayWalk();
        SetSpeed(Math.Abs(Base.Physics.hspeed / 4f));
      }
    }

    if (!Kat.preventAlphaChange && GetAlpha() < 1f) {
      SetAlpha(GetAlpha() + 0.025f);
    }
  }

  public void PlayIdle() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (Kat.Stance == eKat_Stance.KATFU)
      Play("kat_idle", 1f);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Play("kat_gun_idle", 1f);
      else {
        if (IsPlaying("kat_gun_walk_shoot"))
          Play("kat_gun_idle_shoot", 1f, 0.13f);
        else
          Play("kat_gun_idle_shoot", 1f);
      }
    }
  }

  public void PlayWalk() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (Kat.Stance == eKat_Stance.KATFU)
      Play("kat_walk", 1f);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Play("kat_gun_walk", 1f);
      else {
        if (IsPlaying("kat_gun_idle_shoot"))
          Play("kat_gun_walk_shoot", 1f, 0.13f);
        else
          Play("kat_gun_walk_shoot", 1f);
      }
    }

    SetSpeed(Math.Abs(Base.Physics.hspeed / 4f));
  }

  public void PlayJump() {
    if (IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (Kat.Stance == eKat_Stance.KATFU)
      Play("kat_jump", 1f);
    else if (Kat.Stance == eKat_Stance.GUN) {
      if (!Game.AttackHeld)
        Play("kat_gun_jump", 1f);
      else
        Play("kat_gun_jump_shoot", 1f);
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
        Base.Physics.hspeed = 0;
        Kat.stopPhysics = false;
        break;
      case "kat_punch_1":
        if (Kat.playNextPunch) {
          Play("kat_punch_2", 1f);
          Base.Sound.Play("Punch");
        }
        else
          PlayIdle();
        break;
      case "kat_punch_2":
        PlayIdle();
        break;
      case "kat_recover":
        Kat.StartInvincible();
        break;
      case "kat_gun_recover":
        Kat.StartInvincible();
        break;
      case "kat_gun_start":
        Play("kat_gun_idle", 1f);
        break;
      case "kat_gun_end":
        Play("kat_idle", 1f);
        break;
      default:
        break;
    }
  }
}
