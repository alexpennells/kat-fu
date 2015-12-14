using UnityEngine;
using System.Collections;

public class Weed_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    base.FootingCollision(footing);

    if (Base.Sprite.IsPlaying("weed_air"))
      Base.Sprite.Play("Land");
  }
}
