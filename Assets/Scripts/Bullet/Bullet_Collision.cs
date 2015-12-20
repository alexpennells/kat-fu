public class Bullet_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.BULLET; } }

  protected override void ZombieCollision(Zombie_Base other) {
    other.GetHurt(2);

    if (Base.Physics.hspeed > 0)
      other.Physics.hspeed += 0.75f;
    else
      other.Physics.hspeed -= 0.75f;

    Base.DestroySelf();
  }

  protected override void BatCollision(Bat_Base other) {
    other.GetHurt(5);

    if (other.Physics.vspeed > 1)
      other.Physics.vspeed = 1;
    else if (other.Physics.vspeed < -1)
      other.Physics.vspeed = -1;

    if (Base.Physics.hspeed > 0)
      other.Physics.hspeed += 0.75f;
    else
      other.Physics.hspeed -= 0.75f;

    Base.DestroySelf();
  }

  protected override void WeedCollision(Weed_Base other) {
    other.GetHurt(10);

    if (!other.HasFooting) {
      if (Base.Physics.hspeed > 0)
        other.Physics.hspeed += 1f;
      else
        other.Physics.hspeed -= 1f;
    }

    Base.DestroySelf();
  }

  protected override void GlobCollision(Glob_Base other) {
    other.GetHurt(5);

    if (Base.Physics.hspeed > 0)
      other.Physics.hspeed += 0.75f;
    else
      other.Physics.hspeed -= 0.75f;

    Base.DestroySelf();
  }

}
