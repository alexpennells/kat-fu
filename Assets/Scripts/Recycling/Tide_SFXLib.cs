using System.Collections;
using System.Collections.Generic;

public class Tide_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("TideBounce", 1),
      new Clip("TideHurt", 1)
    };
  }
}
