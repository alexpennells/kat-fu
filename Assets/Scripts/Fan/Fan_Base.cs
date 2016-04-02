using UnityEngine;

public class Fan_Base : BaseObj {
  [Tooltip("Adjust the Kat by this hspeed on collision")]
  public float power = -0.1f;

  [Tooltip("Animation speed")]
  public float speed = 1f;

  [Tooltip("Which unique fan is this in the Stitchin engine")]
  public int fanID = 0;

  [Tooltip("The number of times the fan must be hit before it's destroyed")]
  public int fanHealth = 1;

  /***********************************
   * PRIVATE VARS AND ACCESSORS
   **********************************/

  private AudioSource hum;

  private int lastAttackId = -500;
  public int LastAttackID { get { return lastAttackId; } set { lastAttackId = value; } }

  /***********************************
   * FUNCTIONS
   **********************************/

  protected override void LoadReferences() {
    hum = GetComponent<AudioSource>();
  }

  protected override void Init() {
    if (!Stitch.fanStatus[fanID])
      DestroySelf();
  }

  public override void DestroySelf() {
    if (Stitch.fanStatus[fanID]) {
      Stitch.CreateSmallElectricBlast(new Vector3(Mask.Right, Mask.Center.y, z));
      Sound.Play("Break");
      Stitch.fanStatus[fanID] = false;
    }
    base.DestroySelf();
  }

  public void GetHurt() {
    fanHealth--;
    Stitch.CreateSmallSmokePuff(new Vector3(Mask.Right, Mask.Center.y, z));

    if (fanHealth < 1)
      DestroySelf();
  }

  public void SetHumVolume(float vol) {
    hum.volume = vol;
  }
}
