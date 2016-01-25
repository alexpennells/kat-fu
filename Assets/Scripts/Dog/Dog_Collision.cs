using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Dog_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.DOG; } }
}
