using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Glob_Base : BaseObj {

  public int jumpDelay = 4000;
  private bool canJump = true;

  public bool hurt = false;
  public int health = 50;

  protected override void Init () {
    HurtTimer.Interval = 250;
    JumpTimer.Interval = jumpDelay;
    JumpTimer.Enabled = true;
  }

  protected override void Step () {
    if (!hurt && Sprite.IsPlaying("glob_hurt"))
      StopJump();

    if (Sprite.IsPlaying("glob_land", "glob_attack_land", "glob_idle"))
      Physics.hspeed = 0;

    if (canJump) {
      Sprite.Play("Jump");
      canJump = false;
    }
  }

  public void GetHurt(int damage = 10) {
    HurtTimer.Enabled = false;
    health -= damage;
    hurt = true;

    if (health > 0) {
      Sprite.Play("Hurt");
      HurtTimer.Enabled = true;
    }
    else
      Sprite.Play("Die");
  }

  public void StartJump () {
    if (Sprite.IsPlaying("glob_jump")) {
      Physics.hspeed = (transform.localScale.x > 0) ? 3 : -3;
      Physics.vspeed = 6;
    }
    else {
      Physics.hspeed = (transform.localScale.x > 0) ? 6 : -6;
      Physics.vspeed = 5;
    }
  }

  public void StopJump() {
    JumpTimer.Enabled = false;
    Sprite.Play("Idle");

    if (VectorLib.GetDistanceX(this, Stitch.Kat) <= 100)
      canJump = true;
    else
      JumpTimer.Enabled = true;
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer JumpTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    JumpTimer.Enabled = false;
    canJump = true;
  }

  public Timer HurtTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;
    hurt = false;
  }

}
