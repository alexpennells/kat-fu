public class Manhole_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.MANHOLE; } }

  protected override void KatCollision(Kat_Base other) {
    if (other.Physics.vspeed > 0 && !Base.Physics.Active)
      (Base as Manhole_Base).StartSpinning();
  }
}
