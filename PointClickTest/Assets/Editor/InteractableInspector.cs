using UnityEngine;
using UnityEditor;
using System.Collections;

//LEGACY! They show up fine anyway (just unlabeled) and the inspector does not affect derived classes
//[CustomEditor(typeof(Interactable))]
public class InteractableInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable t = (Interactable)target;

        if(t.responses.Length < 1)
        {
            t.responses = new Response[9];
        }
        for(int i = 0; i < t.responses.Length; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(((Actions)i).ToString());
            t.responses[i].success = EditorGUILayout.Toggle("Possible?", t.responses[i].success);
            t.responses[i].item = (Item)EditorGUILayout.ObjectField("Item", t.responses[i].item, typeof(Item), false);
            t.responses[i].message = EditorGUILayout.TextArea(t.responses[i].message);
            EditorGUILayout.Separator();
        }
    }
}
