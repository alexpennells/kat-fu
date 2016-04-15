using UnityEngine;

public class Stitch : MonoBehaviour
{
  public static float katHealth = 2f;

  /***********************************
   * SINGLETON ITEMS
   **********************************/

   public static bool[] tunaCans = { true, false, false, false, false, false, false, false };
   public static bool[] fanStatus = { false, false };

  /***********************************
   * ABILITIES
   **********************************/

  public static bool canKick = true;
  public static bool canGroundPound = true;
  public static bool canGroundBoom = false;
  public static bool canClimb = false;
  public static bool canCling = false;
  public static bool canDodge = false;
  public static bool canUseGun = false;
  public static bool canUseGrenades = false;
  public static bool canUppercut = true;

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

  public static GameObject CreateDust(Vector3 pos) {
    GameObject p = Game.CreateParticle("Dust", pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform)
      t.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    return p;
  }

  public static GameObject CreateHit(Vector3 pos) {
    string[] colors = { "Red", "White", "Blue", "Green", "Yellow" };
    GameObject p = Game.CreateParticle("Hit" + colors[Game.Random.Next(0, 5)], pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform)
      t.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    return p;
  }

  public static GameObject CreateGroundHit(Vector3 pos) {
    bool isCrit = Game.Random.Next(1, 10) == 5;
    GameObject p = Game.CreateParticle(isCrit ? "GroundPoundCrit" : "GroundPound", pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform)
      t.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    return p;
  }

  public static GameObject CreateGroundBoom(Vector3 pos) {
    bool isCrit = Game.Random.Next(1, 10) == 5;
    GameObject p = Game.CreateParticle(isCrit ? "GroundBoomCrit" : "GroundBoom", pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform) {
      Renderer child = t.GetComponent<Renderer>();
      if (child != null)
        child.sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    }
    return p;
  }

  public static GameObject CreateSmallElectricBlast(Vector3 pos) {
    GameObject p = Game.CreateParticle("SmallElectricBlast", pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform) {
      Renderer child = t.GetComponent<Renderer>();
      if (child != null)
        child.sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    }
    return p;
  }

  public static GameObject CreateSmallSmokePuff(Vector3 pos) {
    GameObject p = Game.CreateParticle("SmallSmokePuff", pos);
    p.GetComponent<Renderer>().sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    foreach (Transform t in p.transform) {
      Renderer child = t.GetComponent<Renderer>();
      if (child != null)
        child.sortingOrder = Stitch.Kat.Sprite.GetLayer() + 1;
    }
    return p;
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  private Kat_Base kat;
  public Kat_Base LocalKat { get { return kat; } }
  public static Kat_Base Kat { get { return Stitch.Instance.LocalKat; } }

}
