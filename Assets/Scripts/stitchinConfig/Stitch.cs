using UnityEngine;

public class Stitch : MonoBehaviour
{
  public static float katHealth = 2f;

  /***********************************
   * ABILITIES
   **********************************/

  public static bool canKick = false;
  public static bool canGroundPound = false;
  public static bool canGroundBoom = false;
  public static bool canClimb = false;
  public static bool canCling = false;
  public static bool canDodge = false;
  public static bool canUseGun = false;
  public static bool canUseGrenades = false;
  public static bool canUppercut = false;

  /***********************************
   * SCALING ABILITIES
   **********************************/

  public static int heartCount = 2;
  public static int jumpHeight = 6;
  public static int moveSpeed = 3;
  public static int maxGrenades = 5;

  /***********************************
   * DRAWING CONSTANTS
   **********************************/

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

  public static GameObject CreateHit(Vector3 pos) {
    string[] colors = { "Red", "White", "Blue", "Green", "Yellow" };
    GameObject p = Game.CreateParticle("Hit" + colors[Game.Random.Next(0, 5)], pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    return p;
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  private Kat_Base kat;
  public Kat_Base LocalKat { get { return kat; } }
  public static Kat_Base Kat { get { return Stitch.Instance.LocalKat; } }

}
