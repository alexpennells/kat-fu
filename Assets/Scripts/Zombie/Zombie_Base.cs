using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Zombie_Base : BaseObj {
  public bool attacking = false;

  protected override void Init() {
    AttackTimer.Interval = 300;
  }

  protected override void Step() {
    if (InAttackRange() || attacking) {
      if (!attacking && FacingKat()) {
        Sprite.Play("zombie_attack", 1.5f);
        attacking = true;
        AttackTimer.Enabled = true;
      }

      Physics.hspeedMax = 4;
      if (transform.localScale.x > 0)
        Physics.hspeed += 0.25f;
      else
        Physics.hspeed -= 0.25f;
    }
    else if (InChaseRange()) {
      Physics.SkipNextFrictionUpdate();
      Physics.hspeedMax = 3;

      if (x < Stitch.Kat.x)
        Physics.hspeed += 0.05f;
      else if (x > Stitch.Kat.x)
        Physics.hspeed -= 0.05f;
    }
  }

  protected override void OffScreenStep() {
    Physics.hspeed = 0;
    AttackTimer.Enabled = false;
    attacking = false;
  }

  public bool InChaseRange () {
    if (Stitch.Kat == null)
      return false;
    return Math.Abs(Stitch.Kat.x - x) < 250;
  }

  public bool InAttackRange () {
    if (Stitch.Kat == null)
      return false;
    return Math.Abs(Stitch.Kat.x - x) < 40;
  }

  public bool FacingKat () {
    return (transform.localScale.x > 0 && Stitch.Kat.x > x) || (transform.localScale.x < 0 && Stitch.Kat.x < x);
  }

  public Timer AttackTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    AttackTimer.Enabled = false;
    attacking = false;
  }

}
