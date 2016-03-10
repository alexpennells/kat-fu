using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class HeartItem_Base : BaseObj {
  private bool collected = false;
  public bool Collected { get { return collected; } }

  private bool dying = false;
  public bool Dying { get { return dying; } }

  private bool canBeCollected = false;
  public bool CanBeCollected { get { return canBeCollected; } }

  // Once collected, move to this destination point before destroying self.
  private int uiHeartIndex;

  public static void Create(Vector3 position) {
    BaseObj b = Game.Create("HeartItem", position);
    b.Physics.hspeed = Game.Random.Next(-2, 3);
    b.Physics.vspeed = Game.Random.Next(1, 3);
  }

  public override void DestroySelf() {
    if (Collected) {
      Game.HUD.AddHeart(uiHeartIndex);
      Game.SFX.Play("HeartAdd", 1f);
    }

    base.DestroySelf();
  }

  protected override void Init () {
    DeathTimer.Interval = 15000;
    DeathTimer.Enabled = true;
    CollectionTimer.Interval = 1000;
    CollectionTimer.Enabled = true;
  }

  public void Collect () {
    collected = true;
    DeathTimer.Enabled = false;
    Destroy(transform.Find("Light").gameObject);

    Sprite.StopCoroutine("Flicker");
    Sprite.SetAlpha(1f);
    Sprite.SetLayer(20001);

    Physics.Active = false;

    this.uiHeartIndex = Game.HUD.GetNewHeartIndex();
    Stitch.katHealth = Math.Min(Stitch.katHealth + 1, Stitch.heartCount);

    Game.SFX.Play("HeartPickup", 1f);
    StartCoroutine("MoveToUI");
    Sprite.StartBlur(0.01f, 0.6f, 0.1f);
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer DeathTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    DeathTimer.Enabled = false;
    dying = true;
  }

  public Timer CollectionTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    CollectionTimer.Enabled = false;
    canBeCollected = true;
  }

  /*************************************************************
   * CO-ROUTINES
   ************************************************************/

  protected virtual IEnumerator MoveToUI() {
    Vector3 uiDest = Game.HUD.GetHeart(this.uiHeartIndex).gameObject.transform.localPosition;
    float startZ = z;
    Vector3 startScale = transform.localScale;

    for (float t = 0; t <= 1; t += 0.2f) {
      z = Mathf.Lerp(startZ, uiDest.z, t);
      transform.localScale = Vector3.Lerp(startScale, new Vector3(0.4f, 0.4f, 1), t);
      yield return null;
    }

    yield return new WaitForSeconds(0.5f);
    transform.parent = Game.HUD.transform;

    Vector3 start = transform.localPosition;

    for (float t = 0; t < 1; t += 0.04f) {
      transform.localPosition = Vector3.Lerp(start, uiDest, t);
      yield return null;
    }

    DestroySelf();
    yield break;
  }

}
