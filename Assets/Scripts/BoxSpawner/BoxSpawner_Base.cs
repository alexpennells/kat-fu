using UnityEngine;
using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;

public enum eSpawnType
{
  CAN,
  WATER_BOTTLE
};

public class BoxSpawner_Base : BaseObj {
  [Tooltip("Time between spawning objects")]
  public int spawnerDelay = 3000;

  [Tooltip("Max enemies created from this spawner")]
  public int maxSpawns = 3;

  [Tooltip("The type of enemy that will be spawned")]
  public eSpawnType spawnType = eSpawnType.CAN;

  [Tooltip("Enemy total health before death")]
  public int health = 30;

  /***********************************
   * PRIVATE VARS AND ACCESSORS
   **********************************/

  private int lastAttackId = -500;
  public int LastAttackID { get { return lastAttackId; } set { lastAttackId = value; } }

  private bool hurt = false;
  public bool Hurt { get { return hurt; } }

  private int spawning = 0;
  private List<int> spawns = new List<int>();

  /***********************************
   * FUNCTIONS
   **********************************/

  protected override void Init () {
    SpawnTimer.Interval = spawnerDelay;
    HurtTimer.Interval = 600;
    SpawnTimer.Enabled = true;
  }

  protected override void Step () {
    if (spawning == 1) {
      if (transform.localScale.y < 1.4)
        transform.localScale = transform.localScale + (Vector3.up * 0.1f);
      else
        CreateSpawn();
    } else if (spawning == 2) {
      if (transform.localScale.y > 1)
        transform.localScale = transform.localScale - (Vector3.up * 0.05f);
      else {
        spawning = 0;
        SpawnTimer.Enabled = true;
      }
    }
  }

  public void CreateSpawn() {
    spawning = 2;
    if (spawns.Count == maxSpawns)
      return;

    Recycling_Base spawn;

    switch (spawnType) {
      case eSpawnType.WATER_BOTTLE:
        spawn = Game.Create("WaterBottle", new Vector2(x, y)) as Recycling_Base;
        spawn.Physics.vspeed = 6f;
        spawn.Physics.hspeed = (Stitch.Kat.x > x) ? 1f : -1f;
        break;
      default:
        spawn = Game.Create("Can", new Vector2(x, y)) as Recycling_Base;
        spawn.Physics.vspeed = 8f;
        spawn.Physics.hspeed = (Stitch.Kat.x > x) ? 2f : -2f;
      break;
    }

    spawn.turfMin = -100000;
    spawn.turfMax = 100000;
    spawn.SolidPhysics.startOnGround = false;
    spawn.Sprite.SetLayer(Sprite.GetLayer() - 1);
    spawn.HasAdjustedLayer = false;
    spawn.Spawner = this;

    Sound.Play("Spawn");
    spawns.Add(spawn.ID);
  }
  public void RemoveSpawn(int spawnID) {
    spawns.Remove(spawnID);
  }

  public void GetHurt(int damage) {
    Sound.Play("Hurt");

    HurtTimer.Enabled = false;
    health -= damage;
    hurt = true;

    if (health <= 0) {
      SpawnTimer.Enabled = false;

      Physics.gravity = 0.25f;
      Stitch.Kat.Sound.Play("Attack");
      PlayEffect("explosion");

      HeartItem_Base.Create(Mask.Center);
      HeartItem_Base.Create(Mask.Center + new Vector3(6, 0, 0));
      HeartItem_Base.Create(Mask.Center + new Vector3(-6, 0, 0));

      DestroySelf();
    }
    else
      HurtTimer.Enabled = true;
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer SpawnTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    SpawnTimer.Enabled = false;
    spawning = 1;
  }

  public Timer HurtTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    SpawnTimer.Enabled = false;
    HurtTimer.Enabled = false;
    hurt = false;
    SpawnTimer.Enabled = true;
  }

}
