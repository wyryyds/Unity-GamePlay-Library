using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 二维格子的结构体
/// </summary>
[System.Serializable]
public struct Coord
{
    public int x;
    public int y;
    public Coord(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }
    /// <summary>
    /// ！= 与 == 的运算符重载
    /// </summary>
    /// <param name="_c1"></param>
    /// <param name="_c2"></param>
    /// <returns></returns>
    public static bool operator ==(Coord _c1, Coord _c2) => (_c1.x == _c2.x) && (_c1.y == _c2.y);
    public static bool operator !=(Coord _c1, Coord _c2) => !(_c1 == _c2);
}
/// <summary>
/// 地图生成
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2 mapSize;
    public Transform mapHolder;
    [Range(0f, 1f)] public float outlinePercent;

    [Range(0f,100f)] public float obsMinHeight;
    [Range(0f,100f)] public float obsMaxHeight;

    public GameObject obsPrefab;
    public List<Coord> allTilesCoord = new List<Coord>();
    public Queue<Coord> shulffledQue;

    private Coord mapCenter;
    bool[,] mapObstacles;
    [Header("Map Fully Accessible")]
    [Range(0, 1)] public float obsPercent;
    private void Start()
    {
        GenerateMap();
    }
    /// <summary>
    /// 地图生成核心方法：生成障碍物
    /// </summary>
    private void GenerateMap()
    {
        for(int i=0;i<mapSize.x;i++)
            for(int j=0;j<mapSize.y;j++)
            {
                Vector3 newpos = new Vector3(-mapSize.x / 2 + 0.5f + i, 0, -mapSize.y / 2 + 0.5f + j);
                GameObject newObj = Instantiate(tilePrefab, newpos,Quaternion.Euler(90,0,0));
                newObj.transform.SetParent(mapHolder);
                newObj.transform.localScale *= (1 - outlinePercent);

                allTilesCoord.Add(new Coord(i, j));
            }

        shulffledQue = new Queue<Coord>(Utilities.ShufflesCoords(allTilesCoord.ToArray()));

        int obsCount = (int)(mapSize.x * mapSize.y * obsPercent);

        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);
        mapObstacles = new bool[(int)mapSize.x, (int)mapSize.y];

        int currentObsCount = 0;
        for(int i=0;i<obsCount;i++)
        {
            Coord randomCoord = GetRandomCoord();
            mapObstacles[randomCoord.x, randomCoord.y] = true;
            currentObsCount++;
            if(randomCoord!=mapCenter&&MapIsFullyAccessible(mapObstacles,currentObsCount))
            {
                float obsHeight = Mathf.Lerp(obsMinHeight, obsMaxHeight, Random.Range(0f, 1f));
                Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + randomCoord.x, obsHeight / 2, -mapSize.y / 2 + 0.5F + randomCoord.y);
                GameObject newObj = Instantiate(obsPrefab, newPos, Quaternion.identity);
                newObj.transform.SetParent(mapHolder);
                newObj.transform.localScale = new Vector3(1f - outlinePercent, obsHeight, 1f - outlinePercent);
            }
            else
            {
                mapObstacles[randomCoord.x, randomCoord.y] = false;
                currentObsCount--;
            }
        }


    }
    /// <summary>
    /// 是否能形成通路的判断。加入在某一格子生成障碍物之后不能行走，那就不能在此位置上生成障碍物
    /// </summary>
    /// <param name="_mapObstacles"></param>
    /// <param name="_currentObsCount"></param>
    /// <returns></returns>
    private bool MapIsFullyAccessible(bool [,] _mapObstacles,int _currentObsCount)
    {
        bool[,] mapFlags = new bool[_mapObstacles.GetLength(0), _mapObstacles.GetLength(1)];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;

        int accessibleCount = 1;

        while(queue.Count>0)
        {
            var currentTile = queue.Dequeue();
            for(int x=-1; x<=1;x++)
            {
                for(int y=-1;y<=1;y++)
                {
                    int tx = currentTile.x + x;
                    int ty = currentTile.y + y;

                    if(x==0||y==0)
                    {
                        if(tx>=0&&tx<_mapObstacles.GetLength(0)&&ty>=0&&ty<_mapObstacles.GetLength(1))
                        {
                            if(!mapFlags[tx,ty]&&!_mapObstacles[tx,ty])
                            {
                                mapFlags[tx, ty] = true;
                                accessibleCount++;
                                queue.Enqueue(new Coord(tx, ty));
                            }
                        }
                    }
                }
            }

        }
        int obsTargetCount = (int)(mapSize.x * mapSize.y - _currentObsCount);
        return accessibleCount == obsTargetCount;
    }
    /// <summary>
    /// 洗牌算法随机获取生成障碍物的格子
    /// </summary>
    /// <returns></returns>
    private Coord GetRandomCoord()
    {
        var randomCooord = shulffledQue.Dequeue();
        shulffledQue.Enqueue(randomCooord);
        return randomCooord;
    }

}
