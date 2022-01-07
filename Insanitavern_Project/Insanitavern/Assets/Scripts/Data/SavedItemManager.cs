using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavedItemManager
{

    static SavedItem[] savedItems;

    public static void Initialize()
    {
        savedItems = GameObject.FindObjectsOfType<SavedItem>();

        InitializeAllSavedItems();
    }

    public static void InitializeAllSavedItems()
    {

        if (DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].validatedItems.Length != savedItems.Length)
        {

            DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].validatedItems = new bool[savedItems.Length];
            DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes = new int[savedItems.Length];

            for (int i = 0; i < savedItems.Length; i++)
            {
                DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes[i] = savedItems[i].saveIndex;
            }
        }
        else
        {
            for (int i = 0; i < savedItems.Length; i++)
            {
                for (int j = 0; j < DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes.Length; j++)
                {
                    if (savedItems[i].saveIndex == DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes[j])
                    {
                        savedItems[i].validated = DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].validatedItems[j];
                    }
                }
            }
        }



    }

    public static void RegisterAllValidationForScene()
    {
        for (int i = 0; i < savedItems.Length; i++)
        {
            for (int j  = 0; j < DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes.Length; j++)
            {
                if (DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].itemIndexes[j] == savedItems[i].saveIndex)
                {
                    DataManager.data.sceneDatas[DataManager.CurrentSceneDataIndex()].validatedItems[j] = savedItems[i].validated;
                }
            }
        }

    }
}
