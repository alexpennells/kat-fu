using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class TunaCan_Base : BaseObj {
  [Tooltip("Which unique can is this in the Stitchin engine")]
  public int canID = 0;

  public bool Collected { get { return Stitch.tunaCans[canID]; } }

  private GameObject glow;
  private AudioSource hum;
  private bool fillingHealth;

  protected override void LoadReferences() {
    glow = transform.Find("Glow").gameObject;
    hum = GetComponent<AudioSource>();

    if (Collected) {
      Destroy(glow);
      hum.Stop();
    }
  }

  protected override void Step() {
    if (!Collected) {
      if (Stitch.Kat.z != z || Math.Abs(Stitch.Kat.x - x) > 300)
        hum.volume = 0f;
      else
        hum.volume = Math.Min(1 - (Math.Abs(Stitch.Kat.x - x) / 300f), 0.5f);
    }
  }

  public void Collect() {
    if (!Collected) {
      Destroy(glow);
      hum.Stop();
      Stitch.tunaCans[canID] = true;
      Stitch.heartCount++;
      Game.HUD.CreateNewHeart();
    }

    StartCoroutine("FillHealth");
  }

  /*************************************************************
   * CO-ROUTINES
   ************************************************************/

  protected IEnumerator FillHealth() {
    if (fillingHealth)
      yield break;
    fillingHealth = true;

    while (Stitch.katHealth < Stitch.heartCount) {
      HeartItem_Base h = HeartItem_Base.Create(Mask.Center) as HeartItem_Base;
      h.Collect();
      yield return new WaitForSeconds(0.25f);
    }

    fillingHealth = false;
  }
}
