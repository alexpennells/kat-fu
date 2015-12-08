using UnityEngine;

public class Bullet_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    if (!(Base as Bullet_Base).impacted)
      (Base as Bullet_Base).Impact();
  }

  protected override void RoofCollision(SolidObj roof) {
    if (!(Base as Bullet_Base).impacted)
      (Base as Bullet_Base).Impact();
  }

  protected override void WallCollision(SolidObj wall) {
    if (!(Base as Bullet_Base).impacted)
      (Base as Bullet_Base).Impact();
  }
}
