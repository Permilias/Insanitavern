using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior
{
    public CharacterMovement movement;
    public WarriorDisplay display;
    public CharacterCollision collision;

    public float thirst;
    public float drinkingTimer;
    public float punchTimer;

    public float pushForce;

    public void WarriorUpdate(int registryIndex)
    {
        display.beerObject.SetActive(false);

        //decrease punch timer
        if (punchTimer > 0)
        {
            punchTimer -= Time.deltaTime;
        }

        //thirst
        thirst += Registry.settings.baseThirstGain * Time.deltaTime;
        if (thirst >= Registry.settings.maxThirst) thirst = Registry.settings.maxThirst;
        display.thirstTM.text = Mathf.RoundToInt(thirst).ToString();

        MugPass(registryIndex);

        TargetUpdate(registryIndex);
        DisplayUpdate();
        SpeedUpdate();
    }

    Vector2Int highestPriority;
    float lowest;
    public void MugPass(int registryIndex)
    {
        //mug pass
        float dist = 999;
        highestPriority = new Vector2Int(-1, 0);
        for (int i = 0; i < Registry.mugs.Count; i++)
        {
            #region Warrior holding / drinking
            if (Registry.mugs[i].holdingWarrior == registryIndex)
            {
                display.beerObject.SetActive(true);

                if (drinkingTimer <= 0f)
                {
                    drinkingTimer = 0f;
                    BeerManager.Instance.RemoveMug(i);
                    thirst = 0;
                }
                else
                {
                    drinkingTimer -= Time.deltaTime;
                }

                return;
                #endregion
            }
            else
            {

                //get priority
                int priority = (int)thirst;

                Vector3 mugPos = Registry.mugs[i].holdingWarrior > -1 ?
                    Registry.warriors[Registry.mugs[i].holdingWarrior].movement.transform.position :
                    Registry.mugs[i].mugObject.transform.position;
                Vector2 mugPosVec2 = new Vector2(mugPos.x, mugPos.z);
                Vector2 warriorPos = new Vector2(Registry.warriors[registryIndex].movement.transform.position.x, Registry.warriors[registryIndex].movement.transform.position.z);
                dist = Vector2.Distance(mugPosVec2, warriorPos);

                priority -= (int)(dist * Registry.settings.distanceMultiplier);

                if (priority < 20)
                {
                    continue;
                }

                if (priority >= 100)
                {
                    highestPriority.x = i;
                    highestPriority.y = priority;
                    lowest = dist;
                    break;
                }

                if (highestPriority.y < priority)
                {
                    highestPriority.y = priority;
                    highestPriority.x = i;
                    lowest = dist;
                }
            } //warrior try to acquire mug priority
        }

        drinkingTimer = 0f;

        //if has a mug target try to catch
        if (highestPriority.x > -1)
        {
            Debug.Log("has target " + highestPriority.x + ", dist = " + dist);
            
            if (lowest < Registry.settings.mugCatchRange)
            {
                Debug.Log("under");
                if (Registry.mugs[highestPriority.x].holdingWarrior <= -1)
                {
                    WarriorManager.Instance.GiveMugToWarrior(registryIndex, highestPriority.x);
                }
                else
                {
                    if (Registry.warriors[registryIndex].punchTimer <= 0f)
                    {
                        WarriorManager.Instance.AddConflict(registryIndex, Registry.mugs[highestPriority.x].holdingWarrior);
                    }
                }

            }
        }

    }

    public void TargetUpdate(int registryIndex)
    {
        if(drinkingTimer > 0f)
        {
            movement.SetTarget(movement.transform.position);
            return;
        }

        if(highestPriority.x >= 0)
        {
            state = 1;
            if (Registry.mugs[highestPriority.x].holdingWarrior != registryIndex)
            {
                if(Registry.mugs[highestPriority.x].holdingWarrior > -1)
                {
                    movement.SetTarget(Registry.warriors[Registry.mugs[highestPriority.x].holdingWarrior].movement.transform.position);
                }
                else
                {
                    movement.SetTarget(Registry.mugs[highestPriority.x].mugObject.transform.position);
                }

                return;
            }

        }
        else
        {
            state = 0;
            wanderTimer -= Time.deltaTime;
            if(wanderTimer <= 0)
            {
                StartNewWander();
                wanderTimer = Registry.settings.wanderMinMaxDelay.RandomBetween();
            }

            return;
        }

        Debug.Log("prout4");
        movement.SetTarget(movement.transform.position);
    }
    LayerMask mask;

    //TEMPORARY
    void StartNewWander()
    {
        mask = LayerMask.GetMask("Obstacle");
        bool found = false;
        int count = 10;
        Vector2 rand = Random.insideUnitCircle;
        Vector3 wanderPos = new Vector3(rand.x, 0f, rand.y) * Registry.settings.wanderMinMaxRange.RandomBetween();
        
        while (!found)
        {
            Ray ray = new Ray(movement.transform.position + new Vector3(0, 0.1f, 0), wanderPos + new Vector3(0, 0.1f, 0));
            if (Physics.Raycast(ray, 10, 3))
            {
                rand = Random.insideUnitCircle;
                wanderPos = new Vector3(rand.x, 0f, rand.y) * Registry.settings.wanderMinMaxRange.RandomBetween();
            }
            else
            {
                found = true;
            }

            count--;
            if(count<0)
            {
                wanderPos = movement.transform.position;
                found = true;
            }
        }

        currentWanderSpeed = Registry.settings.wanderMinMaxSpeed.RandomBetween();
        movement.SetTarget(movement.transform.position + wanderPos);
    }

    int state;
    float wanderTimer, currentWanderSpeed;
    float last;
    public void DisplayUpdate()
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

    public void SpeedUpdate()
    {
        switch(state)
        {
            case 0:
                movement.speed = currentWanderSpeed;
                break;
            case 1:
                movement.speed = highestPriority.y > Registry.settings.runningThreshold ? Registry.settings.runningSpeed : Registry.settings.walkingSpeed;
                break;
        }
    }

    public void GetPunched(Vector2 dir)
    {
        movement.Push(dir);
        display.DamageFrame();
        punchTimer = Registry.settings.punchDelayAfterBeingPunched;
    }
}
