using UnityEngine;
using System.Collections.Generic;

public class ChunkHandler : MonoBehaviour
{
    [SerializeField]private Events events;
    [SerializeField]private FloraHandler floraHandler;
    [SerializeField]private BiomeHandler biomeHandler;
    [SerializeField]private Material ground;

    int loadDist = GlobalVariables.chunkViewDist;

    [SerializeField]private GameObject map;

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
        BlocksTypes.initializeBlocks();
        biomeHandler.initializeBiomes();
        floraHandler.initializeFloraHandler();

        events.movedChunk += LoadChunks;
        events.movedChunk += UnloadChunks;
        events.movedChunk += LoadUnloadEntities;
    }

    private void LoadChunks(Vector2Int currentChunk){

        for (int z = -loadDist + currentChunk.y; z < loadDist + currentChunk.y; z++){
            for (int x = -loadDist + currentChunk.x; x < loadDist + currentChunk.x; x++){ //Render range

                Vector2Int chunkPos = new Vector2Int(x,z);
                float chunkDistance = new Vector2(x - currentChunk.x, z - currentChunk.y).magnitude;

                if(chunkDistance < GlobalVariables.chunkViewDist){

                    Chunk chunk = null;

                    if(WorldData.loadedChunks.TryGetValue(chunkPos, out chunk)){ //Check if chunk was generated before
                        if(chunk.chunkObject == null){

                            RenderChunk(ref chunk, chunkPos * 10, chunk.chunkMesh, x, z);
                            WorldData.renderedChunks.Add(chunk); 
                        }
                    } else {                                                    

                        chunk = new Chunk(); //Generate and add to dictionary

                        AddBlockCoordsToDictionary(chunkPos * 10, ref chunk);
                        biomeHandler.generateChunkInfo(chunkPos * 10, GlobalVariables.chunkSize, false);

                        chunk.chunkMesh = customMesh(biomeHandler.generateChunkInfo(chunkPos * 10, GlobalVariables.chunkSize + 1, true), 1 + (GlobalVariables.chunkSize * 2), GlobalVariables.chunkHeight, 1f);
                        
                        chunk.flora.AddRange(floraHandler.addFlora(chunkPos * 10, chunk));

                        RenderChunk(ref chunk, chunkPos * 10, chunk.chunkMesh, x, z);

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

                float chunkDistance = (WorldData.renderedChunks[i].chunkPos - currentChunk).magnitude;

                if (chunkDistance > GlobalVariables.chunkViewDist) 
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

        for (int z = -GlobalVariables.chunkSize; z < GlobalVariables.chunkSize; z++){
            for (int x = -GlobalVariables.chunkSize; x < GlobalVariables.chunkSize; x++)
            {
                Vector2Int realCoord = new Vector2Int(x + (chunkPosition.x), z + (chunkPosition.y));
                chunk.realCoords[x + 5,z + 5] = realCoord;

                if(!WorldData.coordinateData.ContainsKey(realCoord)){

                    CoordinateData tempCoordinate = new CoordinateData();
                    tempCoordinate.blocks = new int[GlobalVariables.chunkHeight];
                    tempCoordinate.occupancy = Occupancy.Empty;

                    WorldData.coordinateData.Add(realCoord, tempCoordinate);
                }
            }
        }
    }

    private void RenderChunk(ref Chunk chunk, Vector2Int chunkPos, Mesh mesh, int x, int y){

        Vector3 truePos = new Vector3(chunkPos.x, 0, chunkPos.y);

        chunk.chunkObject = new GameObject("Cx"+x+"y"+y);
        chunk.chunkObject.transform.parent = map.transform;
        chunk.chunkObject.transform.position = truePos;
        chunk.chunkObject.AddComponent<MeshFilter>().mesh = mesh;
        chunk.chunkObject.AddComponent<MeshRenderer>().material = ground;
        chunk.chunkObject.AddComponent<MeshCollider>();
        chunk.chunkObject.layer = 6;
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
    public Vector2Int[,] realCoords = new Vector2Int[GlobalVariables.chunkSize * 2, GlobalVariables.chunkSize * 2];
    public List<FloraData> flora = new List<FloraData>();
}