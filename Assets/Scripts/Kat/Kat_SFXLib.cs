using System.Collections;
using System.Collections.Generic;

public class Kat_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("GunStart", 1),
      new Clip("GunEnd", 1),
      new Clip("Gunshot", 3),
      new Clip("Footstep", 4),
      new Clip("Punch", 3),
      new Clip("CatDodge", 1),
      new Clip("CatAttack", 3),
      new Clip("CatHiss", 3),
      new Clip("CatHurt", 1),
      new Clip("CatUppercut", 1)
    };
  }
}
