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
    [SerializeField]private GameObject shroom_1;
    [SerializeField]private GameObject shroom_2;
    [SerializeField]private GameObject shroom_3;
    [SerializeField]private GameObject parent;

    public static Dictionary<string, NoiseSetting> floraNoises = new Dictionary<string, NoiseSetting>();

    NoiseHandler noiseHandler = new NoiseHandler();

    private int treeFallDist = 6;

    public void initializeFloraHandler(){

        floraNoises.Add("Tree",         new NoiseSetting(0.06f, -1f, 1f, new Noise(1)));
        floraNoises.Add("Grass Short",  new NoiseSetting(0.15f, -1f, 1f, new Noise(2)));
        floraNoises.Add("Grass Tall",   new NoiseSetting(0.15f, -1f, 1f, new Noise(2)));
        floraNoises.Add("Flower",       new NoiseSetting(0.15f, -1f, 1f, new Noise(3)));
    }

    public List<FloraData> addFlora(Vector2Int chunkPosition, Chunk chunk){

        List<FloraData> floraData = new List<FloraData>();

        floraData.AddRange(TreeGeneration(chunk, FloraType.Tree, floraNoises["Tree"], 0.2f, true));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraType.Grass_Tall, floraNoises["Grass Tall"], 0f, 0.7f));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraType.Grass_Short, floraNoises["Grass Short"], 0f, 0.1f));
        floraData.AddRange(FloraGeneration(chunkPosition, chunk, FloraType.Flower, floraNoises["Flower"], 0f, 0.8f));
        

        return floraData;
    }

    private List<FloraData> FloraGeneration(Vector2Int chunkPosition, Chunk chunk, FloraType type, NoiseSetting nS, float randRange, float breakPoint){

        List<FloraData> floraData = new List<FloraData>();

        for(int z = 0; z < GlobalVariables.chunkSize * 2; z++){
            for(int x = 0; x < GlobalVariables.chunkSize * 2; x++){

                Vector2Int realCoord = chunk.realCoords[x,z];
                CoordinateData coordinate = WorldData.coordinateData[realCoord];
                bool viableFlora = false;
                Occupancy floraOccupancy;
                GameObject model;
                Vector3 extra = new Vector3();
                        
                switch(type){
                    case FloraModel.Flower:{
                        switch(Random.Range(1,4)){
                            case 1:{
                                model = flower_1;
                                extra = new Vector3(0,-0.05f,0);
                                break;
                            }
                            case 2:{
                                model = flower_2;
                                extra = new Vector3(0,-0.05f,0);
                                break;
                            }
                            default:{
                                model = flower_3;
                                extra = new Vector3(0,-0.05f,0);
                                break;
                            }
                        }
                        viableFlora = (coordinate.occupancy == Occupancy.Empty || coordinate.occupancy == Occupancy.ShortGrass) && coordinate.occupancy != Occupancy.TreeFall && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.Occupied;
                        break;
                    }
                    case FloraType.Grass_Short:{

                        viableFlora = coordinate.occupancy != Occupancy.Occupied && coordinate.occupancy != Occupancy.TallGrass && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.ShortGrass;
                        model = grass_Short;
                        break;
                    }
                    case FloraType.Grass_Tall:{

                        viableFlora = coordinate.occupancy != Occupancy.Occupied && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.TallGrass;
                        model = grass_Tall;
                        break;
                    }
                    case FloraType.Rock:{

                        viableFlora = coordinate.occupancy != Occupancy.Occupied && coordinate.biome != Biome.Seabed? true : false;
                        floraOccupancy = Occupancy.Empty;
                        model = rock;
=======
                        break;
                    }
                    default:{

                        viableFlora = false;
                        model = null;
                        floraOccupancy = Occupancy.Empty;
                        break;
                    }
                }
                
                if(viableFlora){

                    Vector3 realPosition = new Vector3( realCoord.x + 0.5f + Random.Range(-randRange, randRange), 
                                                        coordinate.heighestBlock + 0.5f, 
                                                        realCoord.y + 0.5f + Random.Range(-randRange, randRange)
                                                        );
                
                    float noiseNumber = noiseHandler.Evaluate(realPosition, nS);

                    if(noiseNumber > breakPoint)
                    {
                        floraData.Add(new FloraData(realPosition + extra, type, model));
                        coordinate.occupancy = floraOccupancy;
                        WorldData.coordinateData[realCoord] = coordinate;
                    }
                }
            }
        }
    
        return floraData;
    }

    private List<FloraData> TreeGeneration(Chunk chunk, FloraType type, NoiseSetting nS, float breakPoint, bool shrooms){

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
                
                    float noiseNumber = noiseHandler.Evaluate(realPosition, nS);

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
            floraData.Add(new FloraData(realPosition, type, tree_1));
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

            if(shrooms){

                int numShrooms = Random.Range(-2,5);
                GameObject model;

                switch(Random.Range(1,4)){
                    case 1:{
                        model = shroom_1;
                        break;
                    }
                    case 2:{
                        model = shroom_2;
                        break;
                    }
                    default:{
                        model = shroom_3;
                        break;
                    }
                }

                for(int i = 0; i < numShrooms; i++){

                    Vector2Int randCoord = new Vector2Int(Random.Range(-2,2), Random.Range(-2,2)) + highestCoord;
                    CoordinateData shroomCoordinate;

                    if(WorldData.coordinateData.ContainsKey(randCoord)){

                        shroomCoordinate = WorldData.coordinateData[randCoord];

                        if(randCoord != highestCoord && shroomCoordinate.occupancy != Occupancy.ShortGrass){

                            realPosition = new Vector3(randCoord.x + 0.5f, shroomCoordinate.heighestBlock + 0.5f, randCoord.y+ 0.5f);;
                            FloraData shroomData = new FloraData(realPosition, FloraType.Shroom, model);
                            shroomCoordinate.occupancy = Occupancy.ShortGrass;

                            WorldData.coordinateData[randCoord] = shroomCoordinate;
                            floraData.Add(shroomData);
                        }
                    } else {

                        shroomCoordinate = new CoordinateData();
                        shroomCoordinate.blocks = new int[GlobalVariables.chunkHeight];
                        shroomCoordinate.occupancy = Occupancy.TreeFall;

                        realPosition = new Vector3(randCoord.x + 0.5f, shroomCoordinate.heighestBlock + 0.5f, randCoord.y + 0.5f);

                        FloraData shroomData = new FloraData(realPosition, FloraType.Shroom, model);
                        shroomCoordinate.occupancy = Occupancy.ShortGrass;

                        floraData.Add(shroomData);

                        WorldData.coordinateData.Add(randCoord, shroomCoordinate);
                    }
                }
            }

            WorldData.coordinateData[highestCoord] = coordinate;
        }

        

        return floraData;
    }

    public void instantiateFlora(FloraData floraData, float chunkDistance){

        if(floraData.floraObject == null){
            switch(floraData.floraType){
                case FloraType.Tree:{

                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,90 * Random.Range(0,3),0)), parent.transform);
                    break;
                }
                case FloraType.Grass_Tall:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
                    break;
                }
                case FloraType.Grass_Short:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
                    break;
                }
                case FloraType.Rock:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,4) * 90,0)), parent.transform);
                    break;
                }
                case FloraType.Flower:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
                case FloraType.Shroom:{

                    if(chunkDistance < GlobalVariables.smallFloraViewDist)
                    floraData.floraObject = Instantiate(floraData.floraModel, floraData.position, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), parent.transform);
                    break;
                }
            }
        } else {

            switch(floraData.floraType){
                case FloraType.Grass_Tall:{

                    SmallFloraDestroy(floraData.floraObject, chunkDistance);
                    break;
                }
                case FloraType.Grass_Short:{

                    SmallFloraDestroy(floraData.floraObject, chunkDistance);
                    break;
                }
                case FloraType.Rock:{

                    SmallFloraDestroy(floraData.floraObject, chunkDistance);
                    break;
                }
                case FloraType.Flower:{

                    SmallFloraDestroy(floraData.floraObject, chunkDistance);
                    break;
                }
                default:{
                    break;
                }
            }
        }
        
    }

    private void SmallFloraDestroy(GameObject floraObject, float chunkDistance){

        if(chunkDistance > GlobalVariables.smallFloraViewDist){
            Destroy(floraObject);
        }
    }
}

public class FloraData
{
    public Vector3 position;
    public GameObject floraObject;
    public GameObject floraModel;
    public FloraType floraType;

    public FloraData(Vector3 position, FloraType floraType, GameObject floraModel){

        this.position = position;
        this.floraType = floraType;
        this.floraModel = floraModel;
    }
}

public enum FloraType{

    Tree,
    Grass_Tall,
    Grass_Short,
    Rock,
    Flower,
    Shroom
}

public enum Occupancy{

    Empty,
    TreeFall,
    ShortGrass,
    TallGrass,
    Occupied
}
