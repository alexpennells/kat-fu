using System.Collections;
using System.Collections.Generic;

public class Manhole_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("ManholeUp", 1),
      new Clip("ManholeDown", 1),
      new Clip("ManholeFlip", 1)
    };
  }
}
