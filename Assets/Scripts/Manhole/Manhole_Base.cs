using UnityEngine;

public class Manhole_Base : SolidObj {
  public override eObjectType SpecialType { get { return eObjectType.MANHOLE; } }
  protected Animator animator;
  private float restY, spriteRestY;

  [Tooltip("Attack needed to open. 0 = standard jump, 1 = ground pound, 2 = ground boom")]
  public int strength = 0;

  protected override void Init() {
    this.animator = transform.Find("Sprite").GetComponent<Animator>();
    this.spriteRestY = animator.gameObject.transform.position.y;

    restY = y;
    Physics.Active = false;
  }

  protected override void Step() {
    if (Physics.Active && y <= restY && Physics.vspeed < 0) {
      active = true;
      y = restY;
      animator.Play("Idle");
    }

    Physics.Active = !active;

    if (active) {
      if (Stitch.Kat.Footing == this && animator.gameObject.transform.position.y != spriteRestY - 1) {
        SetSpriteY(spriteRestY - 1);
        Sound.Play("Down");
      }
      else if (Stitch.Kat.Footing != this && animator.gameObject.transform.position.y == spriteRestY - 1){
        SetSpriteY(spriteRestY);
        Sound.Play("Up");
      }
    }
  }

  public void StartSpinning() {
    active = false;
    Physics.Active = true;

    animator.Play("Spin");
    animator.speed = 2f;
    Physics.vspeed = 5;
    Sound.Play("Flip");
  }

  private void SetSpriteY(float y) {
    animator.gameObject.transform.position = new Vector3(animator.gameObject.transform.position.x, y, animator.gameObject.transform.position.z);
  }
}
