using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class HeartItem_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    base.FootingCollision(footing);
    Base.Physics.vspeed = 0.5f;
  }
}
