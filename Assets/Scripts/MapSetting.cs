using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapSetting : ScriptableObject {

    public string seed;
    [Range(1,500)]
    public int width;
    [Range(1, 500)]
    public int height;

    /// <summary>
    /// 地图中空白部分的比例
    /// </summary>
    [Range(0, 100)]
    public int percentage;
    [Range(10, 100)]
    public int minRoomWidth;
    [Range(10, 100)]
    public int minRoomHeight;
    [Range(0,100)]
    public int BSPIterationTimes;
    [Range(0, 10)]
    public int passageWidth;
    [Range(0, 10)]
    public int corridorWidth;

    [Range(0, 20)]
    public int maxMonsterNum;
    [Range(0, 100)]
    public int trapPercentage;
    public int trapBlockNum;
    [Range(0, 100)]
    public int wallTrapPercentage;

    [Range(1,50)]
    [Tooltip("障碍物区块的最大数量")]
    public int obstacleBlockNums;

    [Range(0, 100)]
    [Tooltip("障碍物数量占比")]
    public int obstacleBlockPercentage;

    [Range(0, 8)]
    [Tooltip("孤立障碍块的最小数量")]
    public int minObstacleSize;


    [Range(0, 100)]
    [Tooltip("战利品房间内奖励道具的数量占比")]
    public int lootRoomGadgetPercentage;

    [Range(0, 100)]
    [Tooltip("其它房间内的道具数量占比")]
    public int gadgetPercentage;

    [Range(0, 100)]
    public int caveRoomFloorPercentage;
    [Range(0, 10)]
    public int caveNum;
    public Vector2Int minHollowSize;
    public Vector2Int maxHollowSize;


    public int floorBlockNum;
    public Vector2Int minFloorBlockSize;

    [Range(0, 100)]
    [Tooltip("敌人的数量占比")]
    public int enemyPercentage;
    [Range(0, 50)]
    [Tooltip("单个房间内最大敌人数量")]
    public int maxEnemyNum;

    public bool trapActivated;

    public AnimationCurve RoomTypePercentage;

    public AnimationCurve TileTypePercentageCurve;

    public static Dictionary<string, System.Random> seeds = new Dictionary<string, System.Random>();
}
