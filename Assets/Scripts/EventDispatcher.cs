using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventDispatcher {

    public delegate void ChangeLifePoint(ref int a);
    //public delegate void SetActiveGameObjects(params GameObject[] gameObjects);

    public static Dictionary<GameObject, Action> OnHitActions = new Dictionary<GameObject, Action>();
    public static Dictionary<GameObject, ChangeLifePoint> DevourActoins = new Dictionary<GameObject, ChangeLifePoint>();
    public static Action<int, int, int> playerAttributeUpdate;
    public static Action<int> hitPlayer;
    public static Action<int> resurrectPlayer;
    public static Action OnBossDead;
    public static Action OnPlayerDead;
    public static string bossDebuffTips="BossDebuffs:";


    public static List<Action<Boss>> bossDebuffActions=new List<Action<Boss>>();
    //public static SetActiveGameObjects SetUIsActivated;

    public delegate void GenerateRoomInProcess(List<Vector2Int> vector2s);

    public static GenerateRoomInProcess GenerateRoom;

    public static void DispatchGameobjectAction(GameObject go)
    {
        OnHitActions[go]();
    }

}

public class DiffultyAdjuster:ScriptableObject
{
    public delegate void UpdatePlayerAttribute(out int hp,out int dp,out int speed);

    public static UpdatePlayerAttribute GetPlayerAttribute;
    public static float difficultyDegree=1f;
    public static int totalEnemyHP;
    public static int playerHP;
    public static int playerDP;
    
    /// <summary>
    /// 乘以100后的粗略值
    /// </summary>
    public static int playerSpeed;
    public static int playerHitRate;
    public static int playerHitTimes;
    public static int playerAttackTimes;

    public void EvaluateRoomDifficulty()
    {
        
    }

    public void EvaluateEnemyDensity()
    {

    }

    public static ToolPotion.PotionType EvaluatePotionType()
    {
        ToolPotion.PotionType[] allPotions=(ToolPotion.PotionType[])Enum.GetValues(typeof(ToolPotion.PotionType));
        ToolPotion.PotionType temp = allPotions[UnityEngine.Random.Range(0,allPotions.Length)];
        GetPlayerAttribute(out playerHP, out playerDP, out playerSpeed);
        if (playerAttackTimes > 0)
            playerHitRate = playerHitTimes * 100 / playerAttackTimes;
        else
            playerHitRate = 0;
        Enemy[] enemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
        int enemiesHp = 0;
        int averageCost = 0;
        foreach(Enemy enemy in enemies)
        {
            //if(!enemy.gameObject.name.Contains("Boss"))
                enemiesHp += enemy.hp;
        }
        averageCost = enemiesHp / enemies.Length+1;
        if(playerHitRate==0||playerHP<=enemiesHp)
        {
            temp = ToolPotion.PotionType.Blood;
        }
        //Debug.Log(averageCost * 100 / playerHitRate + 1);
        //Debug.Log(playerHP + " " + playerDP + " " + playerSpeed);
        Debug.Log((playerHP <= (averageCost * 100 / playerHitRate + 1)) + " " + playerHitRate + " attacktimes:" + playerAttackTimes + "  hittimes:" + playerHitTimes + " averagecost" + averageCost + " effectiveHP:" + averageCost * 100 / playerHitRate + 1 + " playerhp:" + playerHP);
        Debug.Log(temp);
        return temp;
    }

    public void EvaluateTrapDensity()
    {

    }

    public void EvaluateObstacleDensity()
    {

    }
}
