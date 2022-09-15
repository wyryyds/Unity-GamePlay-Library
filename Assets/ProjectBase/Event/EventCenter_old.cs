using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///事件中心，用来进行广播、监听。利用object传参，装箱拆箱，比较浪费资源
///</summary>
public class EventCenter_old : BaseManager<EventCenter_old> 
{
	//key -- 事件的名字
	//value --对应的是 监听这个事件 对应的委托函数们
	//<object>因为数组也可以用object表示
	private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();
	/// <summary>
	/// 添加事件监听,监听者就是监听EventTrigger的，EventTrigger一执行，所有的AddEventListener就会执行
	/// 没有监听者那么就即使EventTrigger执行，也不会有谁执行
	/// </summary>
	/// <param name="name">事件的名字</param>
	/// <param name="action">准备用来处理事件的委托函数，这个其实是EventTrigger传过来的参数</param>
	public void AddEventListener(string name, UnityAction<object> action)
	{
        if (eventDic.ContainsKey(name))
        {
			eventDic[name] += action;
        }
        else
        {
			eventDic.Add(name, action);
        }
	}
	/// <summary>
	/// 移除对应的事件监听。比如玩家监听怪物死亡，如果玩家死了，那么就要移除监听，否则，一直存在引用，会造成内存泄漏
	/// </summary>
	/// <param name="name"></param>
	/// <param name="action"></param>
	public void RemoveEventListener(string name, UnityAction<object> action)
	{
        if (eventDic.ContainsKey(name))
        {
			eventDic[name] -= action;
        }
	}
	/// <summary>
	/// 事件触发，就是广播者，就是执行所有监听他的人的特定方法
	/// </summary>
	/// <param name="name">哪一个名字的事件触发了，例如name为按下某按键</param>
	/// <param name="info">哪一个按键被触发了</param>
	public void EventTrigger(string name,object info)
	{
        if (eventDic.ContainsKey(name))
        {
			//依次执行所有的name委托
			eventDic[name].Invoke(info);
			//eventDic[name](info);
		}
	}
	public void Clear()
	{
		eventDic.Clear();
	}
}

