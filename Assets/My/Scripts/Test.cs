using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public UnityAction myAction;
    public InputMgr im;
    private void Start()
    {
        
        myAction += Test1;
    }

    public void Test1()
    {

    }
}
