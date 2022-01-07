using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    public int warriorIndex = -1;

    

    private void OnCollisionStay(Collision collision)
    { 
        if(collision.gameObject.layer == 6)
        {
            int collidedWarriorIndex = Registry.GetWarriorIndexByPosition(collision.transform.position);
            if(collidedWarriorIndex < 0)
            {
                return;
            }

            WarriorManager.Instance.AddConflict(warriorIndex, collidedWarriorIndex);
        }
    }
}
