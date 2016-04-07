using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Mouse_Base : BaseObj {
  [Tooltip("Whether this mouse is attacking or running from the cat")]
  public bool runAway = false;

  protected override void Step() {
    if (runAway)
      RunAway();
    else
      Attack();
  }

  private void RunAway() {
    if (x < Stitch.Kat.x && Stitch.Kat.x < x + 75) {
      Physics.hspeed -= 0.25f;
    } else if (x > Stitch.Kat.x && Stitch.Kat.x > x - 75) {
      Physics.hspeed += 0.25f;
    }

    if (Math.Abs(Stitch.Kat.x - x) > 75) {
      Physics.friction = 0.25f;
    } else {
      Physics.friction = 0.05f;
    }
  }

  private void Attack() {
    if (Stitch.Kat.x > x + 75) {
      Physics.hspeed += 0.1f;
    } else if (Stitch.Kat.x < x - 75) {
      Physics.hspeed -= 0.1f;
    }

    if (Math.Abs(Stitch.Kat.x - x) < 75) {
      Physics.friction = 0.25f;
    } else {
      Physics.friction = 0.05f;
    }
  }
}
