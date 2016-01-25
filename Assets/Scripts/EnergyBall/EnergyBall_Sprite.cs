using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class EnergyBall_Sprite : SpriteObj {

  public override void Step () {
    if (Base.Physics.hspeed > 0)
      FacingRight = true;
    else
      FacingLeft = true;
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayDie() {
    Animate("energy_die", 1f);
  }
}
