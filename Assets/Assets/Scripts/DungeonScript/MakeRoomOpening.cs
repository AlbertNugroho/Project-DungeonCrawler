using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeRoomOpening
{
    public static HashSet<Vector2Int> GeneratePlatforms(Dictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> platforms = new HashSet<Vector2Int>();
        foreach (var kvp in RoomsDictionary)
        {
            HashSet<Vector2Int> roomfloor = kvp.Value;
            platforms.UnionWith(CheckUp(roomfloor, wholefloor, walls));
            platforms.UnionWith(CheckDown(roomfloor, wholefloor, walls));
            platforms.UnionWith(CheckCorners(roomfloor, wholefloor, walls));
        }
        return platforms;
    }

    public static HashSet<Vector2Int> GenerateSaws(Dictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> saws = new HashSet<Vector2Int>();
        foreach (var kvp in RoomsDictionary)
        {
            HashSet<Vector2Int> roomfloor = kvp.Value;
            saws.UnionWith(CheckSawsDown(roomfloor, wholefloor, walls));
        }
        return saws;
    }

    public static HashSet<Vector2Int> CheckCorners(HashSet<Vector2Int> roomfloor, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> temp = new HashSet<Vector2Int>();

        Vector2Int upperLeft = RoomCorners.GetUpperLeftCoordinate(roomfloor);
        Vector2Int upperRight = RoomCorners.GetUpperRightCoordinate(roomfloor);
        Vector2Int bottomLeft = RoomCorners.GetLeftBottomCoordinate(roomfloor);
        Vector2Int bottomRight = RoomCorners.GetRightBottomCoordinate(roomfloor);

        Vector2Int[] candidates =
        {
            upperLeft + new Vector2Int(-1, 1),
            upperRight + new Vector2Int(1, 1),
            bottomLeft + new Vector2Int(-1, -1),
            bottomRight + new Vector2Int(1, -1)
        };

        foreach (var candidate in candidates)
        {
            if (wholefloor.Contains(candidate) && PlatformAvailability(walls, candidate))
                temp.Add(candidate);
        }
        return temp;
    }

    public static HashSet<Vector2Int> CheckUp(HashSet<Vector2Int> roomfloor, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> temp = new HashSet<Vector2Int>();
        Vector2Int upperLeft = RoomCorners.GetUpperLeftCoordinate(roomfloor);
        Vector2Int upperRight = RoomCorners.GetUpperRightCoordinate(roomfloor);

        for (int i = upperLeft.x; i <= upperRight.x; i++)
        {
            Vector2Int candidate = new Vector2Int(i, upperLeft.y + 1);
            if (wholefloor.Contains(candidate) && PlatformAvailability(walls, candidate))
            {
                temp.Add(candidate);
            }
        }
        return temp;
    }

    public static HashSet<Vector2Int> CheckDown(HashSet<Vector2Int> roomfloor, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> temp = new HashSet<Vector2Int>();
        Vector2Int bottomLeft = RoomCorners.GetLeftBottomCoordinate(roomfloor);
        Vector2Int bottomRight = RoomCorners.GetRightBottomCoordinate(roomfloor);

        for (int i = bottomLeft.x; i <= bottomRight.x; i++)
        {
            Vector2Int candidate = new Vector2Int(i, bottomLeft.y - 1);
            if (wholefloor.Contains(candidate) && PlatformAvailability(walls, candidate))
            {
                temp.Add(candidate);
            }
        }
        return temp;
    }

    public static HashSet<Vector2Int> CheckSawsDown(HashSet<Vector2Int> roomfloor, HashSet<Vector2Int> wholefloor, HashSet<Vector2Int> walls)
    {
        HashSet<Vector2Int> saws = new HashSet<Vector2Int>();
        Vector2Int bottomLeft = RoomCorners.GetLeftBottomCoordinate(roomfloor);
        Vector2Int bottomRight = RoomCorners.GetRightBottomCoordinate(roomfloor);

        for (int i = bottomLeft.x; i <= bottomRight.x; i++)
        {
            Vector2Int candidate = new Vector2Int(i, bottomLeft.y - 1);
            if (wholefloor.Contains(candidate) && !PlatformAvailability(walls, candidate))
            {
                saws.Add(candidate + new Vector2Int(0, 0));
            }
        }
        return saws;
    }

    public static bool PlatformAvailability(HashSet<Vector2Int> walls, Vector2Int platformpos)
    {
        if (walls.Contains(platformpos + Vector2Int.up) ||
            walls.Contains(platformpos + Vector2Int.down))
        {
            return false;
        }

        bool mainWallAt2 = walls.Contains(platformpos + Vector2Int.up * 2) ||
                           walls.Contains(platformpos + Vector2Int.down * 2);

        if (mainWallAt2)
        {
            if (walls.Contains(platformpos + (Vector2Int.up + Vector2Int.left) * 2) ||
                walls.Contains(platformpos + (Vector2Int.up + Vector2Int.right) * 2) ||
                walls.Contains(platformpos + (Vector2Int.down + Vector2Int.left) * 2) ||
                walls.Contains(platformpos + (Vector2Int.down + Vector2Int.right) * 2))
            {
                return false;
            }
        }

        return true;
    }
}

public static class RoomCorners
{
    public static Vector2Int GetLeftBottomCoordinate(HashSet<Vector2Int> coordinates) =>
        coordinates.Aggregate((a, b) => (b.x < a.x || (b.x == a.x && b.y < a.y)) ? b : a);

    public static Vector2Int GetUpperLeftCoordinate(HashSet<Vector2Int> coordinates) =>
        coordinates.Aggregate((a, b) => (b.x < a.x || (b.x == a.x && b.y > a.y)) ? b : a);

    public static Vector2Int GetRightBottomCoordinate(HashSet<Vector2Int> coordinates) =>
        coordinates.Aggregate((a, b) => (b.x > a.x || (b.x == a.x && b.y < a.y)) ? b : a);

    public static Vector2Int GetUpperRightCoordinate(HashSet<Vector2Int> coordinates) =>
        coordinates.Aggregate((a, b) => (b.x > a.x || (b.x == a.x && b.y > a.y)) ? b : a);
}