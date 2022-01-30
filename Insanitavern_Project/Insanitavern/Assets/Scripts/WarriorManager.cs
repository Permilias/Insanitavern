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
        Registry.warriors[index].WarriorUpdate(index);
        
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
