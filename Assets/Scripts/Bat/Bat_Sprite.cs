using UnityEngine;
using System.Collections;

public class Bat_Sprite : SpriteObj {
  public override void Step () {
    if (!(Base as Bat_Base).hurt && IsPlaying("bat_hurt"))
      Play("bat_fly");

    TurnSprite();
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Stitch.Kat.x > Base.x)
      transform.localScale = new Vector3(1, 1, 1);
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }
}
