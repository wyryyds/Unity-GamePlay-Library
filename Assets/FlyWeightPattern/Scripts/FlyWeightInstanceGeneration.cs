using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlyWeightPattern.Scripts
{
    public class FlyWeightInstanceGeneration : MonoBehaviour
    {
        [SerializeField]
        public SceneObjectGroup sceneObjectGroup;
        public int batchCount = 10;

        public Button GenButton;

        private GameObject _genRoot;


        private void Start()
        {
            GenButton.onClick.AddListener(StartGen);
        }

        private void StartGen()
        {
            StartCoroutine(StartGenerate());
        }

        private IEnumerator StartGenerate()
        {
            int initCount = 0;
            int totalCount = 0;
            if (_genRoot != null)
            {
                GameObject.Destroy(_genRoot);
            }
            _genRoot = new GameObject("root");
            while (initCount < batchCount)
            {
                initCount++;
                yield return null;
                var sceneObjects = sceneObjectGroup.SceneObjects[totalCount];
                var go = new GameObject(sceneObjects.name);
                MeshRenderer renderer = go.AddComponent<MeshRenderer>();
                MeshFilter filter = go.AddComponent<MeshFilter>();
                renderer.sharedMaterials = sceneObjects.flyweightData.materials;
                go.transform.rotation = sceneObjects.matrix.ExtractRotation();
                go.transform.position = sceneObjects.matrix.ExtractPosition();
                go.transform.localScale = sceneObjects.matrix.ExtractScale();
                go.transform.SetParent(_genRoot.transform, true);
                filter.mesh = sceneObjects.flyweightData.mesh;
                totalCount++;
                if (totalCount >= sceneObjectGroup.SceneObjects.Length)
                {
                    break;
                }

                if (initCount == batchCount)
                {
                    initCount = 0;
                }
            }
        }

    }
}