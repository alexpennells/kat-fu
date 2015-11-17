using System;
using UnityEngine;

public class Kat_Sprite : SpriteObj {

  [Tooltip("The speed that the sprite turns around")]
  public float turnSpeed = 0.2f;

  public override void Step() {
    TurnSprite();

    if (Base.SolidPhysics.HasFooting) {
      if (Base.Physics.hspeed == 0)
        Play("kat_idle", 1f);
      else {
        Play("kat_walk");
        SetSpeed(Math.Abs(Base.Physics.hspeed / 4f));
      }
    }
  }

  private void TurnSprite() {
    if (Base.Physics.hspeed < 0 && transform.localScale.x > -1)
      transform.localScale = transform.localScale - new Vector3(turnSpeed, 0, 0);
    else if (Base.Physics.hspeed > 0 && transform.localScale.x < 1)
      transform.localScale = transform.localScale + new Vector3(turnSpeed, 0, 0);
    else if (Base.Physics.hspeed == 0) {
      if (transform.localScale.x > 0 && transform.localScale.x < 1)
        transform.localScale = transform.localScale + new Vector3(turnSpeed, 0, 0);
      else if (transform.localScale.x < 0 && transform.localScale.x > -1)
        transform.localScale = transform.localScale - new Vector3(turnSpeed, 0, 0);
    }

    if (transform.localScale.x > 1)
      transform.localScale = new Vector3(1, 1, 1);
    else if (transform.localScale.x < -1)
      transform.localScale = new Vector3(-1, 1, 1);
  }
}
