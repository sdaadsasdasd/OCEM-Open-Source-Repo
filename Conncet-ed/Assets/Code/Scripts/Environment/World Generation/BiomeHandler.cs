using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeHandler : MonoBehaviour
{
    public static Dictionary<Biome, BiomeData> biomeTypes = new Dictionary<Biome, BiomeData>();

    Noise mapNoise;
    NoiseHandler noiseHandler = new NoiseHandler();
    NoiseSetting biomeNoise;

    [SerializeField]private int seed;

    public void initializeBiomes(){

        List<NoiseSetting> noiseSettings = new List<NoiseSetting>();
        noiseSettings.Add(new NoiseSetting(0.002f, 0.8f, 1.5f, new Noise(seed)));
        noiseSettings.Add(new NoiseSetting(0.015f, -0.2f, 0.2f, new Noise(seed + 1)));

        BiomeData Grassy = new BiomeData(BlocksTypes.blockTypes[1], noiseSettings);
        biomeTypes.Add(Biome.Grassy, Grassy);

        noiseSettings = new List<NoiseSetting>();
        noiseSettings.Add(new NoiseSetting(0.002f, 0.8f, 1.5f, new Noise(seed)));
        noiseSettings.Add(new NoiseSetting(0.015f, -0.2f, 0.2f, new Noise(seed + 1)));

        BiomeData Sea = new BiomeData(BlocksTypes.blockTypes[3], noiseSettings);
        biomeTypes.Add(Biome.Seabed, Sea);

        noiseSettings = new List<NoiseSetting>();
        noiseSettings.Add(new NoiseSetting(0.002f, 0.8f, 1.5f, new Noise(seed)));
        noiseSettings.Add(new NoiseSetting(0.015f, -0.2f, 0.2f, new Noise(seed + 1)));

        BiomeData Forest = new BiomeData(BlocksTypes.blockTypes[4], noiseSettings);
        biomeTypes.Add(Biome.Forest, Forest);

        mapNoise = new Noise(seed);
        biomeNoise = new NoiseSetting(0.005f, -0.8f, 0.8f, new Noise(seed + 2));


    }

    public int[,,] generateChunkInfo(Vector2Int chunkPosition, int chunkSize, bool meshPass){

        int[,,] chunkData = TerrainNoiseFilter(chunkPosition, chunkSize);

        return chunkData;
    }

    private int[,,] TerrainNoiseFilter(Vector2Int chunkPosition, int chunkSize){

        int[,,] terrainData = new int[chunkSize * 2, GlobalVariables.chunkHeight,chunkSize * 2];  

        for(int z = -chunkSize; z < chunkSize; z++){
            for(int x = -chunkSize; x < chunkSize; x++){

                Vector2Int realCoord = chunkPosition + new Vector2Int(x, z);
                Vector3 realPosition = new Vector3(realCoord.x, 0, realCoord.y);

                float seaNumber = noiseHandler.Evaluate(realPosition, 0.003f, -0.7f, 0.9f, mapNoise);
                float levelGround = Mathf.Clamp(seaNumber / 2, 0, 0.5f);
                float roughness = noiseHandler.Evaluate(realPosition, 0.025f, -0.1f, 0.1f, mapNoise);
                float hillyness = noiseHandler.Evaluate(realPosition, 0.01f, 0f, 0.5f, mapNoise) * roughness * 2;

                Biome biome = seaNumber + roughness > 0 ? Biome.Grassy : Biome.Seabed;
                
                float forest = noiseHandler.Evaluate(realPosition, 0.005f, -1f, 1f, biomeNoise.noise);

                if(biome == Biome.Grassy)
                biome =  forest < 0 ? Biome.Grassy : Biome.Forest;

                float multiple = 1 + seaNumber - levelGround + roughness + hillyness;

                int height = (int)(20 * multiple);

                int blockId = biomeTypes[biome].blockType.id;
                terrainData[x + chunkSize,height,z + chunkSize] = blockId;

                for(int y = 0; y < height; y++){

                    terrainData[x + chunkSize,y,z + chunkSize] = blockId;
                }

                if(WorldData.coordinateData.ContainsKey(realCoord)){

                    CoordinateData coordinate = WorldData.coordinateData[realCoord];
                    coordinate.biome = biome;
                    coordinate.blocks = new int[GlobalVariables.chunkHeight];
                    coordinate.heighestBlock = height;
                    for(int y = 0; y < height; y++){

                        coordinate.blocks[y] = biomeTypes[biome].blockType.id;
                    }

                    WorldData.coordinateData[realCoord] = coordinate;
                }
            }
        }

        return terrainData;
    }

    private void SeaGenerationPass(Vector2Int chunkPosition){
        //Deep Area
        //Lagoon Area
        //Beach
        //Land

        for (int z = 0; z < GlobalVariables.chunkSize * 2; z++){
            for (int x = 0; x < GlobalVariables.chunkSize * 2; x++)
            {
                Vector3 coordPosition = new Vector3(x + (chunkPosition.x * 10), 0, z + (chunkPosition.y * 10));
                float biome = 1;//noiseHandler.Evaluate(coordPosition, 0.007f, 1, 0, noi);
                Biome biomeId = biome > 0 ? Biome.Grassy : Biome.Forest;

                CoordinateData tempCoordData = WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)];
                tempCoordData.biome = biomeId;
                WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)] = tempCoordData;
            }
        }
    }

    private void GenerateForestBiome(Vector2Int chunkPosition){

        for (int z = 0; z < GlobalVariables.chunkSize * 2; z++){
            for (int x = 0; x < GlobalVariables.chunkSize * 2; x++)
            {
                Vector3 coordPosition = new Vector3(x + (chunkPosition.x * 10), 0, z + (chunkPosition.y * 10));
                float biome = 1;// noiseHandler.Evaluate(coordPosition, 0.007f, 1, 0);
                Biome biomeId = biome > 0 ? Biome.Grassy : Biome.Forest;

                CoordinateData tempCoordData = WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)];
                tempCoordData.biome = biomeId;
                WorldData.coordinateData[new Vector2Int((int)coordPosition.x, (int)coordPosition.z)] = tempCoordData;
            }
        }
    }
}

public class BiomeData{

    public BlockType blockType;
    public List<NoiseSetting> noiseSettings = new List<NoiseSetting>();

    public BiomeData(BlockType blockType, List<NoiseSetting> noiseSettings){

        this.blockType = blockType;
        this.noiseSettings = noiseSettings;
    }
}

public struct NoiseSetting{

    public float scale;
    public float min;
    public float max;
    public Noise noise;
    

    public NoiseSetting(float scale, float min, float max, Noise noise){

        this.scale = scale;
        this.min = min;
        this.max = max;
        this.noise = noise;
    }
}

public enum Operation{

    Add,
    Subract,
    Multiply,
    Divide,
    Null
}
public enum Biome{

    Grassy,
    Beachy,
    Seabed,
    Forest
}
