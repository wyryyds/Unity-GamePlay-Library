using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime;


public class BehaviorLoad : MonoBehaviour
{
    public BehaviorTree _behaviorTree;

    private void Awake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
