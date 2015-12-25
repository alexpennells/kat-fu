using System;
using System.Timers;
using UnityEngine;

public class Kat_Base : InputObj {

  // Whehter the air kick is still up.
  public bool hasAirAttack = true;

  // Whehter the uppercut is still up.
  public bool hasUppercut = true;

  // Whether to prevent friction and gravity.
  public bool stopPhysics = false;

  // Whether or not to play the next punch in a hit combo.
  public bool playNextPunch = false;

  // Whether to prevent the alpha from fading back to 1 when hurt.
  public bool preventAlphaChange = false;

  public bool Invincible { get { return Sprite.GetAlpha() < 0.8f; } }
  public bool Hurt { get { return Sprite.IsPlaying("kat_hurt", "kat_recover", "kat_gun_hurt", "kat_gun_recover"); } }
  public bool Punching { get { return Sprite.IsPlaying("kat_punch_1", "kat_punch_2"); } }
  public bool Kicking { get { return Sprite.IsPlaying("kat_kick", "kat_lunge"); } }
  public bool GroundPounding { get { return Sprite.IsPlaying("kat_pound") && Physics.vspeed < 0; } }
  public bool Uppercutting { get { return Sprite.IsPlaying("kat_uppercut") && Physics.vspeed > 0; } }

  private eKat_Stance stance = eKat_Stance.KATFU;
  public eKat_Stance Stance {
    get { return stance; }
    set { stance = value; }
  }

  public void GetHurt(float hspeed) {
    stopPhysics = false;
    AirKickTimer.Enabled = false;
    GroundKickTimer.Enabled = false;
    hasAirAttack = true;
    hasUppercut = true;

    Physics.hspeedMax = 5;
    Physics.vspeed = 0;
    Physics.hspeed = hspeed;
    if (hspeed < 0)
      transform.localScale = new Vector3(1, 1, 1);
    else
      transform.localScale = new Vector3(-1, 1, 1);

    Sound.Play("Hurt");
    Sprite.Play("Hurt");
  }

  public void StartInvincible() {
    Sprite.SetAlpha(0.5f);
    Sprite.Play("Idle");
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
      if (Physics.hspeed == 0)
        Sprite.Play("Recover");
      return;
    }

    base.Step();

    if (stopPhysics) {
      Physics.SkipNextFrictionUpdate();
      Physics.SkipNextGravityUpdate();
      Physics.hspeedMax = 7;
    }
    else {
      if (Stance == eKat_Stance.KATFU)
        Physics.hspeedMax = 5;
      else if (Stance == eKat_Stance.GUN) {
        if (!Game.AttackHeld)
          Physics.hspeedMax = 4;
        else
          Physics.hspeedMax = 3;
      }
    }
  }

  protected override void LeftHeld (float val) {
    if (stopPhysics || Sprite.IsPlaying("kat_punch_1", "kat_punch_2"))
      return;

    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    Physics.hspeed -= 0.5f;
  }

  protected override void RightHeld (float val) {
    if (stopPhysics || Sprite.IsPlaying("kat_punch_1", "kat_punch_2"))
      return;

    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    Physics.hspeed += 0.5f;
  }

  protected override void JumpPressed () {
    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (HasFooting && !stopPhysics) {
      Physics.vspeed = 8;
      Sprite.Play("Jump");
      (Sprite as Kat_Sprite).PlayLandSound();
      SolidPhysics.Collider.ClearFooting();
    }
  }

  protected override void JumpReleased () {
    if (Sprite.IsPlaying("kat_jump", "kat_gun_jump", "kat_gun_jump_shoot") && Physics.vspeed > 2)
      Physics.vspeed = 2;
  }

  protected override void SkatePressed () {
    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    if (!stopPhysics && HasFooting) {
      DodgeTimer.Enabled = true;
      stopPhysics = true;
      Sprite.Play("Dodge");
      Sound.Play("Dodge");

      if (transform.localScale.x < 0)
        Physics.hspeed = (Stance == eKat_Stance.KATFU ? -7 : -5);
      else
        Physics.hspeed = (Stance == eKat_Stance.KATFU ? 7 : 5);
    }
  }

  protected override void GrindPressed () {
    if (Sprite.IsPlaying("kat_idle", "kat_walk", "kat_gun_idle", "kat_gun_walk")) {
      Sprite.Play("SwitchStance");

      if (Stance == eKat_Stance.KATFU) {
        Stance = eKat_Stance.GUN;
        Sound.Play("GunStart");
      }
      else {
        Stance = eKat_Stance.KATFU;
        Sound.Play("GunEnd");
      }
    }
  }

  protected override void AttackPressed () {
    if (Stance == eKat_Stance.GUN)
      return;

    if (!SolidPhysics.HasFooting) {
      if (Game.DownHeld && !Sprite.IsPlaying("kat_pound"))
        StartPound();
      else if (hasUppercut && Game.UpHeld) {
        if (!Sprite.IsPlaying("kat_uppercut"))
          StartUppercut();
      }
      else if (hasAirAttack)
        StartAirKick();
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
        Sound.Play("Punch");
        Sprite.Play("Punch1");
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

    Sprite.Play("Kick");
    Sound.Play("Kick");
    AirKickTimer.Enabled = true;
  }

  private void StartGroundKick () {
    stopPhysics = true;

    if (transform.localScale.x < 0)
      Physics.hspeed = -7;
    else
      Physics.hspeed = 7;

    Sprite.Play("Kick");
    Sound.Play("Kick");
    GroundKickTimer.Enabled = true;
  }

  private void StartPound () {
    Sprite.Play("GroundPound");
    Sound.Play("Uppercut");
    hasAirAttack = false;
    stopPhysics = true;
    Physics.vspeed = 2f;
  }

  private void StartUppercut () {
    hasUppercut = false;
    GroundKickTimer.Enabled = false;
    stopPhysics = false;
    Sprite.Play("Uppercut");
    Sound.Play("Uppercut");
    Physics.vspeed = 8;
  }

  public void CreateBullet() {
    BaseObj bullet = Game.Create("Bullet", new Vector2(x, Mask.Center.y - 5));
    Sound.Play("Gunshot");

    if (transform.localScale.x > 0)
      bullet.Physics.hspeed = 10;
    else
      bullet.Physics.hspeed = -10;
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
