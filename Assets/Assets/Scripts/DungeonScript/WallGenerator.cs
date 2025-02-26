using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> wallpos, TileMapVisualizer tmv)
    {
        foreach (var wall in wallpos)
        {
            tmv.PaintSingleBasicWall(wall);
        }
    }

    public static void CreatePlatforms(HashSet<Vector2Int> doorpos, TileMapVisualizer tmv)
    {
        foreach (var door in doorpos)
        {
            tmv.PaintSingleBasicDoor(door);
        }
    }
    public static void CreateSaws(HashSet<Vector2Int> sawpos, TileMapVisualizer tmv)
    {
        foreach (var saws in sawpos)
        {
            tmv.PaintSingleBasicSaw(saws);
        }
    }
}
