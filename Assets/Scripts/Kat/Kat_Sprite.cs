using System;
using UnityEngine;

public class Kat_Sprite : SpriteObj {
  public bool KeepPunching { get { return keepPunching; } set { keepPunching = value; } }
  private bool keepPunching = false;

  public int AttackID { get { return attackId; } }
  private int attackId = -1;

  private float nextPunchTime = 0f;

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  public override void Step() {
    if (Kat.DisableInput)
      return;

    if (Kat.Is("Hurt") || IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (IsPlaying("kat_dodge", "kat_gun_dodge")) {
      if (Kat.stopPhysics) {
        if (Base.Physics.hspeed > 0)
          FacingLeft = true;
        else
          FacingRight = true;
        return;
      }
      else
        Play("Idle");
    }

    if (!Base.HasFooting && IsPlaying("kat_idle", "kat_walk", "kat_gun_idle", "kat_gun_walk"))
      Play("Jump");

    if (IsPlaying("kat_lunge", "kat_kick") && Kat.stopPhysics == false) {
      if (Base.HasFooting)
        Play("Walk");
      else
        Play("Jump");
    }

    if (IsPlaying("kat_jump", "kat_gun_jump", "kat_gun_jump_shoot"))
      Play("Jump");

    if (Base.HasFooting && !IsPlaying("kat_punch", "kat_lunge", "kat_uppercut")) {
      if (Base.Physics.hspeed == 0 || (!Game.LeftHeld && !Game.RightHeld))
        Play("Idle");
      else
        Play("Walk");
    }

    if (!Kat.preventAlphaChange && GetAlpha() < 1f) {
      SetAlpha(GetAlpha() + 0.025f);
    }

    if (Base.HasFooting && Math.Abs(Base.Physics.hspeed) > 0.5f)
      Stitch.CreateDust(Base.Position + Vector3.up * 12 + Vector3.right * Base.Physics.hspeed);
  }

  public void PlayFootstepSound() { Base.Sound.Play("Footstep"); }
  public void PlayLandSound() { Base.Sound.Play("Land"); }

  public void StopPunch(AnimationEvent animationEvent) {
    if (!KeepPunching) {
      PlayIdle();
      nextPunchTime = animationEvent.animatorStateInfo.normalizedTime;
      StopBlur();
    } else {
      float attackSpeed = Base.Physics.hspeed == 0 ? 7f : 2f;
      Base.Physics.hspeed = FacingRight ? attackSpeed : -attackSpeed;
      KeepPunching = false;
      Base.Sound.Play("Punch");
      attackId++;
    }
  }

  public void AnimationComplete (string animation) {
    switch (animation) {
      case "kat_pound":
        Base.Physics.vspeed = -8;
        Kat.stopPhysics = false;
        StartBlur(0.0025f, 0.8f, 0.08f);
        break;
      case "kat_uppercut":
        Play("Jump");
        break;
      case "kat_recover":
        Kat.State("Invincible");
        break;
      case "kat_gun_recover":
        Kat.State("Invincible");
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

    float animSpeed = Kat.DisableInput ? 1.5f : Math.Abs(Base.Physics.hspeed / 4f);

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

  public void PlayPunch() {
    KeepPunching = true;
    Animate("kat_punch", 1.5f, nextPunchTime);
    StartBlur(0.001f, 0.4f, 0.05f);
    attackId++;
  }

  public void PlayKick() {
    if (Base.HasFooting)
      Animate("kat_lunge", 2f);
    else
      Animate("kat_kick", 2f);

    StartBlur(0.001f, 0.4f, 0.05f);
    attackId++;
  }

  public void PlayUppercut() {
    Animate("kat_uppercut", 1f);
    StartBlur(0.001f, 0.3f, 0.02f);
    attackId++;
  }

  public void PlayGroundPound() {
    Animate("kat_pound", 1f);
    attackId++;
  }

  public void PlaySpin() {
    Animate("kat_spin", 1.5f);
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
