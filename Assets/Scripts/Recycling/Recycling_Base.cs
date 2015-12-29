using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Recycling_Base : BaseObj {
  [Tooltip("Minimum delay in milliseconds between each jump")]
  public int jumpDelay = 3000;

  [Tooltip("The jump height of the object")]
  public int jumpHeight = 5;

  [Tooltip("Speed the object moves when in the air")]
  public int moveSpeed = 1;

  [Tooltip("Enemy total health before death")]
  public int health = 20;

  [Tooltip("The min X position that this object will move to")]
  public float turfMin = -100f;

  [Tooltip("The max X position that this object will move to")]
  public float turfMax = 100f;

  /***********************************
   * PRIVATE VARS AND ACCESSORS
   **********************************/

  private float startX;
  private bool canJump = true;

  public float TurfMax { get { return startX + turfMax; } }
  public float TurfMin { get { return startX + turfMin; } }

  private bool hurt = false;
  public bool Hurt { get { return hurt; } }

  /***********************************
   * FUNCTIONS
   **********************************/

  protected override void Init () {
    HurtTimer.Interval = 600;
    JumpTimer.Interval = jumpDelay;
    JumpTimer.Enabled = true;

    startX = x;
  }

  protected override void Step () {
    if (hurt && health <= 0) {
      Physics.SkipNextFrictionUpdate();

      transform.RotateAround(
        Collision.Handler.GetComponent<BoxCollider2D>().bounds.center,
        Vector3.forward,
        3f * ((Physics.hspeed >= 0) ? -5 : 5)
      );
    }
    else
      transform.rotation = Quaternion.AngleAxis(Physics.vspeed * (Physics.hspeed > 0 ? 2 : -2), Vector3.forward);

    if (!HasFooting)
      Physics.SkipNextFrictionUpdate();

    if (HasFooting && canJump && !hurt) {
      // Jump towards the cat.
      if (x < TurfMin) {
        Physics.hspeed = moveSpeed;
        Physics.vspeed = jumpHeight;
        JumpTimer.Enabled = true;
        canJump = false;
      }
      else if (x > TurfMax) {
        Physics.hspeed = -moveSpeed;
        Physics.vspeed = jumpHeight;
        JumpTimer.Enabled = true;
        canJump = false;
      }
      else if (Stitch.Kat.x > TurfMin && Stitch.Kat.x < TurfMax) {
        Physics.hspeed = (Stitch.Kat.x > x) ? moveSpeed : -moveSpeed;
        Physics.vspeed = jumpHeight;
        JumpTimer.Enabled = true;
        canJump = false;
      }
    }
  }

  public void GetHurt(int damage = 10) {
    HurtTimer.Enabled = false;
    JumpTimer.Enabled = false;

    health -= damage;
    hurt = true;

    if (health <= 0) {
      Physics.vspeed = 3;
      Physics.gravity = 0.25f;
      Destroy(SolidPhysics.Collider);
    } else {
      HurtTimer.Enabled = true;
      JumpTimer.Enabled = true;
    }
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer JumpTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    JumpTimer.Enabled = false;
    canJump = true;
  }

  public Timer HurtTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;
    hurt = false;
  }

  /***********************************
   * EDITOR NONSENSE
   **********************************/

  protected override void DrawGizmos () {
    if (Mask == null)
      return;

    if (!Application.isPlaying)
      startX = x;

    Debug.DrawLine(new Vector3(TurfMin, Mask.Center.y - 10, 0), new Vector3(TurfMin, Mask.Center.y + 10, 0), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(TurfMax, Mask.Center.y - 10, 0), new Vector3(TurfMax, Mask.Center.y + 10, 0), Color.magenta, 0, false);
  }

}
