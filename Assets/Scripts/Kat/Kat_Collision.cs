using UnityEngine;

public class Kat_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.KAT; } }

  private Kat_Base Kat { get { return Base as Kat_Base; } }

  protected override void SceneExitCollision(SceneExit other) {
    if (other.isDoor && (!Game.UpHeld || !Base.HasFooting))
      return;

    Game.ChangeScene(other.sceneName, other.exitID, "KatHead");
  }

  protected override void TunaCanCollision(TunaCan_Base other) {
    if (!Game.UpHeld || !Base.HasFooting)
      return;

    other.Collect();
  }

  protected override void FanCollision(Fan_Base other) {
    if (Kat.Is("Punching") && Kat.x > other.Mask.Right - 20 && Kat.Sprite.FacingRight) {
      if ((other.LastAttackID != (Base.Sprite as Kat_Sprite).AttackID)) {
        other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
        other.GetHurt();

        if (Base.Sprite.FacingRight)
          Stitch.CreateHit(new Vector3(Base.Mask.Right + 10, Base.Mask.Center.y, Base.z));
        else
          Stitch.CreateHit(new Vector3(Base.Mask.Left - 10, Base.Mask.Center.y, Base.z));

        Base.Physics.hspeed = 0;
        Game.FreezeFor(0.1f);
        return;
      }
    }

    if (!Kat.Is("Punching"))
      Base.x += other.power;
  }

  protected override void HeartItemCollision(HeartItem_Base other) {
    if (!other.Collected && other.CanBeCollected && Stitch.heartCount > Stitch.katHealth)
      other.Collect();
  }

  protected override void RecyclingCollision(Recycling_Base other) {
    if (other.Dead)
      return;

    if (!other.Hurt)
      PushAwayFrom(other);

    if (Kat.Is("Punching") && PunchSuccess(other)) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      Kat.Physics.hspeed = 0;
      other.GetHurt(5, (Base.Sprite.FacingRight) ? 2 : -2);

      if (Base.Sprite.FacingRight)
        Stitch.CreateHit(new Vector3(Base.Mask.Right + 10, Base.Mask.Center.y, Base.z));
      else
        Stitch.CreateHit(new Vector3(Base.Mask.Left - 10, Base.Mask.Center.y, Base.z));

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("Kicking") && KickSuccess(other)) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      other.GetHurt(10, (Base.Sprite.FacingRight) ? 4 : -4);
      BounceOffEnemy(4);

      if (Base.Sprite.FacingRight)
        Stitch.CreateHit(new Vector3(Base.Mask.Right, Base.Mask.Center.y, Base.z));
      else
        Stitch.CreateHit(new Vector3(Base.Mask.Left, Base.Mask.Center.y, Base.z));

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("GroundPounding")) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      other.GetHurt(10, (Base.Sprite.FacingLeft) ? 2 : -2);
      other.Physics.vspeed = -2;
      BounceOffEnemy(6, false);
      Kat.Physics.hspeed = 0;

      Stitch.CreateGroundHit(new Vector3(Base.x, Base.Mask.Bottom, Base.z));
      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("Uppercutting")) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      other.GetHurt(10, (Kat.x > other.x) ? -2 : 2);
      BounceOffEnemy(7, false);

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (!other.Hurt && !Kat.Is("Invincible") && !Kat.Is("Hurt"))
      GetHurt(other, 0.5f, 5);
  }

  protected override void BoxSpawnerCollision(BoxSpawner_Base other) {
    PushAwayFrom(other);

    if (Kat.Is("Punching") && PunchSuccess(other)) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      Kat.Physics.hspeed = 0;
      other.GetHurt(5);

      if (Base.Sprite.FacingRight)
        Stitch.CreateHit(new Vector3(Base.Mask.Right + 10, Base.Mask.Center.y, Base.z));
      else
        Stitch.CreateHit(new Vector3(Base.Mask.Left - 10, Base.Mask.Center.y, Base.z));

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("Kicking") && KickSuccess(other)) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      BounceOffEnemy(4);
      other.GetHurt(10);

      if (Base.Sprite.FacingRight)
        Stitch.CreateHit(new Vector3(Base.Mask.Right, Base.Mask.Center.y, Base.z));
      else
        Stitch.CreateHit(new Vector3(Base.Mask.Left, Base.Mask.Center.y, Base.z));

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("GroundPounding")) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      other.GetHurt(10);
      other.Physics.vspeed = -2;
      BounceOffEnemy(6, false);
      Kat.Physics.hspeed = 0;

      Stitch.CreateGroundHit(new Vector3(Base.x, Base.Mask.Bottom, Base.z));
      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }

    if (Kat.Is("Uppercutting")) {
      if (other.LastAttackID == (Base.Sprite as Kat_Sprite).AttackID)
        return;

      other.GetHurt(10);
      BounceOffEnemy(7, false);

      other.LastAttackID = (Base.Sprite as Kat_Sprite).AttackID;
      Game.FreezeFor(0.05f);
      return;
    }
  }

  protected override void AirVentCollision(AirVent_Base other) {
    if (Base.Physics.vspeed < -3)
      Base.Physics.vspeed = -3;

    Base.Physics.vspeed += other.ventPower;

    if (Base.Physics.vspeed > 5)
      Base.Physics.vspeed = 5;

    Base.Sprite.Play("Spin");
    Base.Physics.SkipNextGravityUpdate();
  }

  protected override void ZombieCollision(Zombie_Base other) {
    if (other.Sprite.IsPlaying("zombie_die"))
      return;

    if (GroundAttackSuccess(other)) {
      other.GetHurt();
      if (Base.Sprite.FacingRight)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      BounceOffEnemy(6, Kat.Is("Kicking"));
    }

    else if (Kat.Is("GroundPounding")) {
      other.GetHurt();

      if (other.Sprite.FacingLeft)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Is("Uppercutting")) {
      other.GetHurt();

      if (Kat.x > other.x)
        other.Physics.hspeed = -other.Physics.hspeedMax;
      else
        other.Physics.hspeed = other.Physics.hspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Is("Invincible") && !Kat.Is("Hurt"))
      Kat.State("Hurt", new object[] { other.x > Kat.x ? -5 : 5 });
  }

  protected override void BatCollision(Bat_Base other) {
    if (other.Sprite.IsPlaying("bat_die"))
      return;

    if (GroundAttackSuccess(other)) {
      other.GetHurt();

      if (Base.Sprite.FacingRight)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      other.Physics.vspeed = (other.Mask.Center.y - Base.Mask.Center.y) / 2;

      BounceOffEnemy(6, Kat.Is("Kicking"));
    }

    else if (Kat.Is("GroundPounding")) {
      other.GetHurt();

      other.Physics.hspeed = 0;
      other.Physics.vspeed = -other.Physics.vspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Is("Uppercutting")) {
      other.GetHurt();

      other.Physics.hspeed = (other.x - Base.x) / 2;
      other.Physics.vspeed = other.Physics.vspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Is("Invincible") && !Kat.Is("Hurt"))
      Kat.State("Hurt", new object[] { other.x > Kat.x ? -5 : 5 });
  }

  protected override void WeedCollision(Weed_Base other) {
    if (other.Sprite.IsPlaying("weed_die"))
      return;

    if (!other.Sprite.IsPlaying("weed_bite") && GroundAttackSuccess(other)) {
      other.GetHurt();

      if (Base.Sprite.FacingRight)
        other.Physics.hspeed = other.Physics.hspeedMax;
      else
        other.Physics.hspeed = -other.Physics.hspeedMax;

      other.Physics.vspeed = (other.Mask.Center.y - Base.Mask.Center.y) / 2;

      BounceOffEnemy(6, Kat.Is("Kicking"));
    }

    else if (Kat.Is("GroundPounding")) {
      other.GetHurt();

      other.Physics.hspeed = 0;
      other.Physics.vspeed = -other.Physics.vspeedMax;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (!other.Sprite.IsPlaying("weed_bite") && Kat.Is("Uppercutting")) {
      other.GetHurt();

      other.Physics.hspeed = (other.x - Base.x) / 2;
      other.Physics.vspeed = other.Physics.vspeedMax;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Is("Invincible") && !Kat.Is("Hurt"))
      Kat.State("Hurt", new object[] { other.x > Kat.x ? -5 : 5 });
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

      BounceOffEnemy(6, Kat.Is("Kicking"));
    }

    else if (Kat.Is("GroundPounding")) {
      other.GetHurt();

      other.Physics.vspeed = -2;
      if (other.Sprite.FacingLeft)
        other.Physics.hspeed = 2;
      else
        other.Physics.hspeed = -2;

      BounceOffEnemy(8, false);
      Kat.Physics.hspeed = 0;
    }

    else if (Kat.Is("Uppercutting")) {
      other.GetHurt();

      if (Kat.x > other.x)
        other.Physics.hspeed = -2;
      else
        other.Physics.hspeed = 2;

      BounceOffEnemy(12, false);
    }

    else if (!other.hurt && !Kat.Is("Invincible") && !Kat.Is("Hurt"))
      Kat.State("Hurt", new object[] { other.x > Kat.x ? -5 : 5 });
  }

  protected override void EnergyBallCollision(EnergyBall_Base other) {
    if (!other.impacted && !Kat.Is("Invincible") && !Kat.Is("Hurt")) {
      Kat.State("Hurt", new object[] { other.x > Kat.x ? -5 : 5 });
      other.Impact();
    }
  }

  /***********************************
   * PRIVATE FUNCTIONS
   **********************************/

  private bool PunchSuccess (BaseObj other) {
    return ((Base.Sprite.FacingRight && other.x > Kat.x) || (Base.Sprite.FacingLeft && other.x < Kat.x));
  }

  private bool KickSuccess (BaseObj other) {
    return ((Base.Sprite.FacingRight && (other.x + 20) > Kat.x) || (Base.Sprite.FacingLeft && (other.x - 20) < Kat.x));
  }

  private bool GroundAttackSuccess (BaseObj other) {
    return (Kat.Is("Punching") && PunchSuccess(other)) || (Kat.Is("Kicking") && KickSuccess(other));
  }

  private void GetHurt (BaseObj other, float damage, float hspeed) {
    Kat.State("Hurt", new object[] { other.x > Kat.x ? -hspeed : hspeed });
    Game.HUD.LoseHealth(damage);
    Game.FreezeFor(0.1f);

    if (other.x > Kat.x)
      Stitch.CreateHit(new Vector3(Base.Mask.Right + 10, Base.Mask.Center.y, Base.z));
    else
      Stitch.CreateHit(new Vector3(Base.Mask.Left - 10, Base.Mask.Center.y, Base.z));
  }

  private void BounceOffEnemy (float newVspeed = 6, bool changeHspeed = true) {
    Kat.stopPhysics = false;
    Kat.AirKickTimer.Enabled = false;
    Kat.GroundKickTimer.Enabled = false;
    Kat.hasAirAttack = true;
    Kat.hasUppercut = true;

    Kat.Physics.vspeed = newVspeed;
    if (changeHspeed) {
      if (Base.Sprite.FacingRight)
        Kat.Physics.hspeed = -4;
      else
        Kat.Physics.hspeed = 4;
    }

    Base.Sprite.Play("Spin");
  }

  private void PushAwayFrom (BaseObj other) {
    if (other.Mask.Center.x > Base.Mask.Center.x && Base.Physics.hspeed > -1)
      Base.Physics.hspeed -= Base.Physics.friction * 4;
    else if (other.Mask.Center.x <= Base.Mask.Center.x && Base.Physics.hspeed < 1)
      Base.Physics.hspeed += Base.Physics.friction * 4;
  }
}
