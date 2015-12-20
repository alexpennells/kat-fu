using UnityEngine;
using System.Collections.Generic;

public class Glob_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    if (Base.Physics.vspeed != 0 && Base.Sprite.IsPlaying("glob_jump", "glob_attack"))
      Base.Sprite.Play("Land");

    base.FootingCollision(footing);
  }
}
