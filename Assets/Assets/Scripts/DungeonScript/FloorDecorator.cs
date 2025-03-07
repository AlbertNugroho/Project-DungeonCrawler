using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorDecorator : MonoBehaviour
{
    public static HashSet<Vector2Int> DecorateFloor(Dictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary, HashSet<Vector2Int> wholefloor)
    {
        HashSet<Vector2Int> floordecoration = new HashSet<Vector2Int>();

        foreach (var kvp in RoomsDictionary)
        {
            HashSet<Vector2Int> roomfloor = kvp.Value;
            if (roomfloor.Count == 0) continue;

            int minX = roomfloor.Min(tile => tile.x);
            int minY = roomfloor.Min(tile => tile.y);
            int maxX = roomfloor.Max(tile => tile.x);
            int maxY = roomfloor.Max(tile => tile.y);

            int centerY = (minY + maxY) / 2;
            int offsetY = Random.Range(-1, 2); // Small random variation in Y
            Vector2Int start = new Vector2Int(minX, centerY + offsetY);

            List<Vector2Int> checklist = new List<Vector2Int>();
            Vector2Int cur = start;

            // Move right until the end of the room
            while (roomfloor.Contains(cur))
            {
                if (cur.x > minX + 1 && cur.x < maxX - 1 &&
               cur.y > minY + 1 && cur.y < maxY - 1)
                {
                    checklist.Add(cur);
                }
                cur = new Vector2Int(cur.x + 1, cur.y);
            }

            // Add the decorations for this room to the final decoration set
            floordecoration.UnionWith(RandomDecorPlacement(checklist, floordecoration));
        }

        return floordecoration;
    }

    public static HashSet<Vector2Int> RandomDecorPlacement(List<Vector2Int> checklist, HashSet<Vector2Int> existingDecorations)
    {
        int sewerMax = 3;
        HashSet<Vector2Int> final = new HashSet<Vector2Int>();

        if (checklist.Count < sewerMax) return final; // Not enough tiles

        HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < sewerMax; i++)
        {
            Vector2Int randomTile;
            int attempts = 20; // Prevent infinite loops in case of bad luck

            do
            {
                randomTile = checklist[Random.Range(0, checklist.Count)];
                attempts--;
            }
            while ((usedPositions.Any(pos => Vector2Int.Distance(pos, randomTile) < 5) ||
                    existingDecorations.Any(pos => Vector2Int.Distance(pos, randomTile) < 5)) && attempts > 0);

            if (attempts == 0) break; // If we fail too many times, stop trying

            usedPositions.Add(randomTile);
            existingDecorations.Add(randomTile); // Store globally to maintain spacing across rooms

            // Form a 2x2 tile from the selected tile
            final.Add(randomTile);
            final.Add(new Vector2Int(randomTile.x + 1, randomTile.y));
            final.Add(new Vector2Int(randomTile.x, randomTile.y + 1));
            final.Add(new Vector2Int(randomTile.x + 1, randomTile.y + 1));
        }

        return final;
    }

}
