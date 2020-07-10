﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapSetting : ScriptableObject {

    [Range(1,200)]
    public int width;
    [Range(1, 200)]
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
}
