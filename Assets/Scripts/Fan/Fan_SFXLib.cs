using System.Collections;
using System.Collections.Generic;

public class Fan_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("FanBreak", 1),
      new Clip("FanHum", 1)
    };
  }
}
