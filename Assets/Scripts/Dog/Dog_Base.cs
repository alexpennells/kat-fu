using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Dog_Base : BaseObj {
  protected override void Step() {
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
