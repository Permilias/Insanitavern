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

        Registry.warriors.Add(newWarrior);
    }

    public void WarriorUpdate(int index)
    {
        Registry.warriors[index].display.beerObject.SetActive(false);
        Registry.warriors[index].display.damageObject.SetActive(false);
        float lowest = 999;
        int chosenIndex = -1;
        bool drinking = false;
        for (int i = 0; i < Registry.mugs.Count; i++)
        {
            if (Registry.mugs[i].holdingWarrior == index)
            {
                Registry.warriors[index].display.beerObject.SetActive(true);
                drinking = true;

                Registry.warriors[index].UpdateDisplay();

                if (Registry.warriors[index].drinkingTimer <= 0f)
                {
                    BeerManager.Instance.RemoveMug(i);
                    print("finished drinking");
                    return;
                }
                else
                {
                    Registry.warriors[index].drinkingTimer -= Time.deltaTime;
                }

                break;
            }
            else
            {
                Vector3 mugPos = Registry.mugs[i].holdingWarrior > -1 ?
                    Registry.warriors[Registry.mugs[i].holdingWarrior].movement.transform.position :
                    Registry.mugs[i].mugObject.transform.position;
                float dist = Vector3.Distance(mugPos, Registry.warriors[index].movement.transform.position);
                if (dist < lowest)
                {
                    lowest = dist;
                    chosenIndex = i;
                }
            }
        }

        if (drinking) return;

        Registry.warriors[index].drinkingTimer = 0f;

        Registry.warriors[index].targetMugIndex = chosenIndex;

        if (Registry.warriors[index].catchAttemptTimer <= 0f)
        {
            if (lowest < Registry.settings.mugCatchRange)
            {
                GiveMugToWarrior(index, Registry.warriors[index].targetMugIndex);
                Registry.warriors[index].catchAttemptTimer = Registry.settings.catchAttemptDelay;
            }
        }
        else
        {
            Registry.warriors[index].catchAttemptTimer -= Time.deltaTime;
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
        print("addingConflict");

        WarriorConflict conflict = new WarriorConflict(warrior, collidedWarrior);

        bool adds = true;
        for (int i = 0; i < currentConflicts.Count; i++)
        {
            if(currentConflicts[i].index == conflict.index)
            {
                adds = false;
                break;
            }
        }

        if (adds) currentConflicts.Add(conflict);
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
        int attackingPower = Registry.settings.attackingPowerBonus;
        int attackedPower = 0;

        if (currentConflicts[conflictIndex].attackingMugIndex >= 0) return;

        if (attackingPower >= attackedPower)
        {
            if(currentConflicts[conflictIndex].attackedMugIndex >= 0)
            {
                print("giving mug");
                GiveMugToWarrior(currentConflicts[conflictIndex].attackingWarrior, currentConflicts[conflictIndex].attackedMugIndex);
            }

            print("what");
            Registry.warriors[currentConflicts[conflictIndex].attackedWarrior].movement.Push(PushDir(currentConflicts[conflictIndex].attackingWarrior, currentConflicts[conflictIndex].attackedWarrior));
        }
        else
        {
            print("what2");
            Registry.warriors[currentConflicts[conflictIndex].attackedWarrior].movement.Push(PushDir(currentConflicts[conflictIndex].attackedWarrior, currentConflicts[conflictIndex].attackingWarrior));
        }     
    }

    Vector3 PushDir(int pushingWarrior, int pushedWarrior)
    {
        Vector3 diff = Registry.warriors[pushedWarrior].movement.transform.position - Registry.warriors[pushingWarrior].movement.transform.position;
        diff.Normalize();
        return diff * 6;
    }

    public void GiveMugToWarrior(int warriorIndex, int mugIndex)
    {
        Registry.mugs[mugIndex].holdingWarrior = warriorIndex;
        Registry.warriors[warriorIndex].drinkingTimer = Registry.settings.drinkingDuration;
    }
}
