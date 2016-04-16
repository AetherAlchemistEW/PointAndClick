using UnityEngine;
using UnityEditor;
using System.Collections;

//The class for handling the creation of 'Item's within the editor as assets
public class CreateItem : Editor
{
    [MenuItem("Adventure Framework/Create Item")]
    static void CreateSOItem()
    {
        string path = EditorUtility.SaveFilePanel("Create Item", "Assets/Items/", "NewItem.asset", "asset");

        if(path == "")
        {
            return;
        }

        path = FileUtil.GetProjectRelativePath(path);

        Item newItem = CreateInstance<Item>();
        AssetDatabase.CreateAsset(newItem, path);
        AssetDatabase.SaveAssets();
    }
}
