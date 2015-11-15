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
      case eObjectType.PLAYER:
        PlayerCollision(other);
        break;
      case eObjectType.LADDER:
        LadderCollision(other as LadderObj);
        break;
      case eObjectType.SPROUT:
        SproutCollision((Sprout)other);
        break;
      case eObjectType.SCENE_EXIT:
        SceneExitCollision((SceneExit)other);
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

  protected virtual void PlayerCollision(BaseObj other) {}
  protected virtual void LadderCollision(LadderObj other) {}
  protected virtual void LadderExit(LadderObj other) {}
  protected virtual void SproutCollision(Sprout other) {}
  protected virtual void SceneExitCollision(SceneExit other) {}
}
