using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public uint itemdID;
        public string itemName;
        public uint itemPrice;
    }

    public class ItemManager : ScriptableObject
    {
        public Item[] dataArray;
    }
}
