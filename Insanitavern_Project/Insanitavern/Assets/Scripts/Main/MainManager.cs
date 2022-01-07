using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : GameSceneManager
{
    public static MainManager Instance;

    public override void InstanceSetting()
    {
        base.InstanceSetting();
        Instance = this;
    }

    public CharacterMovement test, test2;
    public override void Initialize()
    {
        base.Initialize();
        Registry.Initialize();

        WarriorManager.Instance.Initialize();
        BeerManager.Instance.Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.C))
        {
            WarriorManager.Instance.AddWarrior();
        }

        WarriorManager.Instance.ResolveConflicts();

        for (int i = 0; i < Registry.warriors.Count; i++)
        {
            WarriorManager.Instance.WarriorUpdate(i);
        }

        for (int i = 0; i < Registry.mugs.Count; i++)
        {
            BeerManager.Instance.MugUpdate(i);
        }


    }
}
