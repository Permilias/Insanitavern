using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public float mugCatchRange;
    public float catchAttemptDelay;
    public float drinkingDuration;

    [Header("Conflict Settings")]
    public int attackingPowerBonus;
    public float pushStrength;
}
