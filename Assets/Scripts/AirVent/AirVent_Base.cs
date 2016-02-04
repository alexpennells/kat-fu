using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class AirVent_Base : BaseObj {
  [Tooltip("The amount to accelerate objects on collision")]
  public float ventPower = 0.1f;
}
