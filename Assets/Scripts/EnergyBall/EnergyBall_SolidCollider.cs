using UnityEngine;

public class EnergyBall_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    if (!(Base as EnergyBall_Base).impacted)
      (Base as EnergyBall_Base).Impact();
  }

  protected override void RoofCollision(SolidObj roof) {
    if (!(Base as EnergyBall_Base).impacted)
      (Base as EnergyBall_Base).Impact();
  }

  protected override void WallCollision(SolidObj wall) {
    if (!(Base as EnergyBall_Base).impacted)
      (Base as EnergyBall_Base).Impact();
  }
}
