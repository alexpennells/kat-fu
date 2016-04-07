using UnityEngine;
using System;
using System.Collections;

public class Mouse_Sprite : SpriteObj {
  public override void Step () {
    TurnSprite();

    if (Base.Physics.hspeed == 0)
      PlayIdle();
    else
      PlayRun();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      FacingLeft = true;
    else if (Stitch.Kat.x > Base.x)
      FacingRight = true;

    if (Base.Physics.hspeed > 3 && FacingLeft)
      FacingRight = true;
    else if (Base.Physics.hspeed < -3 && FacingRight)
      FacingLeft = true;
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    Animate("mouse_idle", 0.5f);
  }

  public void PlayRun() {
    Animate("mouse_run", 1.4f - Math.Abs((Base.Physics.hspeed / 8f)));
  }
}
