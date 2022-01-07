using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

[ExecuteInEditMode]
public class SavedItem : MonoBehaviour
{
    public bool validated;

    public int saveIndex;


#if UNITY_EDITOR
    private void OnEnable()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (SaveSorting.ItemIndexIsDuplicate(saveIndex))
            {
                saveIndex = SaveSorting.NewItemIndex();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
#endif

}
