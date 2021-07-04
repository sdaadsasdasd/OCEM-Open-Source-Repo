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
    Noise floraNoise;

    private int treeFallDist = 6;

    NoiseSetting grassShortNoise;
    NoiseSetting grassTallNoise;
    NoiseSetting treeNoise;
    NoiseSetting flowerNoise;

    public void initializeFloraHandler(){

        floraNoise = new Noise(2);
        treeNoise = new NoiseSetting(0.06f, -1, 1, new Noise(1));
        grassShortNoise = new NoiseSetting(0.15f, -1f, 1f, floraNoise);
        grassTallNoise = new NoiseSetting(0.15f, -1f, 1f, floraNoise);
        flowerNoise = new NoiseSetting(0.15f, -1f, 1f, new Noise(3));
    }

    public List<FloraData> addFlora(Vector2Int chunkPosition, Chunk chunk){

        List<FloraData> floraData = new List<FloraData>();

        floraData.AddRange(TreeGeneration(chunk, FloraModel.Tree_1, treeNoise, 0.2f));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraModel.Grass_Tall, grassTallNoise, 0f, 0.7f));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraModel.Grass_Short, grassShortNoise, 0f, 0.1f));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraModel.Flower, flowerNoise, 0f, 0.8f));
        

        return floraData;
    }

    private List<FloraData> FloraGeneration(Vector2Int chunkPosition, Chunk chunk, FloraModel model, NoiseSetting nS, float randRange, float breakPoint){

        List<FloraData> floraData = new List<FloraData>();

        for(int z = 0; z < GlobalVariables.chunkSize * 2; z++){
            for(int x = 0; x < GlobalVariables.chunkSize * 2; x++){

                Vector2Int realCoord = chunk.realCoords[x,z];
                CoordinateData coordinate = WorldData.coordinateData[realCoord];
                bool viableFlora = false;
                Occupancy floraOccupancy;

                switch(model){
                    case FloraModel.Flower:{

                        viableFlora = (coordinate.occupancy == Occupancy.Empty || coordinate.occupancy == Occupancy.ShortGrass) && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.Occupied;
                        break;
                    }
                    case FloraModel.Grass_Short:{

                        viableFlora = coordinate.occupancy != Occupancy.Occupied && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.ShortGrass;
                        break;
                    }
                    case FloraModel.Grass_Tall:{

                        viableFlora = coordinate.occupancy != Occupancy.Occupied && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.Occupied;
                        break;
                    }
                    default:{

                        viableFlora = false;
                        floraOccupancy = Occupancy.Empty;
                        break;
                    }
                }
                
                if(viableFlora){

                    Vector3 realPosition = new Vector3( realCoord.x + 0.5f + Random.Range(-randRange, randRange), 
                                                        coordinate.heighestBlock + 0.5f, 
                                                        realCoord.y + 0.5f + Random.Range(-randRange, randRange));
                
                    float noiseNumber = noiseHandler.Evaluate(realPosition, nS.scale, nS.min, nS.max, nS.noise);

                    if(noiseNumber > breakPoint)
                    {
                        floraData.Add(new FloraData(realPosition, model));
                        coordinate.occupancy = floraOccupancy;
                        WorldData.coordinateData[realCoord] = coordinate;
                    }
                }
            }
        }
    
        return floraData;
    }

    private List<FloraData> TreeGeneration(Chunk chunk, FloraModel model, NoiseSetting nS, float breakPoint){

        //Go through the chunk
        //Generate noise values for each block, if not in a tree fall area
        //Record the highest number
        //Spawn a tree on that block
        //Generate Tree Fall around that block

        bool viableBlock = false;
        float highestValue = 0;
        Vector2Int highestCoord = new Vector2Int();
        Vector3 realPosition = new Vector3();
        CoordinateData coordinate;

        List<FloraData> floraData = new List<FloraData>();

        for(int z = 0; z < GlobalVariables.chunkSize * 2; z++){
            for(int x = 0; x < GlobalVariables.chunkSize * 2; x++){

                Vector2Int realCoord = chunk.realCoords[x,z];
                coordinate = WorldData.coordinateData[realCoord];

                if(coordinate.occupancy == Occupancy.Empty && coordinate.biome == Biome.Forest){

                    realPosition = new Vector3(realCoord.x + 0.5f, coordinate.heighestBlock + 0.5f, realCoord.y + 0.5f);
                
                    float noiseNumber = noiseHandler.Evaluate(realPosition, nS.scale, nS.min, nS.max, nS.noise);

                    if(noiseNumber > highestValue && noiseNumber > breakPoint){

                        highestValue = noiseNumber;
                        highestCoord = realCoord;
                        viableBlock = true;
                    }
                }
            }
        }

        if(viableBlock){

            coordinate = WorldData.coordinateData[highestCoord];

            realPosition = new Vector3(highestCoord.x + 0.5f, coordinate.heighestBlock + 0.5f, highestCoord.y + 0.5f);;
            floraData.Add(new FloraData(realPosition, model));
            coordinate.occupancy = Occupancy.Occupied;

            for(int z = highestCoord.y - treeFallDist; z <= highestCoord.y + treeFallDist; z++){
                for(int x = highestCoord.x - treeFallDist; x <= highestCoord.x + treeFallDist; x++){

                    Vector2Int tempRealCoord = new Vector2Int(x, z);
                    CoordinateData tempCoordinate;

                    if(tempRealCoord != highestCoord){
                        if(WorldData.coordinateData.ContainsKey(tempRealCoord)){

                            tempCoordinate = WorldData.coordinateData[tempRealCoord];
                            tempCoordinate.occupancy = Occupancy.TreeFall;

                        } else {

                            tempCoordinate = new CoordinateData();
                            tempCoordinate.blocks = new int[GlobalVariables.chunkHeight];
                            tempCoordinate.occupancy = Occupancy.TreeFall;

                            WorldData.coordinateData.Add(tempRealCoord, tempCoordinate);
                        }
                    }
                }
            }

            WorldData.coordinateData[highestCoord] = coordinate;
        }

        return floraData;
    }

    //int numTrees = 1;//(int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.5f, 0.6f, 0.6f));
   
        //if(numTrees > 0){
        //    Vector2Int treePos = new Vector2Int(Random.Range(1,8), Random.Range(1,8));
        //    CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[treePos.x, treePos.y]];
   
        //    if(tempCoordData.blocks[tempCoordData.heighestBlock] == 4){
   
        //        Vector3 position = new Vector3(treePos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, treePos.y - 5.5f + chunkPosition.y);
        //        treeData.Add(new FloraData(position, FloraModel.Tree_1));
        //        tempCoordData.treeState = TreeState.Occupied;

        //        WorldData.coordinateData[treePos] = tempCoordData;

        //        for(int x = treePos.x - 3; x <= 3 + treePos.x; x++){
        //           for(int z = treePos.y - 3; z <= 3 + treePos.y; z++){

        //                Vector2Int tempPos = new Vector2Int(x,z) + chunkPosition;
        //                if(WorldData.coordinateData.ContainsKey(tempPos)){
        //                    tempCoordData = WorldData.coordinateData[tempPos];
        //                    tempCoordData.treeState = TreeState.TreeFall;
        //                }
        //            }
        //        }
        //    }
        //}

   //private List<FloraData> GrassPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

   //    int numGrass = 1;//(int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 3f, 6f));
   //                                
   //    List<FloraData> grassData = new List<FloraData>();
   //
   //    for(int i = 0; i <= numGrass; i++){

   //        Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
   //        CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   //
   //        if(tempCoordData.treeState != TreeState.Occupied){
   //
   //            Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, grassPos.y - 5.5f + chunkPosition.y);
   //            grassData.Add(new FloraData(position, FloraModel.Grass_Tall));
   //            tempCoordData.treeState = TreeState.Occupied;

   //            WorldData.coordinateData[grassPos] = tempCoordData;
   //        } 
   //    }

   //    numGrass = 1;//(int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 5f, 15f));
   //
   //    for(int i = 0; i <= numGrass; i++){

   //        Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
   //        CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   //
   //        if(tempCoordData.treeState != TreeState.Occupied){
   //
   //            Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.5f, grassPos.y - 5.5f + chunkPosition.y);
   //            grassData.Add(new FloraData(position, FloraModel.Grass_Short));
   //            tempCoordData.treeState = TreeState.Occupied;

   //            WorldData.coordinateData[grassPos] = tempCoordData;
   //        } 
   //    }

   //    return grassData;
   //}

   //private List<FloraData> RockPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

   //    int numGrass = 1;//(int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 2f, 4f));
   //                                
   //    List<FloraData> rockData = new List<FloraData>();
   //
   //    for(int i = 0; i <= numGrass; i++){

   //        Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
   //        CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   //
   //        Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.45f, grassPos.y - 5.5f + chunkPosition.y);
   //        rockData.Add(new FloraData(position, FloraModel.Rock));
   //         
   //    }

   //    return rockData;
   //}

   //private List<FloraData> FlowerPass(Vector2Int chunkPosition, Noise noise, Chunk chunk){

   //    int numGrass = 1;//(int)Mathf.Round(noiseHandler.Evaluate(new Vector3(chunkPosition.x, 0, chunkPosition.y), 0.1f, 1f, 2f));
   //                                
   //    List<FloraData> flowerData = new List<FloraData>();
   //
   //    for(int i = 0; i <= numGrass; i++){

   //        Vector2Int grassPos = new Vector2Int(Random.Range(0,9), Random.Range(0,9));
   //        CoordinateData tempCoordData = WorldData.coordinateData[chunk.realCoords[grassPos.x, grassPos.y]];
   //
   //        Vector3 position = new Vector3(grassPos.x - 5.5f + chunkPosition.x, tempCoordData.heighestBlock - 0.45f, grassPos.y - 5.5f + chunkPosition.y);
   //        flowerData.Add(new FloraData(position, FloraModel.Flower));
   //         
   //    }

   //    return flowerData;
   //}

    public void instantiateFlora(FloraData floraData, float chunkDistance){

        if(floraData.floraObject == null){
            switch(floraData.floraModel){
                case FloraModel.Tree_1:{

                    floraData.floraObject = Instantiate(tree_1, floraData.position, Quaternion.Euler(new Vector3(0,90 * Random.Range(0,3),0)), parent.transform);
                    break;
                }
                case FloraModel.Grass_Tall:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(grass_Tall, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
                    break;
                }
                case FloraModel.Grass_Short:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(grass_Short, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
                    break;
                }
                case FloraModel.Rock:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(rock, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
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

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(flower, floraData.position + extra, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
            }
        } else {

            switch(floraData.floraModel){
                case FloraModel.Grass_Tall:{

                    if(chunkDistance > GlobalVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    
                    break;
                }
                case FloraModel.Grass_Short:{

                    if(chunkDistance > GlobalVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    break;
                }
                case FloraModel.Rock:{

                    if(chunkDistance > GlobalVariables.smallFloraViewDist){
                        Destroy(floraData.floraObject);
                    }
                    break;
                }
                case FloraModel.Flower:{

                    if(chunkDistance > GlobalVariables.smallFloraViewDist){
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

public enum Occupancy{

    Empty,
    TreeFall,
    ShortGrass,
    Occupied
}
