using UnityEngine;

public class Stitch : MonoBehaviour
{

  void Awake() {
    instance = this;
    player = GameObject.FindWithTag("Player").GetComponent<Player>();
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  // private Kat kat;
  // public Kat LocalKat { get { return kat; } }
  // public static Kat Kat { get { return Stitch.Instance.LocalKat; } }

}
