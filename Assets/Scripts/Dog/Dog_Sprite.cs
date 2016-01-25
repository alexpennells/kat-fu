using UnityEngine;
using System.Collections;

public class Dog_Sprite : SpriteObj {
  public override void Step () {
    TurnSprite();
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
    Animate("dog_idle", 0.25f);
  }
}
