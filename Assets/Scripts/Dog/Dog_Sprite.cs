using UnityEngine;
using System;
using System.Collections;

public class Dog_Sprite : SpriteObj {
  public override void Step () {
    TurnSprite();

    if (Base.Physics.hspeed == 0)
      PlayIdle();
    else
      PlayWalk();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      FacingLeft = true;
    else if (Stitch.Kat.x > Base.x)
      FacingRight = true;
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    Animate("dog_idle", 0.5f);
  }

  public void PlayWalk() {
    Animate("dog_walk", Math.Abs(Base.Physics.hspeed / 4f));
  }
}
