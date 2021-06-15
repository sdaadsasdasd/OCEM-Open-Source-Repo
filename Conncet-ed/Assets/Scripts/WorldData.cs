using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldData
{
    public static Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    public static List<Chunk> renderedChunks = new List<Chunk>();
    public static Dictionary<int, BlockType> blockTypes = new Dictionary<int, BlockType>();
}
