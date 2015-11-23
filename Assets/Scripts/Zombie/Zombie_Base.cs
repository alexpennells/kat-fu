using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Zombie_Base : BaseObj {
  private bool preventAttack = false;

  protected override void Init() {
    PreventAttackTimer.Interval = 200;
  }

  protected override void Step() {
    base.Step();

    if (Sprite.IsPlaying("zombie_attack"))
      return;

    Physics.SkipNextFrictionUpdate();

    if (InChaseRange()) {
      if (x < Stitch.Kat.x)
        Physics.hspeed += 0.05f;
      else if (x > Stitch.Kat.x)
        Physics.hspeed -= 0.05f;
    }

    if (!preventAttack && Math.Abs(Physics.hspeed) > 1 && InAttackRange()) {
      Sprite.Play("zombie_attack", 1f);
      preventAttack = true;
    }
  }

  public bool InChaseRange () {
    if (Stitch.Kat == null)
      return false;
    return Math.Abs(Stitch.Kat.x - x) < 250;
  }

  public bool InAttackRange () {
    if (Stitch.Kat == null)
      return false;
    return Math.Abs(Stitch.Kat.x - x) < 50;
  }

  public Timer PreventAttackTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    PreventAttackTimer.Enabled = false;
    preventAttack = false;
  }

}
