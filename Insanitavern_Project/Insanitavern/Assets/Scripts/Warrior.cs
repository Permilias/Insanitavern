using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior
{
    public CharacterMovement movement;
    public WarriorDisplay display;
    public CharacterCollision collision;

    public float thirst;
    public int targetMugIndex = -1;
    public float drinkingTimer;
    public float punchTimer;

    public float pushForce;

    public void UpdateTarget(int registryIndex)
    {
        if(targetMugIndex >= 0)
        {
            if (Registry.mugs[targetMugIndex].holdingWarrior != registryIndex)
            {
                if(Registry.mugs[targetMugIndex].holdingWarrior > -1)
                {
                    movement.SetTarget(Registry.warriors[Registry.mugs[targetMugIndex].holdingWarrior].movement.transform.position);
                }
                else
                {
                    movement.SetTarget(Registry.mugs[targetMugIndex].mugObject.transform.position);
                }

                return;
            }

        }

        movement.SetTarget(movement.transform.position);
    }

    float last;
    public void UpdateDisplay()
    {
        if(movement.vel.x > 0.1f)
        {
            last = -1;
            display.graphicsParent.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(movement.vel.x < -0.1f)
        {
            last = 1;
            display.graphicsParent.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void GetPunched(Vector2 dir)
    {
        movement.Push(dir);
        display.DamageFrame();
        punchTimer = Registry.settings.punchDelayAfterBeingPunched;
    }
}
