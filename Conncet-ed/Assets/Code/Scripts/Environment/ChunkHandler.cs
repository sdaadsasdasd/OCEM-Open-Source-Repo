using UnityEngine;
using System.Collections.Generic;

//All things terrain and chunk related.
public class ChunkHandler : MonoBehaviour
{
    [SerializeField]private Events events;
    [SerializeField]private FloraHandler floraHandler;
    [SerializeField]private Material ground;

    int loadDist = UVariables.chunkViewDist;

    [SerializeField]private int seed;
    
    Noise mapNoise;
    Noise biomeNoise;
    NoiseHandler noiseHandler = new NoiseHandler();
    GameObject map;
    BiomeType seaBiome;

    enum FoilageObject{
        Tree_1,
        Tree_2,
        Flower_1,
        Flower_2,
        Flower_3,
        Mushroom_1,
        Mushroom_2,
        MushroomPatch_1
    }

    private void OnEnable()
    {
        map = new GameObject("map");

        mapNoise = new Noise(seed);
        biomeNoise = new Noise(seed + 1);
        

        BlocksTypes.initializeBlocks();
        Biomes.initializeBiomes();
        seaBiome = Biomes.biomeTypes[2];

        events.movedChunk += LoadChunks;
        events.movedChunk += UnloadChunks;
        events.movedChunk += LoadUnloadEntities;
    }

    private void LoadChunks(Vector2Int currentChunk){

        for (int z = -loadDist + currentChunk.y; z < loadDist + currentChunk.y; z++){
            for (int x = -loadDist + currentChunk.x; x < loadDist + currentChunk.x; x++){ //Render range

                Vector2Int chunkPos = new Vector2Int(x,z);
                float chunkDistance = new Vector2(x - currentChunk.x, z - currentChunk.y).magnitude;

                if(chunkDistance < UVariables.chunkViewDist){

                    Chunk chunk = null;

                    if(WorldData.loadedChunks.TryGetValue(chunkPos, out chunk)){ //Check if chunk was generated before
                        if(chunk.chunkObject == null){

                            InitializeChunk(ref chunk, chunkPos * 10, chunk.chunkMesh, x, z);
                            WorldData.renderedChunks.Add(chunk); 
                        }
                    } else {                                                    

                        chunk = new Chunk(); //Generate and add to dictionary

                        AddBlockCoordsToDictionary(chunkPos * 10, ref chunk);
                        GenerateForestBiome(chunkPos);
                        GenerateChunkInfo(chunkPos * 10, UVariables.chunkSize, false);

                        chunk.chunkMesh = customMesh(GenerateChunkInfo(chunkPos * 10, UVariables.chunkSize + 1, true), 1 + (UVariables.chunkSize * 2), UVariables.chunkHeight, 1f);

                        chunk.flora.AddRange(floraHandler.addFlora(chunkPos * 10, biomeNoise, chunk));

                        InitializeChunk(ref chunk, chunkPos * 10, chunk.chunkMesh, x, z);

                        chunk.chunkPos = chunkPos;

                        WorldData.loadedChunks.Add(chunkPos, chunk);
                        WorldData.renderedChunks.Add(chunk);
                    }
                }
                
            }
        }
    }

    private void UnloadChunks(Vector2Int currentChunk){

        if(WorldData.renderedChunks.Count > 0){
            for (int i = 0; i < WorldData.renderedChunks.Count; i++){

                Vector2Int cDist = WorldData.renderedChunks[i].chunkPos - currentChunk; //Check if in range

                if (cDist.x > UVariables.chunkViewDist || cDist.y > UVariables.chunkViewDist || cDist.x < -UVariables.chunkViewDist || cDist.y < -UVariables.chunkViewDist) 
                {   
                    foreach(FloraData foilageData in WorldData.renderedChunks[i].flora){
                        
                        Destroy(foilageData.floraObject);
                    } 
                    Destroy(WorldData.renderedChunks[i].chunkObject);
                }

                if(WorldData.renderedChunks[i].chunkObject == null){ //kept seperate for redundancy 

                    WorldData.renderedChunks.Remove(WorldData.renderedChunks[i]);
                }
            }
        }        
    }

    private void LoadUnloadEntities(Vector2Int currentChunk){

        foreach(Chunk chunk in WorldData.renderedChunks){
            
            float chunkDistance = new Vector2(chunk.chunkPos.x - currentChunk.x, chunk.chunkPos.y - currentChunk.y).magnitude;

            foreach(FloraData flora in chunk.flora){

                floraHandler.instantiateFlora(flora, chunkDistance);
            }
        }
    }

    private void AddBlockCoordsToDictionary(Vector2Int chunkPosition, ref Chunk chunk){

        for (int z = 0; z < UVariables.chunkSize * 2; z++){
            for (int x = 0; x < UVariables.chunkSize * 2; x++)
            {
                Vector2Int realCoord = new Vector2Int(x + (chunkPosition.x), z + (chunkPosition.y));
                chunk.realCoords[x,z] = realCoord;
                CoordinateData tempCoordData = new CoordinateData();
                tempCoordData.blocks = new int[UVariables.chunkHeight];
                tempCoordData.treeState = TreeState.NoTree;
                if(!WorldData.coordinateData.ContainsKey(realCoord))
                WorldData.coordinateData.Add(realCoord, tempCoordData);
            }
        }
    }

