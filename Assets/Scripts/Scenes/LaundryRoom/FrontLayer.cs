using UnityEngine;
using System;

public class FrontLayer : MonoBehaviour {
  private float fade = 1f;
  private Ferr2DT_PathTerrain floor;
  private Ferr2DT_PathTerrain blackness;

  void Start() {
    floor = transform.Find("Floor").GetComponent<Ferr2DT_PathTerrain>();
    blackness = transform.Find("BlacknessFade").GetComponent<Ferr2DT_PathTerrain>();
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
  }
}
