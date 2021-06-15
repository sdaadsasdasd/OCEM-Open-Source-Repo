using UnityEngine;
using System.Collections.Generic;

public class ChunkHandler : MonoBehaviour
{
    Events events;
    public Material ground;
    int loadDist = UVariables.chunkViewDist;
    Noise noise = new Noise();
    GameObject map;

    private void OnEnable()
    {
        map = new GameObject("map");
        events = FindObjectOfType<Events>();
        events.movedChunk += LoadChunks;
        events.movedChunk += UnloadChunks;
    }


    private void LoadChunks(Vector2Int currentChunk){

        for (int y = -loadDist + currentChunk.y; y < loadDist + currentChunk.y; y++){
            for (int x = -loadDist + currentChunk.x; x < loadDist + currentChunk.x; x++){ //Render range

                Chunk chunk = null;
                Vector2Int chunkPos = new Vector2Int(x,y);

                if(WorldData.loadedChunks.TryGetValue(chunkPos, out chunk)){ //Check if chunk was generated before
                    if(chunk.chunkObject == null){

                        chunk.chunkObject = new GameObject("Cx"+x+"y"+y);
                        chunk.chunkObject.transform.parent = map.transform;
                        chunk.chunkObject.transform.position = new Vector3(chunkPos.x, 0, chunkPos.y) * 10;
                        chunk.chunkObject.AddComponent<MeshFilter>().mesh = chunk.chunkMesh;
                        chunk.chunkObject.AddComponent<MeshRenderer>().material = ground;
                        chunk.chunkObject.AddComponent<MeshCollider>();
                        chunk.chunkObject.layer = 6;
                        
                        WorldData.renderedChunks.Add(chunk); 
                    }
                } else {

                    chunk = new Chunk(); //Generate and add to dictionary

                    chunk.chunkObject = new GameObject("Cx"+x+"y"+y);
                    Mesh chunkMesh = CustomMesh(GenerateChunkInfo(chunkPos * 10, UVariables.chunkSize + 1), 1 + (UVariables.chunkSize * 2), UVariables.chunkHeight, 1f);
                    chunk.chunkObject.transform.parent = map.transform;
                    chunk.chunkObject.transform.position = new Vector3(chunkPos.x, 0, chunkPos.y) * 10;
                    chunk.chunkObject.AddComponent<MeshFilter>().mesh = chunkMesh;
                    chunk.chunkObject.AddComponent<MeshRenderer>().material = ground;
                    chunk.chunkObject.AddComponent<MeshCollider>();
                    chunk.chunkObject.layer = 6;
                    chunk.chunkData = GenerateChunkInfo(chunkPos * 10, UVariables.chunkSize);
                    chunk.chunkMesh = chunkMesh;
                    chunk.chunkPos = chunkPos;

                    WorldData.loadedChunks.Add(chunkPos, chunk);
                    WorldData.renderedChunks.Add(chunk);
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
                    Destroy(WorldData.renderedChunks[i].chunkObject);
                    
                }

                if(WorldData.renderedChunks[i].chunkObject == null){ //kept seperate for redundancy 

                    WorldData.renderedChunks.Remove(WorldData.renderedChunks[i]);
                }
            }
        }        
    }

    private int[,,] GenerateChunkInfo(Vector2Int chunkPos, int chunkSize){

        int[,,] chunkData = new int[chunkSize * 2, UVariables.chunkHeight, chunkSize * 2];

        for (int z = -chunkSize; z < chunkSize; z++)
        {
            for (int x = -chunkSize; x < chunkSize; x++)
            {
                Vector3 tilePos = new Vector3(x, 0, z);

                int noiseValue = (int)Mathf.Round(noise.Evaluate((tilePos + new Vector3(chunkPos.x, 0, chunkPos.y)) * 0.02f) * 2) + 8;
                chunkData[x + chunkSize,noiseValue,z + chunkSize] = 1;
                tilePos = new Vector3(tilePos.x, noiseValue, tilePos.z);

                for(int i = 0; i < noiseValue; i++){

                    chunkData[x + chunkSize,i,z + chunkSize] = 1;
                }
            }
        }

        return chunkData;
    }

        public static Mesh CustomMesh(int[,,] MeshData, int meshWidth, int meshHeight, float scale){

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for(int x = 1; x < meshWidth;x++){
            for (int z = 1; z < meshWidth; z++){
                for (int y = 0; y < meshHeight; y++){          
                    if(MeshData[x, y, z] == 1){

                        Vector3 blockPos = new Vector3((x - (meshWidth - 2) / 2f) - 1 , y, (z - (meshWidth - 2) / 2f) - 1)  * scale ;
                        int numFaces = 0;
                        //UP
                        if (y < meshHeight - 1 && MeshData[x, y + 1, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1,1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //DOWN
                        if (y > 1 && MeshData[x, y - 1, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //LEFT
                        if (x < meshWidth && MeshData[x + 1, y, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3(1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(1, 1,-1) * (scale / 2f));
                            numFaces++;
                        }
                        //RIGHT
                        if (x > 0 && MeshData[x - 1, y, z] == 0){
                            
                            vertices.Add(blockPos + new Vector3(-1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //FRONT
                        if (z < meshWidth && MeshData[x, y, z + 1] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1, 1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1, 1) * (scale / 2f));
                            numFaces++;
                        }
                        //BACK
                        if (z > 0 && MeshData[x, y, z - 1] == 0){
                            
                            vertices.Add(blockPos + new Vector3( 1, 1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3( 1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1,-1,-1) * (scale / 2f));
                            vertices.Add(blockPos + new Vector3(-1, 1,-1) * (scale / 2f));
                            numFaces++;
                        }

                        int tl = vertices.Count - 4 * numFaces;
                        for(int i = 0; i < numFaces; i++)
                        {
                            triangles.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
                            //uvs.AddRange(Block.blocks[BlockType.Grass].topPos.GetUVs());

                        }
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
}

public class Chunk{

    public GameObject chunkObject;
    public Mesh chunkMesh;
    public Vector2Int chunkPos;
    public int[,,] chunkData = new int[UVariables.chunkSize, UVariables.chunkHeight, UVariables.chunkSize];

}

public class BlockType{

    public Material material;
}