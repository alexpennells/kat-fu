using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Glob_Sprite : SpriteObj {

  public override void Step () {
    TurnSprite();
  }

  private void TurnSprite() {
    if (!Base.HasFooting || IsPlaying("glob_hurt", "glob_die"))
      return;

    if (Stitch.Kat.x < Base.x)
      FacingLeft = true;
    else if (Stitch.Kat.x > Base.x)
      FacingRight = true;
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    Animate("glob_idle", 0.5f);
  }

  public void PlayJump() {
    if (VectorLib.GetDistanceX(Base, Stitch.Kat) > 100)
      Animate("glob_jump", 1f);
    else
      Animate("glob_attack", 1f);
  }

  public void PlayLand() {
    if (IsPlaying("glob_attack"))
      Animate("glob_attack_land", 1f);
    else
      Animate("glob_land", 1f);
  }

  public void PlayHurt() {
    Animate("glob_hurt", 1f);
  }

  public void PlayDie() {
    Animate("glob_die", 1f);
  }
}
