using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDungeonLockedDoor : MonoBehaviour
{
    public static void makelastdoor(GameObject prefab, HashSet<Vector2Int> wholefloor)
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            DestroyImmediate(door);
        }

        bool flag = false;
        Vector2Int pos = new Vector2Int(148, 0);
        HashSet<Vector2Int> points = new HashSet<Vector2Int>();
        int missingCounter = 0;
        for (int y = 0; y <= 149; y++)
        {
            Vector2Int checkPos = new Vector2Int(pos.x, y);
            if (wholefloor.Contains(checkPos)&&!flag)
            {
                points.Add(checkPos);
                flag = true;
                missingCounter = 0; // Reset counter saat lantai ditemukan
            }
            else
            {
                missingCounter++;

                if (missingCounter >= 4) // Jika ada 5 blok kosong berturut-turut
                {
                    flag = false;
                }
            }
        }

        foreach (var point in points)
        {
            Instantiate(prefab, new Vector3(point.x, point.y + 2.5f, 0), Quaternion.identity);
        }
    }
}
