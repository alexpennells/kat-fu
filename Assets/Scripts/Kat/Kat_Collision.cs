using UnityEngine;

public class Kat_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.KAT; } }

  protected override void ZombieCollision(Zombie_Base other) {
    Debug.Log("zombie collision");
  }
}
