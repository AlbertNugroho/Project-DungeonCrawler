using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomGenerator : MonoBehaviour
{
    public static HashSet<Vector2Int> makeroomstart(HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> Platforms)
    {
        BoundsInt startbounds = new BoundsInt(new Vector3Int(-46, 0, 0), new Vector3Int(46, 150, 0));
        BoundsInt startroombounds = new BoundsInt(new Vector3Int(-41, 54, 0), new Vector3Int(23, 11, 0));

        Vector2Int start = (Vector2Int)startroombounds.min; 
        HashSet<Vector2Int> floors = new HashSet<Vector2Int>();
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        for (int i = 0; i < startroombounds.size.x; i++)
        {
            for (int j = 0; j < startroombounds.size.y; j++)
            {
                floors.Add(start + new Vector2Int(i, j));
            }
        }

        start = (Vector2Int)startbounds.min;
        
        for (int i = 0; i < startbounds.size.x; i++)
        {
            for (int j = 0; j < startbounds.size.y; j++)
            {
                walls.Add(start + new Vector2Int(i, j));
            }
        }

        walls.ExceptWith(floors);

        walls.ExceptWith(wholefloor);
        Platforms.UnionWith(MakeRoomOpening.CheckUp(floors, wholefloor, walls));
        Platforms.UnionWith(MakeRoomOpening.CheckDown(floors, wholefloor, walls));
        return walls;
    }
}
