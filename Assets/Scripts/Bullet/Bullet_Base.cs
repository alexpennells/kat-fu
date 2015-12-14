using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Bullet_Base : BaseObj {
  public bool impacted = false;

  private bool deletedFlash = false;

  public void Impact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    impacted = true;
    DestroySelf();
  }

  protected override void Step () {
    if (!deletedFlash && Math.Abs(Stitch.Kat.x - x) > 100) {
      deletedFlash = true;
      transform.Find("Flash").gameObject.SetActive(false);
    }
  }

  protected override void OffScreenStep() {
    DestroySelf();
  }
}
