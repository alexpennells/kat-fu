using UnityEngine;

public class Kat_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    base.FootingCollision(footing);

    (Base as Kat_Base).hasAirAttack = true;
    (Base as Kat_Base).hasUppercut = true;

    if (Base.Sprite.IsPlaying("kat_jump", "kat_kick", "kat_pound", "kat_uppercut"))
      Base.Sprite.Play("kat_idle", 1f);
  }
}
