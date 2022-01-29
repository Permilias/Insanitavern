using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public float mugCatchRange;
    public float catchAttemptDelay;
    public float punchDelay;
    public float punchDelayAfterBeingPunched;
    public float drinkingDuration;

    [Header("Movement Settings")]
    public float rayStartBackMult;

    [Header("Conflict Settings")]
    public int attackingPowerBonus;
    public float basePushForce;
    public float pushResist;
    public float pushRandomDir;
}
