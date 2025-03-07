using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, walltilemap, laddermap, doortilemap, decorationtilemap, sawmap;
    [SerializeField]
    private TileBase floortile, floordecotile;
    [SerializeField]
    private TileBase walltop;
    [SerializeField]
    private TileBase laddertile, platformtile, sawtile;
    [SerializeField]
    private TileBase[] decorationtile;

    public void PaintFloortiles(IEnumerable<Vector2Int> floorpositions)
    {
        ClearTiles();
        PaintTiles(floorpositions, floorTilemap, floortile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> floorpositions, Tilemap floorTilemap, TileBase floortile)
    {
        foreach (var position in floorpositions)
        {
            PaintSingleTile(floorTilemap, floortile ,position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void ClearTiles()
    {
        floorTilemap.ClearAllTiles();
        walltilemap.ClearAllTiles();
        laddermap.ClearAllTiles();
        doortilemap.ClearAllTiles();
        sawmap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int wallpos)
    {
        PaintSingleTile(walltilemap, walltop, wallpos);
    }

    internal void PaintSingleBasicDoor(Vector2Int doorpos)
    {
        PaintSingleTile(doortilemap, platformtile, doorpos);
    }

    internal void PaintSingleLadder(Vector2Int ladderpos)
    {
        PaintSingleTile(laddermap, laddertile, ladderpos);
    }

    internal void PaintSingleDecoration(Vector2Int decorationpos, int id)
    {
        PaintSingleTile(decorationtilemap, decorationtile[id], decorationpos);
    }

    public void ApplyDecorations(TileMapVisualizer visualizer, Dictionary<Vector2Int, DecorationType> decorations)
    {
        foreach (var decoration in decorations)
        {
            int id = (int)decoration.Value; // Convert enum to index
            visualizer.PaintSingleDecoration(decoration.Key, id);
        }
    }

    internal void PaintSingleBasicSaw(Vector2Int saws)
    {
        PaintSingleTile(sawmap, sawtile, saws);
    }

    internal void PaintSingleFloorDeco(Vector2Int pos)
    {
        PaintSingleTile(floorTilemap, floordecotile, pos);
    }
}
