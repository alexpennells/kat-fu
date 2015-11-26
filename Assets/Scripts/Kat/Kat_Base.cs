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

  // Whether to prevent the alpha from fading back to 1 when hurt.
  public bool preventAlphaChange = false;

  public bool Invincible { get { return Sprite.GetAlpha() < 0.8f; } }
  public bool Hurt { get { return Sprite.IsPlaying("kat_hurt", "kat_recover"); } }
  public bool Punching { get { return Sprite.IsPlaying("kat_punch_1", "kat_punch_2"); } }
  public bool Kicking { get { return Sprite.IsPlaying("kat_kick", "kat_lunge"); } }
  public bool GroundPounding { get { return Sprite.IsPlaying("kat_pound") && Physics.vspeed < 0; } }
  public bool Uppercutting { get { return Sprite.IsPlaying("kat_uppercut") && Physics.vspeed > 0; } }

  public void GetHurt(float hspeed) {
    stopPhysics = false;
    AirKickTimer.Enabled = false;
    GroundKickTimer.Enabled = false;
    hasAirAttack = true;

    Physics.vspeed = 0;
    Physics.hspeed = hspeed;
    if (hspeed < 0)
      transform.localScale = new Vector3(1, 1, 1);
    else
      transform.localScale = new Vector3(-1, 1, 1);

    Sprite.Play("kat_hurt", 1f);
  }

  public void StartInvincible() {
    Sprite.SetAlpha(0.5f);
    Sprite.Play("kat_idle", 1f);
    InvincibleTimer.Enabled = true;
    preventAlphaChange = true;
  }

  protected override void Init() {
    Game.Camera.InitPosition();

    AirKickTimer.Interval = 300;
    GroundKickTimer.Interval = 400;
    DodgeTimer.Interval = 300;
    InvincibleTimer.Interval = 1000;
  }

  protected override void Step () {
    if (Hurt) {
      if (Physics.hspeed == 0 && Sprite.IsPlaying("kat_hurt"))
        Sprite.Play("kat_recover");
      return;
    }

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

  public Timer InvincibleTimer { get { return Timer4; } }
  protected override void Timer4Elapsed(object source, ElapsedEventArgs e) {
    InvincibleTimer.Enabled = false;
    preventAlphaChange = false;
  }
}
