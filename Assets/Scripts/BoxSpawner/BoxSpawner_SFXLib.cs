using System.Collections;
using System.Collections.Generic;

public class BoxSpawner_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("BoxSpawn", 1),
      new Clip("BoxSpawnerHurt", 1)
    };
  }
}
