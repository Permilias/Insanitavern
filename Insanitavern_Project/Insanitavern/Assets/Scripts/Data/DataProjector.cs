using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProjector : MonoBehaviour
{
    public Data projectedData;
    private void Update()
    {
        projectedData = DataManager.data;

    }
}
