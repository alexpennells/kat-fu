using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Bat_Base : BaseObj {
  // Max position this bat can move in either direction.
  public Vector2 max = new Vector2(0, 0);
  public Vector2 min = new Vector2(0, 0);
  public Vector2 accel = new Vector2(0, 0);

  // Whether or not the reverse the acceleration in either direction.
  public bool reverseX = false;
  public bool reverseY = false;

  // The interval at which this guy will spit shit. If 0 he won't spit at all!
  public int attackInterval = 0;

  public bool hurt = false;
  public int health = 50;

  private bool attackOnNextStep = false;

  public void GetHurt (int damage = 10) {
    attackOnNextStep = false;
    AttackTimer.Enabled = false;
    HurtTimer.Enabled = false;
    health -= damage;
    hurt = true;

    if (health > 0) {
      Sprite.Play("Hurt");
      HurtTimer.Enabled = true;
    }
    else
      Sprite.Play("Die");
  }

  protected override void Init () {
    // Max and min should be based on local position.
    max = new Vector2(x + max.x, Mask.Center.y + max.y);
    min = new Vector2(x + min.x, Mask.Center.y + min.y);

    HurtTimer.Interval = 300;

    if (attackInterval != 0) {
      AttackTimer.Interval = attackInterval;
      AttackTimer.Enabled = true;
    }
  }

  protected override void Step () {
    if (Sprite.IsPlaying("bat_die")) {
      Physics.hspeed = 0;
      Physics.vspeed = 0;
      return;
    }

    if (Sprite.IsPlaying("bat_hurt", "bat_attack"))
      return;

    if (attackOnNextStep) {
      Sprite.Play("Attack");
      attackOnNextStep = false;
      return;
    }

    Physics.hspeed += (reverseX ? -accel.x : accel.x);
    Physics.vspeed += (reverseY ? -accel.y : accel.y);

    if (x > max.x)
      reverseX = true;
    else if (x < min.x)
      reverseX = false;

    if (Mask.Center.y > max.y)
      reverseY = true;
    else if (Mask.Center.y < min.y)
      reverseY = false;
  }

  // Draw the movement area
  protected override void DrawGizmos () {
    if (Mask == null)
      return;

    Vector2 drawMax, drawMin;

    if (!Application.isPlaying) {
      drawMax = new Vector2(x + max.x, Mask.Center.y + max.y);
      drawMin = new Vector2(x + min.x, Mask.Center.y + min.y);
    }
    else {
      drawMax = max;
      drawMin = min;
    }

    Debug.DrawLine(new Vector3(drawMin.x, Mask.Center.y - 10, 0), new Vector3(drawMin.x, Mask.Center.y + 10, 0), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(drawMax.x, Mask.Center.y - 10, 0), new Vector3(drawMax.x, Mask.Center.y + 10, 0), Color.magenta, 0, false);

    Debug.DrawLine(new Vector3(x - 10, drawMin.y, 0), new Vector3(x + 10, drawMin.y, 0), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(x - 10, drawMax.y, 0), new Vector3(x + 10, drawMax.y, 0), Color.magenta, 0, false);
  }

  public Timer HurtTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;

    if (attackInterval != 0)
      AttackTimer.Enabled = true;

    hurt = false;
  }

  public Timer AttackTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    AttackTimer.Enabled = false;
    attackOnNextStep = true;
  }

}
