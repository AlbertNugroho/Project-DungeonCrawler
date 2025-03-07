using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationData", menuName = "ScriptableObjects/DecorationData", order = 1)]
public class DecorationData : ScriptableObject
{
    public DecorationType type;
    public Vector2Int size;
}
public enum DecorationType
{
    None,
    Cobweb,
    Chain,
    DanglingChain,
    BirdCage
}
public class MakeRoomDecoration : MonoBehaviour
{
    public List<DecorationData> decorationList;
    private static System.Random random = new System.Random();
    public Dictionary<Vector2Int, DecorationType> CheckUp(HashSet<Vector2Int> roomfloor, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        Dictionary<Vector2Int, DecorationType> placedDecorations = new Dictionary<Vector2Int, DecorationType>();

        foreach (var tile in roomfloor)
        {
            Vector2Int aboveTile = tile + Vector2Int.up;
            if (walls.Contains(aboveTile))
            {
                DecorationData decoration = GetRandomDecoration();
                if (decoration != null && CanPlaceDecoration(tile, decoration, walls))
                {
                    placedDecorations[tile] = decoration.type;
                }
            }
        }

        return placedDecorations;
    }

    private bool CanPlaceDecoration(Vector2Int position, DecorationData decoration, HashSet<Vector2Int> walls)
    {
        for (int x = 0; x < decoration.size.x; x++)
        {
            for (int y = 0; y < decoration.size.y; y++)
            {
                if (!walls.Contains(position + new Vector2Int(x, y)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private DecorationData GetRandomDecoration()
    {
        if (decorationList == null || decorationList.Count == 0)
            return null;
        return decorationList[random.Next(decorationList.Count)];
    }
}
