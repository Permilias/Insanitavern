using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlConfig", menuName = "Config/Control")]
public class ControlConfig : ScriptableObject
{
    public KeyCode[] leftInputs, rightInputs, upInputs, downInputs;
}
