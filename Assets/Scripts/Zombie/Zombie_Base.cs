﻿using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Zombie_Base : BaseObj {
  public bool attacking = false;
  public bool hurt = false;
  public int health = 50;

  protected override void Init() {
    AttackTimer.Interval = 300;
    HurtTimer.Interval = 250;
  }

  protected override void Step() {
    if (hurt) {
      Physics.SkipNextFrictionUpdate();
      Physics.hspeedMax = 1;
      return;
    }

    Physics.hspeedMax = 3;

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

  public void GetHurt(int damage = 10) {
    HurtTimer.Enabled = false;
    health -= damage;
    hurt = true;

    if (health > 0) {
      Sprite.Play("zombie_hurt", 1f);
      HurtTimer.Enabled = true;
    }
    else
      Sprite.Play("zombie_die", 1f);
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

  public Timer HurtTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;
    hurt = false;
  }

}
