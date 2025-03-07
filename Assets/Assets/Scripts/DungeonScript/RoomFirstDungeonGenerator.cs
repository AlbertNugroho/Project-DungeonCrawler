using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class RoomFirstDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private int minroomwidth = 5, minroomheight = 5;
    [SerializeField]
    private int dungeonwidth = 20, dungeonheight = 20;
    [SerializeField]
    private int yoffset = 2;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    protected Vector2Int startposition = Vector2Int.zero;
    [SerializeField]
    private TileMapVisualizer visualizer;

    // Menyimpan setiap ruangan menggunakan pusat ruangan sebagai key dan posisi lantainya sebagai value.
    public Dictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    [SerializeField]
    private int brushWidth = 2, brushHeight = 2;
    private HashSet<Vector2Int> floorPositions, corridorPositions;

    public void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        // Membagi area dungeon menjadi ruangan menggunakan Binary Space Partitioning.
        var roomslist = BinarySpacePartitioning(new BoundsInt((Vector3Int)startposition, new Vector3Int(dungeonwidth, dungeonheight, 0)), minroomwidth, minroomheight);
        HashSet<Vector2Int> floordeco = new HashSet<Vector2Int>();
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        HashSet<Vector2Int> platforms = new HashSet<Vector2Int>();
        HashSet<Vector2Int> saws = new HashSet<Vector2Int>();
        HashSet<Vector2Int> ladders = new HashSet<Vector2Int>();
        // Membuat ruangan sederhana dari list yang sudah dibagi.
        floor = CreateSimpleRooms(roomslist, platforms);

        // Buat list pusat ruangan.
        List<Vector2Int> roomcenter = new List<Vector2Int>();
        foreach (var room in roomslist)
        {
            if ((int)room.center.y > (int)room.min.y + yoffset)
            {
                roomcenter.Add(new Vector2Int((int)room.center.x, (int)room.min.y + yoffset + offset));
            }
            else
            {
                roomcenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            }
        }

        roomcenter.Add(new Vector2Int(-29, 56));
        HashSet<Vector2Int> corridors = Connectrooms(roomcenter);
        floor.UnionWith(corridors);

        walls = CreateWallsAroundRooms(floor, dungeonwidth, dungeonheight);
        walls.UnionWith(StartRoomGenerator.makeroomstart(floor, platforms));
        platforms.UnionWith(MakeRoomOpening.GeneratePlatforms(RoomsDictionary, floor, walls));
        saws = MakeRoomOpening.GenerateSaws(RoomsDictionary, floor, walls);
        floordeco = FloorDecorator.DecorateFloor(RoomsDictionary, floor);
        visualizer.PaintFloortiles(floor);
        WallGenerator.CreateSaws(saws, visualizer);
        WallGenerator.CreatePlatforms(platforms, visualizer);
        WallGenerator.CreateWalls(walls, visualizer);
        WallGenerator.CreateFloorDeco(floordeco, visualizer);
        LadderGenerator.CreateLadder(ladders, visualizer);

        // --- Menentukan ruangan spesial dengan Dijkstra ---

        // Bangun graph ruangan dari RoomsDictionary.
        var roomGraph = BuildRoomGraph();

        // Menggunakan ruangan pertama sebagai ruangan mulai.
        Vector2Int startRoom = RoomsDictionary.Keys.First();

        // Dapatkan jarak dari ruangan mulai.
        var distancesFromStart = DijkstraDistances(roomGraph, startRoom);

        // Tentukan ruangan boss (ruangan yang paling jauh dari ruangan mulai).
        Vector2Int bossRoom = GetFurthestRoom(distancesFromStart);
        Debug.Log("Boss Room: " + bossRoom);

        // Dapatkan jarak dari ruangan boss.
        var distancesFromBoss = DijkstraDistances(roomGraph, bossRoom);

        // Tentukan ruangan harta (ruangan yang paling jauh dari ruangan boss).
        Vector2Int treasureRoom = GetFurthestRoom(distancesFromBoss);
        Debug.Log("Treasure Room: " + treasureRoom);
    }

    private HashSet<Vector2Int> Connectrooms(List<Vector2Int> roomcenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomcenter[Random.Range(0, roomcenter.Count)];
        roomcenter.Remove(currentRoomCenter);

        while (roomcenter.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomcenter);
            roomcenter.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        corridorPositions = new HashSet<Vector2Int>(corridors);
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        // Sedikit variasi dalam menentukan titik tujuan.
        destination.x += Random.Range(-minroomwidth / 2 + brushWidth, minroomwidth / 2 - brushWidth);
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
                AddBrush(corridor, position, brushWidth, brushHeight);
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
                AddBrush(corridor, position, brushWidth, brushHeight);
            }
        }
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
                AddBrush(corridor, position, brushWidth, brushHeight);
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
                AddBrush(corridor, position, brushWidth, brushHeight);
            }
        }

        return corridor;
    }

    private void AddBrush(HashSet<Vector2Int> set, Vector2Int center, int brushWidth, int brushHeight)
    {
        int offsetX = brushWidth / 2;
        int offsetY = brushHeight / 2;

        int startX = (brushWidth % 2 == 0) ? center.x - offsetX + 1 : center.x - offsetX;
        int startY = (brushHeight % 2 == 0) ? center.y - offsetY + 1 : center.y - offsetY;

        for (int x = 0; x < brushWidth; x++)
        {
            for (int y = 0; y < brushHeight; y++)
            {
                set.Add(new Vector2Int(startX + x, startY + y));
            }
        }
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomcenter)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomcenter)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomslist, HashSet<Vector2Int> Platforms)
    {
        HashSet<Vector2Int> overallFloor = new HashSet<Vector2Int>();

        foreach (var room in roomslist)
        {
            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    roomFloor.Add(position);
                    overallFloor.Add(position);
                }
            }

            PlacePlatforms(room, Platforms);

            SaveRoomData(new Vector2Int((int)room.center.x, (int)room.center.y), roomFloor);
        }
        return overallFloor;
    }

    private void PlacePlatforms(BoundsInt mainRoom, HashSet<Vector2Int> platform)
    {
        int minPlatformLength = 6;
        int maxPlatformLength = 10;
        int sameYSpacing = 3; // Jarak minimal antar platform jika X berbeda
        int minXWallSpacing = 2 + offset;
        int minYWallSpacing = 2 + offset;
        int maxPlatforms = Mathf.Max(5, mainRoom.size.y / 6);

        List<RectInt> placedPlatforms = new List<RectInt>();

        for (int i = 0; i < maxPlatforms; i++) // Pakai `<` bukan `<=`
        {
            int platformWidth = Random.Range(minPlatformLength, maxPlatformLength);
            int startX = Random.Range(mainRoom.xMin + minXWallSpacing, mainRoom.xMax - platformWidth - minXWallSpacing);
            int startY = Random.Range(mainRoom.yMin + minYWallSpacing + 1, mainRoom.yMax - minYWallSpacing - 1);

            RectInt newPlatform = new RectInt(startX, startY, platformWidth, 1);

            bool overlaps = placedPlatforms.Any(p =>
                (p.y == newPlatform.y && p.xMax >= newPlatform.xMin && p.xMin <= newPlatform.xMax) || // Overlap horizontal
                (p.y != newPlatform.y && Mathf.Abs(p.y - newPlatform.y) < sameYSpacing)); // Overlap vertikal

            if (!overlaps)
            {
                placedPlatforms.Add(newPlatform);
                for (int x = 0; x < platformWidth; x++)
                {
                    platform.Add(new Vector2Int(startX + x, startY));
                }
            }
        }
    }


    private void SaveRoomData(Vector2Int center, HashSet<Vector2Int> floor)
    {
        RoomsDictionary[center] = floor;
    }

    private HashSet<Vector2Int> CreateWallsAroundRooms(HashSet<Vector2Int> floor, int dungeonwidth, int dungeonheight)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        for (int col = 0; col < dungeonwidth; col++)
        {
            for (int row = 0; row < dungeonheight; row++)
            {
                Vector2Int position = new Vector2Int(col, row);
                walls.Add(position);
            }
        }
        walls.ExceptWith(floor);
        return walls;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minwidth, int minheight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minheight && room.size.x >= minwidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minheight * 2)
                    {
                        SplitHorizontally(minheight, roomsQueue, room);
                    }
                    else if (room.size.x >= minwidth * 2)
                    {
                        SplitVertically(minwidth, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minwidth * 2)
                    {
                        SplitVertically(minwidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minheight * 2)
                    {
                        SplitHorizontally(minheight, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVertically(int minwidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minheight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private Dictionary<Vector2Int, Dictionary<Vector2Int, float>> BuildRoomGraph()
    {
        var graph = new Dictionary<Vector2Int, Dictionary<Vector2Int, float>>();
        foreach (var roomKey in RoomsDictionary.Keys)
        {
            if (!graph.ContainsKey(roomKey))
                graph[roomKey] = new Dictionary<Vector2Int, float>();

            foreach (var otherRoomKey in RoomsDictionary.Keys)
            {
                if (roomKey == otherRoomKey)
                    continue;
                float weight = Vector2.Distance(roomKey, otherRoomKey);
                graph[roomKey][otherRoomKey] = weight;
            }
        }
        return graph;
    }

    private Dictionary<Vector2Int, float> DijkstraDistances(
        Dictionary<Vector2Int, Dictionary<Vector2Int, float>> graph,
        Vector2Int startRoom)
    {
        var distances = new Dictionary<Vector2Int, float>();
        var comparer = Comparer<(float, Vector2Int)>.Create((a, b) =>
        {
            int cmp = a.Item1.CompareTo(b.Item1);
            if (cmp == 0)
                cmp = a.Item2.x.CompareTo(b.Item2.x);
            if (cmp == 0)
                cmp = a.Item2.y.CompareTo(b.Item2.y);
            return cmp;
        });
        var priorityQueue = new SortedSet<(float, Vector2Int)>(comparer);

        foreach (var room in graph.Keys)
        {
            distances[room] = float.MaxValue;
        }
        distances[startRoom] = 0;
        priorityQueue.Add((0, startRoom));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Min;
            priorityQueue.Remove(current);
            Vector2Int currentRoom = current.Item2;

            foreach (var neighbor in graph[currentRoom])
            {
                Vector2Int neighborRoom = neighbor.Key;
                float weight = neighbor.Value;
                float newDist = distances[currentRoom] + weight;
                if (newDist < distances[neighborRoom])
                {
                    var oldEntry = (distances[neighborRoom], neighborRoom);
                    if (priorityQueue.Contains(oldEntry))
                        priorityQueue.Remove(oldEntry);

                    distances[neighborRoom] = newDist;
                    priorityQueue.Add((newDist, neighborRoom));
                }
            }
        }
        return distances;
    }

    private Vector2Int GetFurthestRoom(Dictionary<Vector2Int, float> distances)
    {
        Vector2Int furthestRoom = Vector2Int.zero;
        float maxDistance = 0;
        foreach (var kvp in distances)
        {
            if (kvp.Value > maxDistance && kvp.Value != float.MaxValue)
            {
                maxDistance = kvp.Value;
                furthestRoom = kvp.Key;
            }
        }
        return furthestRoom;
    }

    private Color GetRoomColor(int roomId)
    {
        Random.InitState(roomId); // Set a fixed seed
        return new Color(Random.value, Random.value, Random.value, 0.5f);
    }

    //void OnDrawGizmos()
    //{
    //    if (RoomsDictionary == null || RoomsDictionary.Count == 0)
    //        return;

    //    foreach (var room in RoomsDictionary)
    //    {
    //        int minX = room.Value.Min(tile => tile.x);
    //        int maxX = room.Value.Max(tile => tile.x);
    //        int minY = room.Value.Min(tile => tile.y);
    //        int maxY = room.Value.Max(tile => tile.y);

    //        Vector3 center = new Vector3((minX + maxX) / 2f + 0.5f, (minY + maxY) / 2f + 0.5f, 0);
    //        Vector3 size = new Vector3((maxX - minX) + 1, (maxY - minY) + 1, 1);

    //        Gizmos.color = GetRoomColor(room.Key.x); // Generate deterministic color
    //        Gizmos.DrawCube(center, size);
    //    }
    //}

}