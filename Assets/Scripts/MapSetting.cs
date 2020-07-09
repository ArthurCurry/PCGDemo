using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapSetting : ScriptableObject {

    [Range(1,200)]
    public int width;
    [Range(1, 200)]
    public int height;

}
