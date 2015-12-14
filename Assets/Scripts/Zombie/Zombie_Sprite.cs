using UnityEngine;
using System.Collections;

public class Zombie_Sprite : SpriteObj {
  public override void Step () {
    if ((Base as Zombie_Base).attacking || (Base as Zombie_Base).hurt)
      return;

    if ((Base as Zombie_Base).InChaseRange()) {
      TurnSprite();
      Play("Walk");
    }
    else
      Play("Idle");
  }

  private void TurnSprite() {
    if (Stitch.Kat.x < Base.x)
      transform.localScale = new Vector3(-1, 1, 1);
    else if (Stitch.Kat.x > Base.x)
      transform.localScale = new Vector3(1, 1, 1);
  }

  public void AttackAnimComplete() {
    Play("Walk");
  }

  public void DieAnimComplete() {
    Base.DestroySelf();
  }

  public void PlayIdle(float time) {
    Animate("zombie_idle", 0.25f);
  }

  public void PlayWalk(float time) {
    Animate("zombie_walk", 1f);
  }

  public void PlayAttack(float time) {
    Animate("zombie_attack", 1.5f);
  }

  public void PlayHurt(float time) {
    Animate("zombie_hurt", 1f);
  }

  public void PlayDie(float time) {
    Animate("zombie_die", 1f);
  }
}
