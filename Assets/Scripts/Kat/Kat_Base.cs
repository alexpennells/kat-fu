using System;
using UnityEngine;

public class Kat_Base : InputObj {

  [Tooltip("The speed that the sprite turns around")]
  public float turnSpeed = 0.2f;

  protected override void Init() {
    Game.Camera.InitPosition();
  }

  protected override void Step() {
    base.Step();

    if (Physics.hspeed < 0 && transform.localScale.x > -1)
      transform.localScale = transform.localScale - new Vector3(turnSpeed, 0, 0);
    else if (Physics.hspeed > 0 && transform.localScale.x < 1)
      transform.localScale = transform.localScale + new Vector3(turnSpeed, 0, 0);
    else if (Physics.hspeed == 0) {
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

  protected override void LeftHeld (float val) {
    Physics.hspeed -= 0.5f;
  }

  protected override void RightHeld (float val) {
    Physics.hspeed += 0.5f;
  }

  protected override void JumpPressed () {
    if (SolidPhysics.HasFooting)
      Physics.vspeed = 8;
  }
}
