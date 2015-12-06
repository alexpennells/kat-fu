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
      Sprite.Play("weed_hurt", 1f);
      HurtTimer.Enabled = true;
    }
    else
      Sprite.Play("weed_die", 1f);
  }

  protected override void Init () {
    HurtTimer.Interval = 800;

    if (attackInterval != 0) {
      AttackTimer.Interval = attackInterval;
      AttackTimer.Enabled = true;
    }

    Sprite.Play("weed_idle", 0.5f);
  }

  protected override void Step () {
    if (Sprite.IsPlaying("weed_hurt", "weed_die", "weed_jump", "weed_land", "weed_air"))
      return;

    if (VectorLib.GetDistanceX(this, Stitch.Kat) < 48f && Stitch.Kat.Mask.Bottom > Mask.Top) {
      Sprite.Play("weed_jump", 2f);
      AttackTimer.Enabled = false;
      return;
    }
    else if (VectorLib.GetDistance(this, Stitch.Kat) < biteRange) {
      Sprite.Play("weed_bite", 1.5f);
      AttackTimer.Enabled = false;
      return;
    }
    else if (Sprite.IsPlaying("weed_bite")) {
      Sprite.Play("weed_idle", 0.5f);
      AttackTimer.Enabled = true;
      attackOnNextStep = false;
    }

    if (attackOnNextStep) {
      Sprite.Play("weed_spit", 1f);
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
