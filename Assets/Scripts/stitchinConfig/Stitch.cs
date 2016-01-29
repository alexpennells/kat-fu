using UnityEngine;

public class Stitch : MonoBehaviour
{
  /***********************************
   * DRAWING CONSTANTS
   **********************************/

  public static bool SHOW_GAME_GRID = false;
  public static bool SHOW_OBJ_ORIGIN = true;
  public static bool SHOW_MASK_BOUNDS = true;
  public static bool SHOW_TERRAIN_BOUNDS = true;
  public static bool SHOW_RAIL_PATHS = true;
  public static bool SHOW_CAMERA_BOUNDS = true;
  public static bool SHOW_ENTRANCES = true;

  /***********************************
   * GAME CONSTANTS
   **********************************/

  void Awake() {
    instance = this;
    kat = GameObject.FindWithTag("Player").GetComponent<Kat_Base>();
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  private Kat_Base kat;
  public Kat_Base LocalKat { get { return kat; } }
  public static Kat_Base Kat { get { return Stitch.Instance.LocalKat; } }

}
