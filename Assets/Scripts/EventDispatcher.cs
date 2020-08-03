using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventDispatcher {

    public delegate void ChangeLifePoint(ref int a);

    public static Dictionary<GameObject, Action> OnHitActions = new Dictionary<GameObject, Action>();
    public static Dictionary<GameObject, ChangeLifePoint> DevourActoins = new Dictionary<GameObject, ChangeLifePoint>();
    public static Action<int, int, int> playerAttributeUpdate;
    public static Action<int> hitPlayer;
    public static Action<int> resurrectPlayer;

    public delegate void GenerateRoomInProcess(List<Vector2Int> vector2s);

    public static GenerateRoomInProcess GenerateRoom;

    public static void DispatchGameobjectAction(GameObject go)
    {
        OnHitActions[go]();
    }

}

public class DiffultyAdjuster:ScriptableObject
{

    public void EvaluateRoomDifficulty()
    {

    }

    public void EvaluateEnemyDensity()
    {

    }

    public void EvaluateToolDensity()
    {

    }

    public void EvaluateTrapDensity()
    {

    }

    public void EvaluateObstacleDensity()
    {

    }
}
