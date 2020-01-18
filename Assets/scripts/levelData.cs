using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class levelData
{
    public float[,] positions;
    public float[,] scales;
    public float[] zRotations;
    public string[] names;
    public levelData(LevelGeneratorManager data)
    {
        positions = data.positions;
        scales = data.scales;
        zRotations = data.zRotations;
        names = data.names;
    }
}
