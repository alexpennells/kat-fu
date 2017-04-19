using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Spider_Base : BaseObj {
  [Tooltip("Enemy total health before death")]
  public int health = 3;
  public bool Dead { get { return health <= 0; } }

  [Tooltip("Easiest to load this via the inspector")]
  public Sprite deadSprite;

  private float swaySpeed = 0.3f;

  private int lastAttackId = -500;
  public int LastAttackID { get { return lastAttackId; } set { lastAttackId = value; } }

  private SpriteRenderer Renderer { get { return spriteRenderer; } }
  protected SpriteRenderer spriteRenderer;

  protected override void LoadReferences() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  protected override void Init () {
    Physics.Active = false;
  }

  protected override void Step () {
    if (!Dead) {
      if (transform.eulerAngles.z < 180 && transform.eulerAngles.z > 0) {
        swaySpeed = Math.Max(swaySpeed - transform.eulerAngles.z / 600, -4f);
        if (swaySpeed > 0)
          swaySpeed *= 0.975f;
      }
      else if (transform.eulerAngles.z > 180 && transform.eulerAngles.z < 360) {
        swaySpeed = Math.Min(swaySpeed + (360 - transform.eulerAngles.z) / 600, 4f);
        if (swaySpeed < 0)
          swaySpeed *= 0.975f;
      }
    }

    transform.Rotate(Vector3.forward * swaySpeed);
  }

  public void GetHurt (bool moveRight) {
    health--;
    swaySpeed += moveRight ? 3f : -3f;
    Physics.hspeed = moveRight ? 2 : -2;
    Physics.vspeed = 4;

    if (Game.Random.Next(1, 3) == 1)
        HeartItem_Base.Create(Mask.Center);

    if (Dead) {
      Vector3 newPos = Collision.Center2D;
      Renderer.sprite = deadSprite;
      swaySpeed *= -3f;
      Position = newPos;
      Physics.Active = true;
    }
  }
}
