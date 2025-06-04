using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateGroundEnemies : MonoBehaviour
{
    public static void checkeverytile(Dictionary<Vector2Int, HashSet<Vector2Int>> room, HashSet<Vector2Int> platforms, HashSet<Vector2Int> walls, List<GameObject> enemies)
    {
        DestroyAllWithTag("Enemies");

        foreach (var kvp in room)
        {
            List<Vector2Int> spawnablePositions = new List<Vector2Int>();
            HashSet<Vector2Int> curroom = kvp.Value;

            int minX = curroom.Min(tile => tile.x);
            int minY = curroom.Min(tile => tile.y);
            int maxX = curroom.Max(tile => tile.x);
            int maxY = curroom.Max(tile => tile.y);

            for (int i = minY; i <= maxY; i++)
            {
                Vector2Int cur = new Vector2Int(minX + 3, i);
                int safetyLimit = 500;

                while (curroom.Contains(cur) && safetyLimit-- > 0)
                {
                    Vector2Int below = new Vector2Int(cur.x, cur.y - 1);

                    if ((walls.Contains(below) || platforms.Contains(below)) && IsFourTilesAwayFromWall(cur, walls) && HasAtLeastFourFloorTiles(cur, platforms) && IsOneTileAwayFromEdge(cur, platforms))
                    {
                        spawnablePositions.Add(cur);
                    }

                    cur = new Vector2Int(cur.x + 1, i);
                }
            }

            spawnEnemies(spawnablePositions, enemies, pos => isFourTileTall(pos, walls, platforms));
        }
    }

    public static bool isFourTileTall(Vector2Int position, HashSet<Vector2Int> walls, HashSet<Vector2Int> platforms)
    {
        for (int i = 1; i <= 3; i++)
        {
            Vector2Int above = new Vector2Int(position.x, position.y + i);
            if (walls.Contains(above) || platforms.Contains(above))
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsFourTilesAwayFromWall(Vector2Int position, HashSet<Vector2Int> walls)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (walls.Contains(new Vector2Int(position.x - i, position.y)) || walls.Contains(new Vector2Int(position.x + i, position.y)))
            {
                return false;
            }
        }
        return true;
    }

    public static bool HasAtLeastFourFloorTiles(Vector2Int position, HashSet<Vector2Int> platforms)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!platforms.Contains(new Vector2Int(position.x + i, position.y - 1)))
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsOneTileAwayFromEdge(Vector2Int position, HashSet<Vector2Int> platforms)
    {
        return platforms.Contains(new Vector2Int(position.x - 1, position.y - 1)) && platforms.Contains(new Vector2Int(position.x + 1, position.y - 1));
    }

    public static void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Object.DestroyImmediate(obj);
        }
    }

    public static void spawnEnemies(List<Vector2Int> spawnable, List<GameObject> enemyPrefabs, System.Func<Vector2Int, bool> isFourTileTall)
    {
        if (spawnable.Count == 0 || enemyPrefabs.Count == 0) return;

        List<Vector2Int> placedEnemies = new List<Vector2Int>(); // Keep track of spawned enemies
        int enemyCount = Random.Range(1, Mathf.Min(spawnable.Count, 5));

        for (int i = 0; i < enemyCount; i++)
        {
            if (spawnable.Count == 0) break;

            // Find a valid spawn point that is at least 3 tiles away from existing enemies
            Vector2Int spawnPoint = new Vector2Int(0, 0);
            int maxAttempts = 10;
            bool foundValid = false;

            while (maxAttempts-- > 0)
            {
                spawnPoint = spawnable[Random.Range(0, spawnable.Count)];

                if (!isFourTileTall(spawnPoint)) continue;

                // Ensure it is at least 3 tiles away from other enemies
                if (placedEnemies.All(enemy => Vector2Int.Distance(enemy, spawnPoint) >= 3))
                {
                    foundValid = true;
                    break;
                }
            }

            if (!foundValid) continue;

            spawnable.Remove(spawnPoint);
            placedEnemies.Add(spawnPoint); // Store the spawned enemy position

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Object.Instantiate(enemyPrefab, (Vector2)spawnPoint, Quaternion.identity);
        }
    }

}
