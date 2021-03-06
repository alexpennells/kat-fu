using UnityEngine;
using System;

public class FrontLayer : MonoBehaviour {
  private float fade = 1f;
  private Ferr2DT_PathTerrain floor;
  private Ferr2DT_PathTerrain blackness;
  private Fan_Base fan;

  void Start() {
    floor = transform.Find("Floor").GetComponent<Ferr2DT_PathTerrain>();
    blackness = transform.Find("BlacknessFade").GetComponent<Ferr2DT_PathTerrain>();

    if (Stitch.fanStatus[0]) {
      fan = GameObject.Find("Fan").GetComponent<Fan_Base>();
      Mouse_Base m = Game.Create("Mouse", new Vector3(1700, 32, 0)) as Mouse_Base;
      m.runAway = true;
    }
  }

  void Update() {
    if (Stitch.Kat.y < 112)
      fade = Math.Max(fade - 0.05f, 0f);
    else
      fade = Math.Min(fade + 0.05f, 1f);

    floor.vertexColor = new Color(1, 1, 1, Math.Max(fade, 0.3f));
    floor.Build();
    blackness.vertexColor = new Color(1, 1, 1, fade);
    blackness.Build();

    if (fan)
      fan.SetHumVolume(Math.Min(1 - fade, 0.8f));
  }
}
