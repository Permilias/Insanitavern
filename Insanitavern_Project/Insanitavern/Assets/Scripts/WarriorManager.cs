using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorManager : MonoBehaviour
{
    public static WarriorManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject warriorObject;
    Queue<GameObject> warriorObjectsQueue;

    public void Initialize()
    {
        warriorObjectsQueue = new Queue<GameObject>();
        FillWarriorQueue();

        currentConflicts = new List<WarriorConflict>();


    }

    void FillWarriorQueue()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject newObject = Instantiate(warriorObject, transform);
            warriorObjectsQueue.Enqueue(newObject);
            newObject.SetActive(false);
        }
    }

    public void AddWarrior()
    {
        if (warriorObjectsQueue.Count < 1)
        {
            FillWarriorQueue();
        }

        Warrior newWarrior = new Warrior();

        GameObject newWarriorObject = warriorObjectsQueue.Dequeue();

        newWarriorObject.transform.position = Vector3.zero;
        newWarriorObject.SetActive(true);

        newWarrior.movement = newWarriorObject.GetComponent<CharacterMovement>();
        newWarrior.display = newWarriorObject.GetComponentInChildren<WarriorDisplay>();
        newWarrior.collision = newWarriorObject.GetComponent<CharacterCollision>();
        newWarrior.collision.warriorIndex = Registry.warriors.Count;
        newWarrior.pushForce = Registry.settings.basePushForce;
        newWarrior.display.damageObject.SetActive(false);
        newWarrior.thirst = 50;

        Registry.warriors.Add(newWarrior);
    }

    public void WarriorUpdate(int index)
    {
        Registry.warriors[index].display.beerObject.SetActive(false);

        //decrease punch timer
        if (Registry.warriors[index].punchTimer > 0)
        {
            Registry.warriors[index].punchTimer -= Time.deltaTime;
        }

        //thirst
        Registry.warriors[index].thirst += Registry.settings.baseThirstGain * Time.deltaTime;
        if (Registry.warriors[index].thirst >= Registry.settings.maxThirst) Registry.warriors[index].thirst = Registry.settings.maxThirst;
        Registry.warriors[index].display.thirstTM.text = Mathf.RoundToInt(Registry.warriors[index].thirst).ToString();

        float dist = 999;
        Vector2Int highestPriority = new Vector2Int(-1, 0);
        for (int i = 0; i < Registry.mugs.Count; i++)
        {
            #region If warrior is holding
            if (Registry.mugs[i].holdingWarrior == index)
            {
                Registry.warriors[index].display.beerObject.SetActive(true);
                Registry.warriors[index].UpdateTarget(-1);
                Registry.warriors[index].UpdateDisplay();

                if (Registry.warriors[index].drinkingTimer <= 0f)
                {
                    Registry.warriors[index].drinkingTimer = 0f;
                    BeerManager.Instance.RemoveMug(i);
                    Registry.warriors[index].thirst = 0;
                    print("finished drinking");
                }
                else
                {
                    Registry.warriors[index].drinkingTimer -= Time.deltaTime;
                }

                return;
                #endregion
            }
            else
            {
                #region If not holding, find target & target position

                //get priority
                int priority = (int)Registry.warriors[index].thirst;

                Vector3 mugPos = Registry.mugs[i].holdingWarrior > -1 ?
                    Registry.warriors[Registry.mugs[i].holdingWarrior].movement.transform.position :
                    Registry.mugs[i].mugObject.transform.position;
                dist = Vector3.Distance(mugPos, Registry.warriors[index].movement.transform.position);

                priority -= (int)(dist * Registry.settings.distanceMultiplier);

                if(priority < 20)
                {
                    continue;
                }

                if(priority >= 100)
                {
                    highestPriority.x = i;
                    highestPriority.y = priority;
                    break;
                }

                if(highestPriority.y < priority)
                {
                    highestPriority.y = priority;
                    highestPriority.x = i;
                }

                #endregion
            }
        }

        Registry.warriors[index].drinkingTimer = 0f;

        Registry.warriors[index].targetMugIndex = highestPriority.x;

        if(highestPriority.x > -1)
        {
            if (dist < Registry.settings.mugCatchRange)
            {
                if (Registry.mugs[highestPriority.x].holdingWarrior <= -1)
                {
                    GiveMugToWarrior(index, Registry.warriors[index].targetMugIndex);
                }
                else
                {
                    if (Registry.warriors[index].punchTimer <= 0f)
                    {
                        AddConflict(index, Registry.mugs[highestPriority.x].holdingWarrior);
                    }
                }

            }
        }
        else
        {
            //wander

        }

        Registry.warriors[index].UpdateTarget(index);
        Registry.warriors[index].UpdateDisplay();

    }

    public class WarriorConflict
    {
        public int attackingWarrior;
        public int attackedWarrior;

        public int attackingMugIndex;
        public int attackedMugIndex;

        public WarriorConflict(int aiw, int aew)
        {
            attackingWarrior = aiw;
            attackedWarrior = aew;
            if(Registry.warriors[attackingWarrior].drinkingTimer > 0)
            {
                attackingMugIndex = Registry.GetMugIndexByWarrior(attackingWarrior);
            }
            else
            {
                attackingMugIndex = -1;
            }
            if (Registry.warriors[attackedWarrior].drinkingTimer > 0)
            {
                attackedMugIndex = Registry.GetMugIndexByWarrior(attackedWarrior);
            }
            else
            {
                attackedMugIndex = -1;
            }

            SetIndex();
        }

        public void SetIndex()
        {
            if(attackingWarrior > attackedWarrior)
            {
                index = new Vector2Int(attackedWarrior, attackingWarrior);
            }
            else
            {
                index = new Vector2Int(attackingWarrior, attackedWarrior);
            }
        }

        public Vector2Int index;
    }

    public List<WarriorConflict> currentConflicts;

    public void AddConflict(int warrior, int collidedWarrior)
    { 
        WarriorConflict conflict = new WarriorConflict(warrior, collidedWarrior);

        bool adds = true;
        for (int i = 0; i < currentConflicts.Count; i++)
        {
            if(currentConflicts[i].index == conflict.index)
            {
                adds = false;
                print("conflict already exists");
                break;
            }
        }

        if (!adds) return;

            print("addingConflict");

        currentConflicts.Add(conflict);
    }

    public void ResolveConflicts()
    {

        for (int i = 0; i < currentConflicts.Count; i++)
        {
            ResolveConflict(i);
        }

        currentConflicts.Clear();
    }

    public void ResolveConflict(int conflictIndex)
    {
        print("resolving conflict" + conflictIndex);

        int attackingPower = Registry.settings.attackingPowerBonus;
        int attackedPower = 0;
            
   

        if (attackingPower >= attackedPower)
        {
            if(currentConflicts[conflictIndex].attackedMugIndex >= 0)
            {
                print("giving mug");
                GiveMugToWarrior(currentConflicts[conflictIndex].attackingWarrior, currentConflicts[conflictIndex].attackedMugIndex);
            }

            print("what");
            Registry.warriors[currentConflicts[conflictIndex].attackingWarrior].punchTimer = Registry.settings.punchDelay;
            Registry.warriors[currentConflicts[conflictIndex].attackedWarrior].GetPunched(PushDir(currentConflicts[conflictIndex].attackingWarrior, currentConflicts[conflictIndex].attackedWarrior));
        }
        else
        {
            print("what2");
            Registry.warriors[currentConflicts[conflictIndex].attackingWarrior].GetPunched(PushDir(currentConflicts[conflictIndex].attackedWarrior, currentConflicts[conflictIndex].attackingWarrior));
        }     
    }

    Vector2 PushDir(int pushingWarrior, int pushedWarrior)
    {
        Vector3 diff = Registry.warriors[pushedWarrior].movement.transform.position - Registry.warriors[pushingWarrior].movement.transform.position;
        diff.Normalize();
        //add random angle
        /*Vector2 toVec2 = new Vector2(diff.x, diff.z);
        toVec2 = Utility.AddRot(diff, Random.Range(-Registry.settings.pushRandomDir, Registry.settings.pushRandomDir));
        diff = new Vector3(toVec2.x, 0, toVec2.y);
        */
        diff *= Registry.warriors[pushingWarrior].pushForce;

        print(diff);
        return new Vector2(diff.x, diff.z);
    }

    public void GiveMugToWarrior(int warriorIndex, int mugIndex)
    {
        Registry.mugs[mugIndex].holdingWarrior = warriorIndex;
        Registry.warriors[warriorIndex].drinkingTimer = Registry.settings.drinkingDuration;
    }
}
