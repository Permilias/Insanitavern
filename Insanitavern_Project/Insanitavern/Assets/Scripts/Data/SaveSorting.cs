using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
public static class SaveSorting
{
    [MenuItem("Tools/Reset Item Sorting", false, 201)]
    public static void ResetItemSorting()
    {
        Debug.Log("Reseting Item Sorting...");

        currentItemIndex = 0;

        RegisterAllItems();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    static void RegisterAllItems()
    {
        items = GameObject.FindObjectsOfType<SavedItem>();
        Debug.Log(items.Length);
        foreach (SavedItem savedItem in items)
        {
            savedItem.saveIndex = currentItemIndex;
            currentItemIndex++;
            if(PrefabUtility.IsPartOfNonAssetPrefabInstance(savedItem))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(savedItem);
            }
        }
    }

    public static int currentItemIndex;
    public static int NewItemIndex()
    {
        int returnedIndex = currentItemIndex;
        currentItemIndex++;


        return returnedIndex;
    }

    public static int[] intemIndexes;
    static SavedItem[] items;
    public static void GetAllItemIndexes()
    {
        items = GameObject.FindObjectsOfType<SavedItem>();
        intemIndexes = new int[items.Length];
        for (int i = 0; i < intemIndexes.Length; i++)
        {
            intemIndexes[i] = items[i].saveIndex;
        }
    }

    public static bool ItemIndexIsDuplicate(int indexToCheck)
    {
        GetAllItemIndexes();
        int check = 1;
        for (int i = 0; i < intemIndexes.Length; i++)
        {
            if (intemIndexes[i] == indexToCheck)
            {
                if (check == 1) check = 0;
                else if (check == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
#endif
