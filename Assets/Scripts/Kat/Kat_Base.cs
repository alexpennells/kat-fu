using System;
using System.Timers;
using UnityEngine;

public class Kat_Base : InputObj {

  // Whehter the air kick is still up.
  public bool hasAirAttack = true;

  // Whether to prevent friction and gravity.
  public bool stopPhysics = false;

  // Whether or not to play the next punch in a hit combo.
  public bool playNextPunch = false;

  protected override void Init() {
    Game.Camera.InitPosition();

    AirKickTimer.Interval = 300;
    GroundKickTimer.Interval = 400;
    DodgeTimer.Interval = 300;
  }

  protected override void Step () {
    base.Step();

    if (stopPhysics) {
      Physics.SkipNextFrictionUpdate();
      Physics.SkipNextGravityUpdate();
      Physics.hspeedMax = 7;
    }
    else {
      Physics.hspeedMax = 5;
    }
  }

  protected override void LeftHeld (float val) {
    if (stopPhysics || Sprite.IsPlaying("kat_punch_1", "kat_punch_2"))
      return;

    Physics.hspeed -= 0.5f;
  }

  protected override void RightHeld (float val) {
    if (stopPhysics || Sprite.IsPlaying("kat_punch_1", "kat_punch_2"))
      return;

    Physics.hspeed += 0.5f;
  }

  protected override void JumpPressed () {
    if (SolidPhysics.HasFooting && !stopPhysics) {
      Physics.vspeed = 8;
      Sprite.Play("kat_jump", 1f);
    }
  }

  protected override void SkatePressed () {
    if (!stopPhysics && HasFooting) {
      DodgeTimer.Enabled = true;
      stopPhysics = true;
      Sprite.SetAlpha(0.5f);
      Sprite.Play("kat_dodge");

      if (transform.localScale.x < 0)
        Physics.hspeed = -7;
      else
        Physics.hspeed = 7;
    }
  }

  protected override void AttackPressed () {
    if (!SolidPhysics.HasFooting) {
      if (Game.DownHeld && !Sprite.IsPlaying("kat_pound"))
        StartPound();
      else if (hasAirAttack) {
        if (Game.UpHeld && !Sprite.IsPlaying("kat_uppercut")) {
          hasAirAttack = false;
          StartUppercut();
        }
        else
          StartAirKick();
      }
    }

    if (SolidPhysics.HasFooting && !Sprite.IsPlaying("kat_uppercut")) {
      if (Game.UpHeld) {
        StartUppercut();
        return;
      }

      if (stopPhysics)
        return;

      if (Game.LeftHeld || Game.RightHeld) {
        StartGroundKick();
        return;
      }

      if (Sprite.IsPlaying("kat_punch_1"))
        playNextPunch = true;
      else if (!Sprite.IsPlaying("kat_punch_2")) {
        Sprite.Play("kat_punch_1", 1f);
        playNextPunch = false;
      }
    }
  }

  private void StartAirKick () {
    hasAirAttack = false;
    stopPhysics = true;
    Physics.vspeed = 0;

    if (transform.localScale.x < 0)
      Physics.hspeed = -7;
    else
      Physics.hspeed = 7;

    Sprite.Play("kat_kick", 2f);
    AirKickTimer.Enabled = true;
  }

  private void StartGroundKick () {
    stopPhysics = true;

    if (transform.localScale.x < 0)
      Physics.hspeed = -7;
    else
      Physics.hspeed = 7;

    Sprite.Play("kat_lunge", 2f);
    GroundKickTimer.Enabled = true;
  }

  private void StartPound () {
    Sprite.Play("kat_pound", 1f);
    hasAirAttack = false;
    stopPhysics = true;
    Physics.vspeed = 2f;
  }

  private void StartUppercut () {
    GroundKickTimer.Enabled = false;
    stopPhysics = false;
    Sprite.Play("kat_uppercut", 2f);
    Physics.vspeed = 8;
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer AirKickTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    AirKickTimer.Enabled = false;
    stopPhysics = false;
  }

  public Timer GroundKickTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    GroundKickTimer.Enabled = false;
    stopPhysics = false;
  }

  public Timer DodgeTimer { get { return Timer3; } }
  protected override void Timer3Elapsed(object source, ElapsedEventArgs e) {
    DodgeTimer.Enabled = false;
    stopPhysics = false;
    Physics.hspeed = 0;
  }
}
