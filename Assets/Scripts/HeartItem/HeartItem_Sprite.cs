using UnityEngine;
using System;
using System.Collections;
using System.Collections.Specialized;

public class HeartItem_Sprite : SpriteObj {
  private bool dyingAnimation = false;

  public override void Step () {
    if (!(Base as HeartItem_Base).Collected)
      StretchForVspeed();

    // Start flickering
    if ((Base as HeartItem_Base).Dying && !dyingAnimation) {
      dyingAnimation = true;
      StartCoroutine("Flicker");
    }
  }

  public void StopFlicker() {
    StopCoroutine("Flicker");
    SetAlpha(1f);
  }

  private void StretchForVspeed() {
    float desiredY = 0.3f * (1 + Base.Physics.vspeed / 2);
    float curY = transform.localScale.y;

    if (desiredY < curY)
      curY -= 0.01f;
    else if (desiredY > curY)
      curY += 0.01f;
    transform.localScale = new Vector3(0.3f, curY, 1);
  }

  /*************************************************************
   * CO-ROUTINES
   ************************************************************/

  protected virtual IEnumerator Flicker() {
    float flickerSpeed = 0.1f;
    for (int i = 0; i < 100; ++i) {
      if (GetAlpha() == 1)
        SetAlpha(0.25f);
      else
        SetAlpha(1f);

      if (i != 0 && i % 10 == 0)
        flickerSpeed -= 0.01f;
      yield return new WaitForSeconds(flickerSpeed);
    }

    Destroy(gameObject);
    yield break;
  }

}
