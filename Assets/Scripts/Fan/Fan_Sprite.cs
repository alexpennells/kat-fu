using UnityEngine;
using System;
using System.Collections;

public class Fan_Sprite : SpriteObj {
  public override void Init () {
    SetSpeed((Base as Fan_Base).speed);
  }
}
