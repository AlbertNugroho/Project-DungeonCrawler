using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderGenerator
{
    public static void CreateLadder(HashSet<Vector2Int> ladderpos, TileMapVisualizer tmv)
    {
        foreach (var ladder in ladderpos)
        {
            tmv.PaintSingleLadder(ladder);
        }
    }
}
