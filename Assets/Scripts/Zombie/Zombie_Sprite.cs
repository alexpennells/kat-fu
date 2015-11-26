using UnityEngine;
using System.Collections;

public class Zombie_Sprite : SpriteObj {
  public override void Step () {
    if ((Base as Zombie_Base).attacking || (Base as Zombie_Base).hurt)
      return;

    if ((Base as Zombie_Base).InChaseRange()) {
      TurnSprite();
      Play("zombie_walk", 1f);
    }
    else
      Play("zombie_idle", 0.25f);
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Stitch.Kat.x > Base.x)
      transform.localScale = new Vector3(1, 1, 1);
  }

  public void AttackAnimComplete() {
    Play("zombie_idle", 1f);
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }
}
