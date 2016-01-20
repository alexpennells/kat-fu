using System.Collections;
using System.Collections.Generic;

public class Can_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("CanBounce", 1),
      new Clip("CanHurt", 1)
    };
  }
}
