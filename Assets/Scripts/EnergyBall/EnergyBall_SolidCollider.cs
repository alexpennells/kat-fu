using UnityEngine;
using System.Collections.Generic;

public class EnergyBall_SolidCollider : SolidColliderObj {
  protected override bool HandleAllCollisions(List<SolidObj> walls, List<SolidObj> roofs, List<SolidObj> footings) {
    if (walls.Count > 0 || roofs.Count > 0 || footings.Count > 0) {
      if (!(Base as EnergyBall_Base).impacted)
        (Base as EnergyBall_Base).Impact();
      return false;
    }

    return true;
  }
}
