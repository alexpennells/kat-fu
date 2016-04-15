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

  [Tooltip("Vertical speed of the cat when jumping")]
  public float jumpSpeed = 6;

  [Tooltip("Vertical speed of the cat when doing an uppercut")]
  public float uppercutSpeed = 5;

  [Tooltip("Horizontal speed of the cat when doing a kick")]
  public float kickSpeed = 7;

  [Tooltip("Horizontal speed of the cat when walking in Kat-fu stance")]
  public float walkSpeed = 4;

  [Tooltip("Horizontal speed of the cat when walking in Gun stance")]
  public float gunWalkSpeed = 3;

  private eKat_Stance stance = eKat_Stance.KATFU;
  public eKat_Stance Stance {
    get { return stance; }
    set { stance = value; }
  }

  private SceneExit exit;
  public SceneExit Exit { get { return exit; } set { exit = value; } }

  protected override void LoadReferences() {
    // Don't initialize scene stuff if in edit mode.
    if (!Game.Instance)
      return;

    if (Game.Instance.Entrance) {
      this.DisableInput = true;
      Sprite.SetLayer(-z + 10);
      Sprite.Play("Walk");
      Sprite.mirrorToStart = Game.Instance.Entrance.StartPosition.x < Game.Instance.Entrance.Position.x;
      if (Game.Instance.Entrance.isDoor)
        Sprite.SetAlpha(0f);

      Position = Game.Instance.Entrance.Position + Vector3.up * Mask.LocalBottom;
      MoveTo(Game.Instance.Entrance.StartPosition + Vector3.up * Mask.LocalBottom, 20, "WalkInStep", "WalkInComplete");
    }

    if (Game.Camera) {
      Game.Camera.ViewObj = this;
      Game.Camera.JumpToPosition();
    }
  }

  protected override void Init() {
    AirKickTimer.Interval = 300;
    GroundKickTimer.Interval = 400;
    DodgeTimer.Interval = 300;
    InvincibleTimer.Interval = 1000;
  }

  protected override void Step () {
    if (Is("Hurt")) {
      if (Physics.hspeed == 0 && Stitch.katHealth > 0)
        Sprite.Play("Recover");
      return;
    }

    base.Step();

    if (stopPhysics) {
      Physics.SkipNextFrictionUpdate();
      Physics.SkipNextGravityUpdate();
      Physics.hspeedMax = this.kickSpeed;
    } else {
      if (Stance == eKat_Stance.KATFU)
        Physics.hspeedMax = this.walkSpeed;
      else if (Stance == eKat_Stance.GUN) {
        if (!Game.AttackHeld)
          Physics.hspeedMax = this.gunWalkSpeed;
        else
          Physics.hspeedMax = this.gunWalkSpeed - 1;
      }
    }

    if (!stopPhysics && !Is("Uppercutting") && !Is("GroundPounding") && !Is("Punching"))
      Sprite.StopBlur();
  }

  public void WalkInStep(float t) {
    if (Game.Instance.Entrance && Game.Instance.Entrance.isDoor) {
      Sprite.SetAlpha(t);
    }
  }

  public void WalkInComplete() {
    this.DisableInput = false;
  }

  public void WalkOutStep(float t) {
    if (Exit.isDoor) {
      Sprite.SetAlpha(1 - t);
    }
  }

  public void WalkOutComplete() {
    Game.ChangeScene(Exit.sceneName, Exit.exitID, "KatHead");
  }

  /***********************************
   * INPUT HANDLERS
   **********************************/

  protected override void LeftHeld (float val) {
    if (stopPhysics || Is("Punching") || SolidPhysics.InWall)
      return;

    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    Physics.hspeed -= 0.5f;
    if (!(Stance == eKat_Stance.GUN && Game.DownHeld))
      Sprite.FacingLeft = true;
  }

  protected override void RightHeld (float val) {
    if (stopPhysics || Is("Punching") || SolidPhysics.InWall)
      return;

    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end"))
      return;

    Physics.hspeed += 0.5f;

    if (!(Stance == eKat_Stance.GUN && Game.DownHeld))
      Sprite.FacingRight = true;
  }

  protected override void JumpPressed () {
    if (Sprite.IsPlaying("kat_gun_start", "kat_gun_end") || SolidPhysics.InWall)
      return;

    if (HasFooting && !stopPhysics) {
      Physics.vspeed = this.jumpSpeed;
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

    if (Stitch.canDodge && !stopPhysics && HasFooting) {
      DodgeTimer.Enabled = true;
      stopPhysics = true;
      Sprite.Play("Dodge");
      Sound.Play("Dodge");

      if (Sprite.FacingLeft)
        Physics.hspeed = (Stance == eKat_Stance.KATFU ? -this.walkSpeed : -this.gunWalkSpeed) - 2;
      else
        Physics.hspeed = (Stance == eKat_Stance.KATFU ? this.walkSpeed : this.gunWalkSpeed) + 2;
    }
  }

  protected override void GrindPressed () {
    if (Stitch.canUseGun && Sprite.IsPlaying("kat_idle", "kat_walk", "kat_gun_idle", "kat_gun_walk")) {
      Sprite.Play("SwitchStance");

      if (Stance == eKat_Stance.KATFU) {
        Stance = eKat_Stance.GUN;
        Sound.Play("GunStart");
      } else {
        Stance = eKat_Stance.KATFU;
        Sound.Play("GunEnd");
      }
    }
  }

  protected override void AttackPressed () {
    if (Stance == eKat_Stance.GUN)
      return;

    if (!SolidPhysics.HasFooting) {
      if (Stitch.canGroundPound && Game.DownHeld && !Sprite.IsPlaying("kat_pound"))
        State("GroundPound");
      else if (Stitch.canUppercut && Game.UpHeld && hasUppercut) {
        if (!Sprite.IsPlaying("kat_uppercut"))
          State("Uppercut");
      }
      else if (Stitch.canKick && hasAirAttack)
        State("AirKick");
    }

    if (SolidPhysics.HasFooting && !Sprite.IsPlaying("kat_uppercut")) {
      if (Stitch.canUppercut && Game.UpHeld) {
        State("Uppercut");
        return;
      }

      if (stopPhysics)
        return;

      if (Stitch.canKick && (Game.LeftHeld || Game.RightHeld)) {
        State("GroundKick");
        return;
      }

      if (Is("Punching"))
        (Sprite as Kat_Sprite).KeepPunching = true;
      else
        Sprite.Play("Punch");
    }
  }

  public void CreateBullet() {
    BaseObj bullet = Game.Create("Bullet", new Vector2(x, Mask.Center.y - 5));
    bullet.z = z;
    Sound.Play("Gunshot");

    if (Sprite.FacingRight)
      bullet.Physics.hspeed = 10;
    else
      bullet.Physics.hspeed = -10;
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateHurt(float hspeed) {
    stopPhysics = false;
    AirKickTimer.Enabled = false;
    GroundKickTimer.Enabled = false;
    hasAirAttack = true;
    hasUppercut = true;

    Physics.hspeedMax = this.walkSpeed;
    Physics.vspeed = 0;
    Physics.hspeed = hspeed;
    if (hspeed < 0)
      Sprite.FacingRight = true;
    else
      Sprite.FacingLeft = true;

    Sound.Play("Hurt");
    Sprite.Play("Hurt");
  }

  public void StateAirKick() {
    hasAirAttack = false;
    stopPhysics = true;
    Physics.vspeed = 0;

    if (Sprite.FacingLeft)
      Physics.hspeed = -this.kickSpeed;
    else
      Physics.hspeed = this.kickSpeed;

    Sprite.Play("Kick");
    Sound.Play("Kick");
    AirKickTimer.Enabled = true;
  }

  public void StateGroundKick () {
    stopPhysics = true;

    if (Sprite.FacingLeft)
      Physics.hspeed = -this.kickSpeed;
    else
      Physics.hspeed = this.kickSpeed;

    Sprite.Play("Kick");
    Sound.Play("Kick");
    GroundKickTimer.Enabled = true;
  }

  public void StateGroundPound () {
    Sprite.Play("GroundPound");
    Sound.Play("Uppercut");
    hasAirAttack = false;
    stopPhysics = true;
    Physics.vspeed = 2f;
  }

  public void StateUppercut () {
    hasUppercut = false;
    GroundKickTimer.Enabled = false;
    stopPhysics = false;
    Sprite.Play("Uppercut");
    Sound.Play("Uppercut");
    Physics.vspeed = this.uppercutSpeed;
    Game.CreateParticle("JumpRipple", Mask.Center);
  }

  public void StateInvincible() {
    Sprite.SetAlpha(0.5f);
    Sprite.Play("Idle");
    InvincibleTimer.Enabled = true;
    preventAlphaChange = true;
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsHurt() { return Sprite.IsPlaying("kat_hurt", "kat_recover", "kat_gun_hurt", "kat_gun_recover"); }
  public bool IsPunching() { return Sprite.IsPlaying("kat_punch"); }
  public bool IsKicking() { return Sprite.IsPlaying("kat_kick", "kat_lunge"); }
  public bool IsGroundPounding() { return Sprite.IsPlaying("kat_pound") && Physics.vspeed < 0; }
  public bool IsUppercutting() { return Sprite.IsPlaying("kat_uppercut") && Physics.vspeed > 0; }
  public bool IsInvincible() { return Sprite.GetAlpha() < 0.8f; }

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
