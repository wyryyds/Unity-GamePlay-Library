using System;
using UnityEngine;

namespace FlyWeightPattern.Scripts
{
    [Serializable]
    public struct ObjectFlyweightData
    {
        public Mesh mesh;//相同的mesh在导入模型后自然就只存在一份，享元中的共享就体现在这里
        public Material[] materials;//这里的材质也是一样的，仅存在一份，这是由模型资源决定的
    }
    
    [Serializable]
    public struct SceneObject
    {
        public ObjectFlyweightData flyweightData;
        public Matrix4x4 matrix;
        public string name;
    }

    public class SceneObjectGroup : ScriptableObject
    {
        public SceneObject[] SceneObjects;
    }

}