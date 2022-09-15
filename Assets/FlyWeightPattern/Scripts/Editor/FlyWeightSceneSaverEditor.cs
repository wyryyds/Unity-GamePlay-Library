using System.Collections.Generic;
using FlyWeightPattern.Scripts;
using UnityEditor;
using UnityEngine;

namespace FlyWeightPattern.ScriptsEditor
{
    [CustomEditor(typeof(FlyWeightSceneSaver))]
    public class FlyWeightSceneSaverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FlyWeightSceneSaver saver = target as FlyWeightSceneSaver;
            if (GUILayout.Button("生成"))
            {

                SceneObjectGroup sceneObjectGroup = ScriptableObject.CreateInstance<SceneObjectGroup>();
                MeshRenderer[] list = GameObject.FindObjectsOfType<MeshRenderer>(true);
                if( list.Length == 0 ) {
                    EditorUtility.DisplayDialog("警告", "你没有选中任何包含预制体的物体", "重选" );
                    return;
                }
                List<SceneObject> sceneObjects = new List<SceneObject>();
                foreach( MeshRenderer meshRenderer in list )
                {
                    MeshFilter filter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                    var transform = meshRenderer.transform;
                    var matrix = transform.localToWorldMatrix;
                    ObjectFlyweightData flyweightData = new ObjectFlyweightData()
                    {
                        materials = meshRenderer.sharedMaterials,
                        mesh = filter.sharedMesh,
                    };
                    sceneObjects.Add(new SceneObject()
                    {
                        flyweightData = flyweightData,
                        matrix = matrix,
                        name = meshRenderer.name,
                    });

                }

                sceneObjectGroup.SceneObjects = sceneObjects.ToArray();
                AssetDatabase.CreateAsset(sceneObjectGroup,  "Assets/FlyWeightPattern/sceneObjGroup.asset");
                AssetDatabase.Refresh();


            }
        }
    }
}