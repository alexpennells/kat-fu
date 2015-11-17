using UnityEngine;

public class Kat_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    (Base as Kat_Base).hasAirAttack = true;
    (Base as Kat_Base).airKickTimer.Enabled = false;

    if (Base.Sprite.IsPlaying("kat_jump") || Base.Sprite.IsPlaying("kat_kick"))
      Base.Sprite.Play("kat_idle", 1f);
  }
}
