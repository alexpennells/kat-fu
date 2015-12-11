using UnityEngine;

public class Kat_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    if (Base.Physics.vspeed != 0)
      Base.Sound.Play("Land");

    base.FootingCollision(footing);

    (Base as Kat_Base).hasAirAttack = true;
    (Base as Kat_Base).hasUppercut = true;

    if (Base.Sprite.IsPlaying("kat_uppercut")) {
      SpriteObj katSprite = Base.Sprite;
      (katSprite as Kat_Sprite).PlayIdle();
    }
  }
}
