using UnityEngine;

public class Stitch : MonoBehaviour
{

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
