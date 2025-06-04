//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CreateFlyingEnemies : MonoBehaviour
//{
//    public static void checkeverytile(Dictionary<Vector2Int, HashSet<Vector2Int>> room, HashSet<Vector2Int> platforms, HashSet<Vector2Int> walls, List<GameObject> enemyPrefabs)
//    {
//        if(enemyPrefabs == null)
//        {
//            return;
//        }
//        foreach (var kvp in room)
//        {
//            HashSet<Vector2Int> spawnable = new HashSet<Vector2Int>(kvp.Value);
//            spawnable.ExceptWith(platforms);

//            List<Vector2Int> spawnableList = new List<Vector2Int>(spawnable);
//            HashSet<Vector2Int> placedEnemies = new HashSet<Vector2Int>();

//            int maxEnemies = Random.Range(1, 4); // Spawn antara 1 hingga 3 musuh
//            int enemiesSpawned = 0;

//            while (enemiesSpawned < maxEnemies && spawnableList.Count > 0)
//            {
//                Vector2Int spawnPoint = spawnableList[Random.Range(0, spawnableList.Count)];

//                if (!IsNearWall(spawnPoint, walls) && !IsNearOtherEnemies(spawnPoint, placedEnemies))
//                {
//                    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
//                    Object.Instantiate(enemyPrefab, (Vector2)spawnPoint, Quaternion.identity);
//                    placedEnemies.Add(spawnPoint);
//                    enemiesSpawned++;
//                }

//                spawnableList.Remove(spawnPoint); // Hindari memilih posisi yang sama
//            }
//        }
//    }

//    private static bool IsNearWall(Vector2Int position, HashSet<Vector2Int> walls)
//    {
//        // Cek jarak minimal 2 tile dari dinding
//        Vector2Int[] directions = {
//            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
//            new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1),
//            Vector2Int.up * 2, Vector2Int.down * 2, Vector2Int.left * 2, Vector2Int.right * 2
//        };

//        foreach (var dir in directions)
//        {
//            if (walls.Contains(position + dir))
//            {
//                return true; // Jika ada dinding dalam radius 2 tile, hindari spawn
//            }
//        }

//        return false;
//    }

//    private static bool IsNearOtherEnemies(Vector2Int position, HashSet<Vector2Int> placedEnemies)
//    {
//        // Hindari spawn dalam radius 3 tile dari musuh lain
//        foreach (var enemyPos in placedEnemies)
//        {
//            if (Vector2Int.Distance(position, enemyPos) < 3) // Minimal jarak 3 tile
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//}
