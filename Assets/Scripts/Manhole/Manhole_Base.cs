using UnityEngine;

public class Manhole_Base : SolidObj {
  public override eObjectType SpecialType { get { return eObjectType.MANHOLE; } }
  protected Animator animator;
  private float restY;

  protected override void Init() {
    this.animator = transform.Find("Sprite").GetComponent<Animator>();
    restY = y;
    Physics.Active = false;
  }

  protected override void Step() {
    if (Physics.Active && y <= restY) {
      active = true;
      y = restY;
      animator.Play("Idle");
    }

    Physics.Active = !active;
  }

  public void StartSpinning() {
    active = false;
    Physics.Active = true;

    animator.Play("Spin");
    animator.speed = 2f;
    Physics.vspeed = 5;
  }
}