    private void GenerateForestBiome(Vector2Int chunkPosition){


        for (int z = 0; z < UVariables.chunkSize * 2; z++){
            for (int x = 0; x < UVariables.chunkSize * 2; x++)
            {
                Vector3 coordPosition = new Vector3(x + (chunkPosition.x * 10), 0, z + (chunkPosition.y * 10));
                float biome = noiseHandler.Evaluate(coordPosition, 0.007f, 1, 0);
                int biomeId = biome > 0 ? 1 : 4;

                CoordinateData tempCoordData = WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)];
                tempCoordData.biome = biomeId;
                WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)] = tempCoordData;
            }
        }
    }

    private int[,,] GenerateChunkInfo(Vector2Int chunkPosition, int chunkSize, bool meshPass){

        int[,,] chunkData = new int[chunkSize * 2, UVariables.chunkHeight, chunkSize * 2];

        for(int z = -chunkSize; z < chunkSize; z++){
            for(int x = -chunkSize; x < chunkSize; x++){

                Vector2Int realCoord = new Vector2Int(x + (chunkPosition.x) + chunkSize, z + (chunkPosition.y) + chunkSize);

                CoordinateData tempCoordData;
                BiomeType biome;
                int noiseValue;

                if(meshPass){
                    
                    Vector3 coordPosition = new Vector3(realCoord.x, 0, realCoord.y);

                    float biomeId = noiseHandler.Evaluate(coordPosition, 0.007f, 1, 0);
                    biome = biomeId > 0 ? Biomes.biomeTypes[1] : Biomes.biomeTypes[4];
                    noiseValue = (int)noiseHandler.Evaluate(new Vector3(realCoord.x,0,realCoord.y), biome.scale, biome.amplitude, biome.height);

                    for(int i = 0; i < noiseValue; i++){
                        chunkData[x + chunkSize,i, z + chunkSize] = biome.blockType.id;
                    }
                    
                } else {

                    tempCoordData = WorldData.coordinateData[realCoord];
                    biome = Biomes.biomeTypes[tempCoordData.biome];
                    noiseValue = (int)noiseHandler.Evaluate(new Vector3(realCoord.x,0,realCoord.y), biome.scale, biome.amplitude, biome.height);
                    tempCoordData.heighestBlock = noiseValue;
                    tempCoordData.blocks[noiseValue] = biome.blockType.id;

                    for(int i = 0; i < noiseValue; i++){
                        tempCoordData.blocks[i] = biome.blockType.id;
                        WorldData.coordinateData[realCoord] = tempCoordData;
                    }
                }
            }
        }

        return chunkData;
    }

    private void InitializeChunk(ref Chunk chunk, Vector2Int chunkPos, Mesh mesh, int x, int y){

        Vector3 truePos = new Vector3(chunkPos.x, 0, chunkPos.y);

        chunk.chunkObject = new GameObject("Cx"+x+"y"+y);
        chunk.chunkObject.transform.parent = map.transform;
        chunk.chunkObject.transform.position = truePos;
        chunk.chunkObject.AddComponent<MeshFilter>().mesh = mesh;
        chunk.chunkObject.AddComponent<MeshRenderer>().material = ground;
        chunk.chunkObject.AddComponent<MeshCollider>();
        chunk.chunkObject.layer = 6;
    }

    

    private int FindHeighestBlock(int[] blocks){

        int heighestBlock = 0;
        for(int i = 0; i < UVariables.chunkHeight; i++){
            if(blocks[i] > 0){
                heighestBlock = i;
            }
        }

        return heighestBlock;
    }

    public static Mesh customMesh(int[,,] chunkData, int meshWidth, int meshHeight, float scale){

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        for(int x = 1; x < meshWidth;x++){
            for (int z = 1; z < meshWidth; z++){
                for (int y = 0; y < meshHeight; y++){

                    if(chunkData[x, y, z] >= 1){

                        Vector3 blockPos = new Vector3((x - (meshWidth - 2) / 2f) - 1 , y, (z - (meshWidth - 2) / 2f) - 1)  * scale ;
                        int numFaces = 0;
                        //Up
                        if (y < meshHeight - 1 && chunkData[x, y + 1, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1,1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //Down
                        if (y > 1 && chunkData[x, y - 1, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //Left
                        if (x < meshWidth && chunkData[x + 1, y, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3(1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1, 1,-1) * (scale / 2f));
                            numFaces++;
                        }
                        //Right
                        if (x > 0 && chunkData[x - 1, y, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3(-1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //Front
                        if (z < meshWidth && chunkData[x, y, z + 1] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //Back
                        if (z > 0 && chunkData[x, y, z - 1] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1, 1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1,-1) * (scale / 2f));
                            numFaces++;
                        }

                        int tl = vertices.Count - 4 * numFaces;
                        for(int i = 0; i < numFaces; i++)
                        {
                            triangles.AddRange(new int[] {tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3});
                            uvs.AddRange(SetUVs(BlocksTypes.blockTypes[chunkData[x,y,z]]));
                        }
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    private static List<Vector2> SetUVs(BlockType blockType){

        Vector2 textureCoords = new Vector2(blockType.textureCoords.x / 16f, blockType.textureCoords.y / 16f);

        List<Vector2> uvs = new List<Vector2>();
        uvs.Add(new Vector2(0,0)                + textureCoords);
        uvs.Add(new Vector2(0.0625f,0)          + textureCoords);
        uvs.Add(new Vector2(0.0625f,0.0625f)    + textureCoords);
        uvs.Add(new Vector2(0,0.0625f)          + textureCoords);
        
        return uvs;
    }
}

public class Chunk{

    public GameObject chunkObject;
    public Mesh chunkMesh;
    public Vector2Int chunkPos;
    public Vector2Int[,] realCoords = new Vector2Int[UVariables.chunkSize * 2, UVariables.chunkSize * 2];
    public List<FloraData> flora = new List<FloraData>();
}