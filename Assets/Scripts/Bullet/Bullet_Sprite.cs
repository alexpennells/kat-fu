using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Bullet_Sprite : SpriteObj {
  public override void Step () {
    if (Base.Physics.hspeed > 0)
      transform.localScale = new Vector3(1, 1, 1);
    else
      transform.localScale = new Vector3(-1, 1, 1);
  }
}