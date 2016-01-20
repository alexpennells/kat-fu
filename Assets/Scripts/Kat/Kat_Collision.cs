using UnityEngine;

public class Kat_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.KAT; } }

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  protected override void ZombieCollision(Zombie_Base other) {
    if (other.Sprite.IsPlaying("zombie_die"))
      return;

    if (GroundAttackSuccess(other)) {
      other.GetHurt();
      if (transform.localScale.x > 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      BounceOffEnemy(6, Kat.Kicking);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();

      if (other.transform.localScale.x < 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Uppercutting) {
      other.GetHurt();

      if (Kat.x > other.x)
        other.Physics.hspeed = -other.Physics.hspeedMax;
      else
        other.Physics.hspeed = other.Physics.hspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);
    }
  }

  protected override void BatCollision(Bat_Base other) {
    if (other.Sprite.IsPlaying("bat_die"))
      return;

    if (GroundAttackSuccess(other)) {
      other.GetHurt();

      if (transform.localScale.x > 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      other.Physics.vspeed = (other.Mask.Center.y - Base.Mask.Center.y) / 2;

      BounceOffEnemy(6, Kat.Kicking);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();

      other.Physics.hspeed = 0;
      other.Physics.vspeed = -other.Physics.vspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Uppercutting) {
      other.GetHurt();

      other.Physics.hspeed = (other.x - Base.x) / 2;
      other.Physics.vspeed = other.Physics.vspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);
    }
  }

  protected override void WeedCollision(Weed_Base other) {
    if (other.Sprite.IsPlaying("weed_die"))
      return;

    if (!other.Sprite.IsPlaying("weed_bite") && GroundAttackSuccess(other)) {
      other.GetHurt();

      if (transform.localScale.x > 0)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      other.Physics.vspeed = (other.Mask.Center.y - Base.Mask.Center.y) / 2;

      BounceOffEnemy(6, Kat.Kicking);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();

      other.Physics.hspeed = 0;
      other.Physics.vspeed = -other.Physics.vspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (!other.Sprite.IsPlaying("weed_bite") && Kat.Uppercutting) {
      other.GetHurt();

      other.Physics.hspeed = (other.x - Base.x) / 2;
      other.Physics.vspeed = other.Physics.vspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);
    }
  }

  protected override void GlobCollision(Glob_Base other) {
    if (other.Sprite.IsPlaying("glob_die"))
      return;

    if (GroundAttackSuccess(other)) {
      other.GetHurt();
      if (Base.Physics.hspeed > 0)
        other.Physics.hspeed = 4;
      else
        other.Physics.hspeed = -4;

      BounceOffEnemy(6, Kat.Kicking);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();

      other.Physics.vspeed = -2;
      if (other.transform.localScale.x < 0)
        other.Physics.hspeed = 2;
      else
        other.Physics.hspeed = -2;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Uppercutting) {
      other.GetHurt();

      if (Kat.x > other.x)
        other.Physics.hspeed = -2;
      else
        other.Physics.hspeed = 2;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);
    }
  }

  protected override void RecyclingCollision(Recycling_Base other) {
    if (GroundAttackSuccess(other)) {
      other.GetHurt();
      other.Physics.hspeed = (Base.transform.localScale.x > 0) ? 4 : -4;

      if (Kat.Kicking)
        BounceOffEnemy(6);
      else
        BounceOffEnemy(0, false);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();

      other.Physics.vspeed = -2;
      other.Physics.hspeed = (other.transform.localScale.x < 0) ? 2 : -2;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Uppercutting) {
      other.GetHurt();
      other.Physics.hspeed = (Kat.x > other.x) ? -2 : 2;

      BounceOffEnemy(12, false);
    }

    else if (!other.Hurt && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);
    }
  }

  protected override void BoxSpawnerCollision(BoxSpawner_Base other) {
    if (GroundAttackSuccess(other)) {
      other.GetHurt();
      other.Physics.hspeed = (Base.transform.localScale.x > 0) ? 4 : -4;
      BounceOffEnemy(5);
    }

    else if (Kat.GroundPounding) {
      other.GetHurt();
      other.Physics.hspeed = (other.transform.localScale.x < 0) ? 2 : -2;
      BounceOffEnemy(5);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Uppercutting) {
      other.GetHurt();
      other.Physics.hspeed = (Kat.x > other.x) ? -2 : 2;
      BounceOffEnemy(6, false);
      Kat.Physics.hspeed = (Kat.x > other.x) ? 4 : -4;
    }

    else {
      BounceOffEnemy(5, false);
      Kat.Physics.hspeed = (Kat.x > other.x) ? 4 : -4;
    }
  }

  protected override void EnergyBallCollision(EnergyBall_Base other) {
    if (!other.impacted && !Kat.Invincible && !Kat.Hurt) {
      if (other.x > Kat.x)
        Kat.GetHurt(-5);
      else
        Kat.GetHurt(5);

      other.Impact();
    }
  }

  // Whether or not Punchin is facing the enemy object when punching/kicking.
  private bool PunchSuccess (BaseObj other) {
    return ((transform.localScale.x > 0 && other.x > Kat.x) || (transform.localScale.x < 0 && other.x < Kat.x));
  }

  private bool KickSuccess (BaseObj other) {
    return ((transform.localScale.x > 0 && (other.x + 20) > Kat.x) || (transform.localScale.x < 0 && (other.x - 20) < Kat.x));
  }

  private bool GroundAttackSuccess (BaseObj other) {
    return (Kat.Punching && PunchSuccess(other)) || (Kat.Kicking && KickSuccess(other));
  }

  private void BounceOffEnemy (float newVspeed = 6, bool changeHspeed = true) {
    Kat.stopPhysics = false;
    Kat.AirKickTimer.Enabled = false;
    Kat.GroundKickTimer.Enabled = false;
    Kat.hasAirAttack = true;
    Kat.hasUppercut = true;

    Kat.Physics.vspeed = newVspeed;
    if (changeHspeed) {
      if (transform.localScale.x > 0)
        Kat.Physics.hspeed = -4;
      else
        Kat.Physics.hspeed = 4;
    }

    Base.Sprite.Play("Spin");
  }
}
