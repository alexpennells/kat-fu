// Since C# is shitty at generic types, the object types need to be defined
// per project to create simple stubs for them at the collision level.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionStubs : CollisionObj
{
  /***********************************
   * FUNCTIONS
   **********************************/

  // Event handler that gets fired from the CollisionHandler instance attached to the BoxCollider/Rigidbody.
  new public virtual void HandleCollision(BaseObj other) {
    switch (other.Collision.Type) {
      case eObjectType.LADDER:
        LadderCollision(other as LadderObj);
        break;
      case eObjectType.KAT:
        KatCollision(other as Kat_Base);
        break;
      case eObjectType.ZOMBIE:
        ZombieCollision(other as Zombie_Base);
        break;
    }
  }

  new public virtual void HandleExitCollision(BaseObj other) {
    switch (other.Collision.Type) {
      case eObjectType.LADDER:
        LadderExit(other as LadderObj);
        break;
    }
  }

  /***********************************
   * FUNCTION STUBS
   **********************************/

  protected virtual void LadderCollision(LadderObj other) {}
  protected virtual void LadderExit(LadderObj other) {}

  protected virtual void KatCollision(Kat_Base other) {}
  protected virtual void ZombieCollision(Zombie_Base other) {}

}
