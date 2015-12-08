using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Bullet_Base : BaseObj {
  public bool impacted = false;

  public void Impact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    impacted = true;
    DestroySelf();
    // Sprite.Play("energy_die", 1f);
  }

  protected override void OffScreenStep() {
    DestroySelf();
  }
}
