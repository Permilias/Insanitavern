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

        Registry.warriors.Add(newWarrior);
    }

    public void WarriorUpdate(int index)
    {
        Registry.warriors[index].display.beerObject.SetActive(false);

        if (Registry.warriors[index].punchTimer > 0)
        {
            Registry.warriors[index].punchTimer -= Time.deltaTime;
        }

        float lowest = 999;
        int targetMugIndex = -1;
        bool drinking = false;
        for (int i = 0; i < Registry.mugs.Count; i++)
        {
            #region If warrior is holding
            if (Registry.mugs[i].holdingWarrior == index)
            {
                Registry.warriors[index].display.beerObject.SetActive(true);
                drinking = true;

                Registry.warriors[index].UpdateDisplay();

                if (Registry.warriors[index].drinkingTimer <= 0f)
                {
                    Registry.warriors[index].drinkingTimer = 0f;
                    BeerManager.Instance.RemoveMug(i);
                    print("finished drinking");
                    return;
                }
                else
                {
                    Registry.warriors[index].drinkingTimer -= Time.deltaTime;
                }

                break;
                #endregion
            }
            else
            {
                #region If not holding, find target & target position
                //get mug pos
                Vector3 mugPos = Registry.mugs[i].holdingWarrior > -1 ?
                    Registry.warriors[Registry.mugs[i].holdingWarrior].movement.transform.position :
                    Registry.mugs[i].mugObject.transform.position;
                float dist = Vector3.Distance(mugPos, Registry.warriors[index].movement.transform.position);
                if (dist < lowest)
                {
                    lowest = dist;
                    targetMugIndex = i;
                }
                #endregion
            }
        }

        if (drinking) return;

        Registry.warriors[index].drinkingTimer = 0f;

        Registry.warriors[index].targetMugIndex = targetMugIndex;

        if(targetMugIndex > -1)
        {
            if (lowest < Registry.settings.mugCatchRange)
            {
                if (Registry.mugs[targetMugIndex].holdingWarrior <= -1)
                {
                    GiveMugToWarrior(index, Registry.warriors[index].targetMugIndex);
                }
                else
                {
                    if (Registry.warriors[index].punchTimer <= 0f)
                    {
                        AddConflict(index, Registry.mugs[targetMugIndex].holdingWarrior);
                    }
                }

            }

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

    Vector3 PushDir(int pushingWarrior, int pushedWarrior)
    {
        Vector3 diff = Registry.warriors[pushedWarrior].movement.transform.position - Registry.warriors[pushingWarrior].movement.transform.position;
        diff.Normalize();
        diff *= Registry.warriors[pushingWarrior].pushForce;
        print(diff);
        return diff;
    }

    public void GiveMugToWarrior(int warriorIndex, int mugIndex)
    {
        Registry.mugs[mugIndex].holdingWarrior = warriorIndex;
        Registry.warriors[warriorIndex].drinkingTimer = Registry.settings.drinkingDuration;
    }
}
