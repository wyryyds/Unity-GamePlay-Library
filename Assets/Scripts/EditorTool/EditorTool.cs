using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoolTool : EditorWindow
{
    [SerializeField]
    public SerializedObject test;
    [MenuItem("Tool/PoolTool")]
    public static void WindowShow()
    {
        EditorWindow.GetWindow<PoolTool>().Show();
    }

    private void OnGUI()
    {
        
    }

    private void OnHierarchyChange()
    {
        Debug.Log("更新成功");
    }
}
