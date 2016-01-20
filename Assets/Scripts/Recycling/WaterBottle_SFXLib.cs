using System.Collections;
using System.Collections.Generic;

public class WaterBottle_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("WaterBottleBounce", 1),
      new Clip("WaterBottleHurt", 1)
    };
  }
}
