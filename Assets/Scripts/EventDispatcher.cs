using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDispatcher {

    public static Dictionary<GameObject, Action> HitActions = new Dictionary<GameObject, Action>();

    public static void DispatchAction(GameObject go)
    {
        HitActions[go]();
    }

}
