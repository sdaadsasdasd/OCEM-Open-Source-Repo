using System.Collections.Generic;
using UnityEngine;

//This script is here to save all the world data.

public static class WorldData
{
    public static Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    public static Dictionary<Vector2Int, CoordinateData> coordinateData = new Dictionary<Vector2Int, CoordinateData>();
    public static List<Chunk> renderedChunks = new List<Chunk>();
    
}

public struct CoordinateData
{
    public Biome biome;
    public int[] blocks;
    public Occupancy occupancy;
    public int heighestBlock;    
}


