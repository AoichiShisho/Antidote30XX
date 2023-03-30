using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public interface ITimeTravels
{
    public void SetTimePeriod(TimePeriod period);
}

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class ITimeTravelsInspector : Editor
{
    private void OnEnable()
    {
        CheckTag();
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        
        if (EditorGUI.EndChangeCheck())
        {
            CheckTag();
        }
    }

    private void CheckTag()
    {
        var targetScript = target as MonoBehaviour;
        var timeTravelsComponent = targetScript.GetComponent<ITimeTravels>();

        if (timeTravelsComponent != null)
        {
            if (targetScript.gameObject.tag != "TimeTravels")
            {
                EditorGUIUtility.SetIconForObject(targetScript.gameObject, EditorGUIUtility.IconContent("console.warnicon").image as Texture2D);
                Debug.LogWarning($"The GameObject '{targetScript.gameObject.name}' has a script that implements ITimeTravels, but it isn't tagged with 'TimeTravels'.");
            }
            else
            {
                EditorGUIUtility.SetIconForObject(targetScript.gameObject, null);
            }
        }
    }
}