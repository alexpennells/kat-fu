using System;
using System.Timers;
using UnityEngine;

public class Kat_Base : InputObj {

  // Whehter the air kick is still up.
  public bool hasAirAttack = true;

  // The timer for the air kick.
  public Timer airKickTimer = new Timer();

  protected override void Init() {
    Game.Camera.InitPosition();

    airKickTimer.Elapsed += new ElapsedEventHandler(AirKickTimerElapsed);
    airKickTimer.Interval = 500;
  }

  protected override void Step () {
    base.Step();

    if (!hasAirAttack && airKickTimer.Enabled) {
      Physics.SkipNextFrictionUpdate();
      Physics.SkipNextGravityUpdate();
    }
  }

  protected override void LeftHeld (float val) {
    Physics.hspeed -= 0.5f;
  }

  protected override void RightHeld (float val) {
    Physics.hspeed += 0.5f;
  }

  protected override void JumpPressed () {
    if (SolidPhysics.HasFooting) {
      Physics.vspeed = 8;
      Sprite.Play("kat_jump", 1f);
      airKickTimer.Enabled = true;
    }
  }

  protected override void AttackPressed () {
    if (hasAirAttack && !SolidPhysics.HasFooting) {
      hasAirAttack = false;
      Physics.vspeed = 0;

      if (transform.localScale.x < 0)
        Physics.hspeed = -5;
      else
        Physics.hspeed = 5;

      Sprite.Play("kat_kick", 2f);
    }
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  private void AirKickTimerElapsed(object source, ElapsedEventArgs e) {
      airKickTimer.Enabled = false;
  }
}
