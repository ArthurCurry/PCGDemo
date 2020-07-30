using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDispatcher {

    public static Dictionary<GameObject, Action> GameobjectActions = new Dictionary<GameObject, Action>();
    public static Action<int, int, int> playerAttributeUpdate;


    public static void DispatchGameobjectAction(GameObject go)
    {
        GameobjectActions[go]();
    }

}
