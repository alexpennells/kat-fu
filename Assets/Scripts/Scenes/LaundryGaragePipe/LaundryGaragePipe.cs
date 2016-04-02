using UnityEngine;
using System;

public class LaundryGaragePipe : MonoBehaviour {
  private float fade = 1f;
  private Fan_Base fan;

  void Start() {
    fan = GameObject.Find("Fan").GetComponent<Fan_Base>();
  }

  void Update() {
    if (fan) {
      if (Math.Abs(Stitch.Kat.x - fan.x) < 300)
        fade = Math.Min(fade + 0.05f, 2f);
      else
        fade = Math.Max(fade - 0.05f, 0.5f);

      fan.SetHumVolume(fade);
    }
  }
}
