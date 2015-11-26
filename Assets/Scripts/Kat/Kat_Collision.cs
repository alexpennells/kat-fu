using UnityEngine;

public class Kat_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.KAT; } }

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  protected override void ZombieCollision(Zombie_Base other) {

    if ((Kat.Punching && PunchSuccess(other)) || (Kat.Kicking && KickSuccess(other))) {
      other.GetHurt();
      if (transform.localScale.x > 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      if (Kat.Kicking)
        BounceOffEnemy();
    }

    if (Kat.GroundPounding) {
      other.GetHurt();

      if (other.transform.localScale.x < 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    if (Kat.Uppercutting) {
      other.GetHurt();

      if (Kat.x > other.x)
        other.Physics.hspeed = -other.Physics.hspeedMax;
      else
        other.Physics.hspeed = other.Physics.hspeedMax;

      BounceOffEnemy(8, false);
    }
  }

  // Whether or not Punchin is facing the enemy object when punching/kicking.
  private bool PunchSuccess (BaseObj other) {
    return ((transform.localScale.x > 0 && other.x > Kat.x) || (transform.localScale.x < 0 && other.x < Kat.x));
  }

  private bool KickSuccess (BaseObj other) {
    return ((transform.localScale.x > 0 && (other.x + 20) > Kat.x) || (transform.localScale.x < 0 && (other.x - 20) < Kat.x));
  }

  private void BounceOffEnemy (float newVspeed = 6, bool changeHspeed = true) {
    Kat.stopPhysics = false;
    Kat.AirKickTimer.Enabled = false;
    Kat.GroundKickTimer.Enabled = false;
    Kat.hasAirAttack = true;

    Kat.Physics.vspeed = newVspeed;
    if (changeHspeed) {
      if (transform.localScale.x > 0)
        Kat.Physics.hspeed = -4;
      else
        Kat.Physics.hspeed = 4;
    }

    Base.Sprite.Play("kat_spin", 2f);
  }
}
