using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class EnergyBall_Base : BaseObj {
  public bool impacted = false;

  public void Impact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    impacted = true;
    Sprite.Play("Die");
  }
}
