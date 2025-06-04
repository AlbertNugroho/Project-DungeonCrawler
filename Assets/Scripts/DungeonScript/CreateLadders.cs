using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLadders : MonoBehaviour
{
    public static HashSet<Vector2Int> CreateLadder(HashSet<Vector2Int> ladderpoints, HashSet<Vector2Int> platform, Dictionary<Vector2Int, HashSet<Vector2Int>> RoomDictionary)
    {
        HashSet<Vector2Int> ladders = new HashSet<Vector2Int>();

        foreach (var kvp in RoomDictionary)
        {
            HashSet<Vector2Int> room = kvp.Value;

            foreach (var startPoint in ladderpoints)
            {
                Vector2Int point = startPoint; // Gunakan salinan agar tidak merusak iterasi
                ladders.Add(new Vector2Int(point.x, point.y + 1));
                ladders.Add(new Vector2Int(point.x, point.y + 2));
                while (true)
                {
                    if (platform.Contains(point))
                    {
                        break; // Hentikan jika bertemu platform
                    }
                    if (!room.Contains(point))
                    {
                        break; // Hentikan jika point keluar dari ruangan
                    }
                    ladders.Add(point);
                    point = new Vector2Int(point.x, point.y - 1); // Turun ke bawah
                }
            }
        }
        return ladders;
    }

}
