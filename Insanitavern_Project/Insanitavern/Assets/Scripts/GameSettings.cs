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
    public Vector2 wanderMinMaxRange;
    public Vector2 wanderMinMaxDelay;
    public Vector2 wanderMinMaxSpeed;
    public float walkingSpeed;
    public float runningSpeed;

    [Header("Thirst Settings")]
    public float baseThirstGain;
    public float maxThirst;
    public float distanceMultiplier;
    public float runningThreshold;

    [Header("Conflict Settings")]
    public int attackingPowerBonus;
    public float basePushForce;
    public float pushResist;
    public float pushRandomDir;
}
