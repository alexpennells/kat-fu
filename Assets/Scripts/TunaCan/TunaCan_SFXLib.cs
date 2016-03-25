using System.Collections;
using System.Collections.Generic;

public class TunaCan_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("TunaCanHum", 1)
    };
  }
}
