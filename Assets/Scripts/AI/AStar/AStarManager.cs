using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarManager : MonoBehaviour
{
    public int col=10;
    public int row=10;
    public GameObject AstarNodePrefab;

    public AstarNode[,] AstarNodes;
    public List<AstarNode> OpenList = new List<AstarNode>();
    public List<AstarNode> CloseList = new List<AstarNode>();
    public List<AstarNode> path = new List<AstarNode>();

    public AstarNode startNode, endNode;
    private void Start()
    {
        AstarNodes = new AstarNode[col, row];
        for(int y=0;y<row;y++)
            for(int x=0;x<col;x++)
            {
                var objNode=Instantiate(AstarNodePrefab, new Vector3(x, y), Quaternion.identity);
                objNode.transform.SetParent(transform, false);
                objNode.transform.name = $"Astar{x}:{y}";
                AstarNode node = objNode.GetComponent<AstarNode>();
                if (node)
                {
                    node.x = x;node.y = y;
                    AstarNodes[x, y] = node;
                }
            }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ShowAstar();
    }
    public List<AstarNode> GetSuccess(AstarNode node)
    {
        List<AstarNode> list = new List<AstarNode>();
        var tempNode = node;
        while(tempNode.parent!=null)
        {
            list.Add(tempNode);
            tempNode = tempNode.parent;
        }
        return list;
    }

    public void ShowAstar()
    {
        path = StartAstar();
        if(path!=null)
        {
            for (int i = 0; i < path.Count; i++)if(!path[i].isEnd) path[i].clickCount = 4;
        }
    }
    /// <summary>
    /// 开始寻路
    /// </summary>
    public List<AstarNode>  StartAstar()
    {
        for(int y=0;y<row;y++)
            for(int x=0;x<col;x++)
            {
                if (AstarNodes[x, y].isStart) startNode = AstarNodes[x, y];
                if (AstarNodes[x, y].isEnd) endNode = AstarNodes[x, y];
            }
        OpenList.Clear();
        CloseList.Clear();
        OpenList.Add(startNode);
        startNode.GetCost(null, endNode);
        while(OpenList.Count>0)
        {
            var node= GetMinCostNode();
            if(node.x==endNode.x&&node.y==endNode.y)
            {
                return GetSuccess(node);
            }
            else
            {
                List<AstarNode> neighbors = GetNeighbors(node);
                for(int i=0;i<neighbors.Count;i++)
                {

                    if (neighbors[i] == null) continue;
                    var tempNode = neighbors[i];

                    var newCost = node.g + AstarNode.EvaluationCost(node, tempNode);

                    var isInOpenList = CheckInList(tempNode, OpenList);
                    var isInCloseList = CheckInList(tempNode, CloseList);
                    if ((isInOpenList || isInCloseList) && tempNode.g < newCost) continue;
                    else
                    {
                        tempNode.parent = node;
                        tempNode.g = newCost;
                        tempNode.h = AstarNode.EvaluationCost(tempNode, endNode);
                        tempNode.f = tempNode.g + tempNode.h;
                        if(isInCloseList)CloseList.Remove(tempNode);
                        if (!isInOpenList) { OpenList.Add(tempNode); if(!tempNode.isEnd)tempNode.clickCount = 5; }
                    }
                }    
            }
            CloseList.Add(node);
        }
        return null;
    }
    /// <summary>
    /// 检查是否在列表中
    /// </summary>
    /// <param name="node"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool CheckInList(AstarNode node,List<AstarNode> list)
    {
        if (list.Count == 0) return false;
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].x == node.x && list[i].y == node.y) return true;
        }
        return false;
    }
    /// <summary>
    /// 获取最小代价的节点
    /// </summary>
    /// <returns></returns>
    public AstarNode GetMinCostNode()
    {
        var node = OpenList[0];
        for(int i=1;i<OpenList.Count;i++)
        {
            node=OpenList[i].f<node.f?OpenList[i]:node;
        }
        OpenList.Remove(node);
        return node;
    }
    /// <summary>
    /// 获取周围节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<AstarNode> GetNeighbors(AstarNode node)
    {
        List<AstarNode> nodes = new List<AstarNode>();
        int x = node.x, y = node.y;
        nodes.Add(GetNode(x - 1, y));
        nodes.Add(GetNode(x + 1, y));
        nodes.Add(GetNode(x ,y - 1));
        nodes.Add(GetNode(x ,y + 1));
        nodes.Add(GetNode(x - 1, y-1));
        nodes.Add(GetNode(x - 1, y+1));
        nodes.Add(GetNode(x + 1, y-1));
        nodes.Add(GetNode(x + 1, y+1));
        for(int i=nodes.Count-1;i>=0;i--)
        {
            if (nodes[i] == null) nodes.RemoveAt(i);
        }
        return nodes;
    }
    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private AstarNode GetNode(int x,int y)
    {
        if (x < 0 || x >= col || y < 0 || y >= row) return null;
        if (AstarNodes[x, y].isObstacle) return null;
        return AstarNodes[x, y];
    }
}
