using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class Item
	{
		public uint itemId;  //uint为无符号整型。
		public string itemName;
		public uint itemPrice;
	}
	public class ItemManager : ScriptableObject
	{

		public Item[] dataArray;
	}
}
