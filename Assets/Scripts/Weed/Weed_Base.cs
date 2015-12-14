using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Weed_Base : BaseObj {

  public float biteRange = 120f;

  // The interval at which this guy will spit shit. If 0 he won't spit at all!
  public int attackInterval = 0;

  public bool hurt = false;
  public int health = 50;

  private bool attackOnNextStep = false;

  public void GetHurt (int damage = 10) {
    attackOnNextStep = false;
    AttackTimer.Enabled = false;
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

  protected override void Init () {
    HurtTimer.Interval = 250;

    if (attackInterval != 0) {
      AttackTimer.Interval = attackInterval;
      AttackTimer.Enabled = true;
    }

    Sprite.Play("Idle");
  }

  protected override void Step () {
    if (Sprite.IsPlaying("weed_hurt", "weed_die", "weed_jump", "weed_land", "weed_air"))
      return;

    if (VectorLib.GetDistanceX(this, Stitch.Kat) < 48f && Stitch.Kat.Mask.Bottom > Mask.Top) {
      Sprite.Play("Jump");
      AttackTimer.Enabled = false;
      return;
    }
    else if (VectorLib.GetDistance(this, Stitch.Kat) < biteRange) {
      Sprite.Play("Bite");
      AttackTimer.Enabled = false;
      return;
    }
    else if (Sprite.IsPlaying("weed_bite")) {
      Sprite.Play("Idle");
      AttackTimer.Enabled = true;
      attackOnNextStep = false;
    }

    if (attackOnNextStep) {
      Sprite.Play("Spit");
      attackOnNextStep = false;
      return;
    }
  }

  public Timer HurtTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;

    if (attackInterval != 0)
      AttackTimer.Enabled = true;

    hurt = false;
  }

  public Timer AttackTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    AttackTimer.Enabled = false;
    attackOnNextStep = true;
  }

}
