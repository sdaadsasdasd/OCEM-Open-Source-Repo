using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraHandler : MonoBehaviour
{
    [SerializeField]private GameObject tree_1;
    [SerializeField]private GameObject grass_Tall;
    [SerializeField]private GameObject grass_Short;
    [SerializeField]private GameObject rock;
    [SerializeField]private GameObject flower_1;
    [SerializeField]private GameObject flower_2;
    [SerializeField]private GameObject flower_3;
    [SerializeField]private GameObject parent;

    NoiseHandler noiseHandler = new NoiseHandler();

    public List<FloraData> addFlora(Vector2Int chunkPosition, Noise noise, Chunk chunk){


        List<FloraData> totalFloraData = new List<FloraData>();
        totalFloraData.AddRange(TreePass(chunkPosition, noise, chunk));
        totalFloraData.AddRange(GrassPass(chunkPosition, noise, chunk));
        totalFloraData.AddRange(RockPass(chunkPosition, noise, chunk));
        totalFloraData.AddRange(FlowerPass(chunkPosition, noise, chunk));

        return totalFloraData;
    }

    private List<FloraData> TreePass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

        int numTrees = (int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.5f, 0.6f, 0.6f));
                                    
        List<FloraData> treeData = new List<FloraData>();
   
        if(numTrees > 0){
            Vector2Int treePos = new Vector2Int(Random.Range(1,8), Random.Range(1,8));
            CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[treePos.x, treePos.y]];
   
            if(tempCoordData.blocks[tempCoordData.heighestBlock] == 4){
   
                Vector3 position = new Vector3(treePos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, treePos.y - 5.5f + chunkPosition.y);
                treeData.Add(new FloraData(position, FloraModel.Tree_1));
                tempCoordData.treeState = TreeState.Occupied;

                WorldData.coordinateData[treePos] = tempCoordData;

                for(int x = treePos.x - 3; x <= 3 + treePos.x; x++){
                   for(int z = treePos.y - 3; z <= 3 + treePos.y; z++){

                        Vector2Int tempPos = new Vector2Int(x,z) + chunkPosition;
                        if(WorldData.coordinateData.ContainsKey(tempPos)){
                            tempCoordData = WorldData.coordinateData[tempPos];
                            tempCoordData.treeState = TreeState.TreeFall;
                        }
                    }
                }
            }
        }

        return treeData;
    }

    private List<FloraData> GrassPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

        int numGrass = (int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 3f, 6f));
                                    
        List<FloraData> grassData = new List<FloraData>();
   
        for(int i = 0; i <= numGrass; i++){

            Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
            CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   
            if(tempCoordData.treeState != TreeState.Occupied){
   
                Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, grassPos.y - 5.5f + chunkPosition.y);
                grassData.Add(new FloraData(position, FloraModel.Grass_Tall));
                tempCoordData.treeState = TreeState.Occupied;

                WorldData.coordinateData[grassPos] = tempCoordData;
            } 
        }

        numGrass = (int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 5f, 15f));
   
        for(int i = 0; i <= numGrass; i++){

            Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
            CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   
            if(tempCoordData.treeState != TreeState.Occupied){
   
                Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, grassPos.y - 5.5f + chunkPosition.y);
                grassData.Add(new FloraData(position, FloraModel.Grass_Short));
                tempCoordData.treeState = TreeState.Occupied;

                WorldData.coordinateData[grassPos] = tempCoordData;
            } 
        }

        return grassData;
    }

    private List<FloraData> RockPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

        int numGrass = (int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 2f, 4f));
                                    
        List<FloraData> rockData = new List<FloraData>();
   
        for(int i = 0; i <= numGrass; i++){

            Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
            CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   
            Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.45f, grassPos.y - 5.5f + chunkPosition.y);
            rockData.Add(new FloraData(position, FloraModel.Rock));
             
        }

        return rockData;
    }

    private List<FloraData> FlowerPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

        int numGrass = (int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 1f, 2f));
                                    
        List<FloraData> flowerData = new List<FloraData>();
   
        for(int i = 0; i <= numGrass; i++){

            Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
            CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   
            Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.45f, grassPos.y - 5.5f + chunkPosition.y);
            flowerData.Add(new FloraData(position, FloraModel.Flower));
             
        }

        return flowerData;
    }

    public void instantiateFlora(FloraData floraData, float chunkDistance){

        if(floraData.floraObject == null){
            switch(floraData.floraModel){
                case FloraModel.Tree_1:{

                    floraData.floraObject = Instantiate(tree_1, floraData.position, Quaternion.Euler(new Vector3(0,90 * Random.Range(0,3),0)), parent.transform);
                    break;
                }
                case FloraModel.Grass_Tall:{

                    if(chunkDistance < UVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(grass_Tall, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
                case FloraModel.Grass_Short:{

                    if(chunkDistance < UVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(grass_Short, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
                case FloraModel.Rock:{

                    if(chunkDistance < UVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(rock, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
                case FloraModel.Flower:{

                    GameObject flower = flower_1;
                    Vector3 extra = new Vector3();

                    switch(Random.Range(1,4)){
                        case 1:{
                            flower = flower_1;
                            extra = new Vector3(0,-0.05f,0);
                            break;
                        }
                        case 2:{
                            flower = flower_2;
                            extra = new Vector3(0,0.26f,0);
                            break;
                        }
                        default:{
                            flower = flower_3;
                            extra = new Vector3(0,-0.05f,0);
                            break;
                        }
                    }

                    if(chunkDistance < UVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(flower, floraData.position + extra, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
            }
        } else {

            switch(floraData.floraModel){
                case FloraModel.Grass_Tall:{

                    if(chunkDistance > UVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    
                    break;
                }
                case FloraModel.Grass_Short:{

                    if(chunkDistance > UVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    break;
                }
                case FloraModel.Rock:{

                    if(chunkDistance > UVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    break;
                }
                case FloraModel.Flower:{

                    if(chunkDistance > UVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    break;
                }
                default:{
                    break;
                }
            }
        }
        
    }
}

public class FloraData
{
    public Vector3 position;
    public GameObject floraObject;
    public FloraModel floraModel;

    public FloraData(Vector3 _position, FloraModel _foilageModel){

        position = _position;
        floraModel = _foilageModel;
    }
}

public enum FloraModel{

    Tree_1,
    Grass_Tall,
    Grass_Short,
    Rock,
    Flower
}

public enum TreeState{

    NoTree,
    TreeFall,
    Occupied
}
