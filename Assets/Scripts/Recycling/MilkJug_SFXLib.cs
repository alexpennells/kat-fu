using System.Collections;
using System.Collections.Generic;

public class MilkJug_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("MilkJugBounce", 1),
      new Clip("MilkJugHurt", 1)
    };
  }
}
