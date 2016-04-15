using UnityEngine;
using System;

public class LaundryGaragePipe : MonoBehaviour {
  private float fade = 1f;
  private Fan_Base fan;

  void Start() {
    if (Stitch.fanStatus[1])
      fan = GameObject.Find("Fan").GetComponent<Fan_Base>();

    if (Game.Instance.Entrance && Game.Instance.Entrance.entranceID == 0) {
      Mouse_Base mouse = Game.Create("Mouse", new Vector3(1712, 224, 0)) as Mouse_Base;
      mouse.runAway = true;
    }
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
