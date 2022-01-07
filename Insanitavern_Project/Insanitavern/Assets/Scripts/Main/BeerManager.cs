using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerManager : MonoBehaviour
{
    public static BeerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float mugSpawnHeight;
    public GameObject mugObject;

    Queue<GameObject> mugQueue;

    public void Initialize()
    {
        mugQueue = new Queue<GameObject>();
        FillMugQueue();
    }

    void FillMugQueue()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newMug = Instantiate(mugObject, transform);
            mugQueue.Enqueue(newMug);
            newMug.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnMug();
        }
    }

    void SpawnMug()
    {
        if(mugQueue.Count < 1)
        {
            FillMugQueue();
             
        }

        Mug newMug = new Mug();

        GameObject mugObject = mugQueue.Dequeue();
        mugObject.SetActive(true);

        Vector3 pos = MouseManager.Instance.groundPosition;
        pos.y = mugSpawnHeight;
        mugObject.transform.position = pos;

        mugObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -10, 0);

        newMug.mugObject = mugObject;
        newMug.holdingWarrior = -1;

        Registry.mugs.Add(newMug);
    }

    public void MugUpdate(int index)
    {
        if(Registry.mugs[index].mugObject.transform.position.y < -1)
        {
            RemoveMug(index);
            return;
        }

        if(Registry.mugs[index].holdingWarrior >= 0)
        {
            Registry.mugs[index].mugObject.SetActive(false);
        }
    }

    public void RemoveMug(int index)
    {
        Registry.mugs[index].mugObject.SetActive(false);
        mugQueue.Enqueue(Registry.mugs[index].mugObject);
        Registry.mugs.RemoveAt(index);
    }
}
