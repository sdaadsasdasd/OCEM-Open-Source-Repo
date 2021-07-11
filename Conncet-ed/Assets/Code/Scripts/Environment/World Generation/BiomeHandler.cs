using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeHandler : MonoBehaviour
{
    public static Dictionary<string, NoiseSetting> biomeNoises = new Dictionary<string, NoiseSetting>();
    public static Dictionary<Biome, BlockType> biomes =  new Dictionary<Biome, BlockType>();

    NoiseHandler noiseHandler = new NoiseHandler();
    NoiseSetting biomeNoise;

    [SerializeField]private int seed;

    public void initializeBiomes(){

        biomes.Add(Biome.Seabed, Blocks.blockTypes[3]);
        biomes.Add(Biome.Grassy, Blocks.blockTypes[1]);
        biomes.Add(Biome.Forest, Blocks.blockTypes[4]);

        biomeNoises.Add("Global Wide Noise",        new NoiseSetting(0.003f, -0.7f, 0.9f, new Noise(seed)));
        biomeNoises.Add("Global Biome Roughness",   new NoiseSetting(0.025f, -0.1f, 0.1f, new Noise(seed + 3))); 
        biomeNoises.Add("Global Roughness",         new NoiseSetting(0.025f, -0.1f, 0.1f, new Noise(seed))); 
        biomeNoises.Add("Grassy Biome Noise",       new NoiseSetting(0.005f, -1f, 1f, new Noise(seed + 1))); 
        biomeNoises.Add("Forest Biome Noise",       new NoiseSetting(0.005f, -1f, 1f, new Noise(seed + 2))); 
        biomeNoises.Add("Land Hillyness",           new NoiseSetting(0.01f, 0f, 0.5f, new Noise(seed + 2))); 
    }

    public int[,,] terrainNoise(Vector2Int chunkPosition, int chunkSize){

        int[,,] terrainData = new int[chunkSize * 2, GlobalVariables.chunkHeight,chunkSize * 2];  

        for(int z = -chunkSize; z < chunkSize; z++){
            for(int x = -chunkSize; x < chunkSize; x++){

                Vector2Int coord = chunkPosition + new Vector2Int(x, z);
                Vector3 realPosition = new Vector3(coord.x, 0, coord.y);

                Biome biome = GenerateBiomeNumber(realPosition);

                int height = (int)(20 * GenerateHeightFactor(biome,realPosition));

                int blockId = biomes[biome].id;
                terrainData[x + chunkSize,height,z + chunkSize] = blockId;

                for(int y = 0; y < height; y++){

                    terrainData[x + chunkSize,y,z + chunkSize] = blockId;
                }

                if(WorldData.coordinateData.ContainsKey(coord)){

                    CoordinateData coordinate = WorldData.coordinateData[coord];
                    coordinate.biome = biome;
                    coordinate.blocks = new int[GlobalVariables.chunkHeight];
                    coordinate.heighestBlock = height;
                    for(int y = 0; y < height; y++){

                        coordinate.blocks[y] = biomes[biome].id;
                    }

                    WorldData.coordinateData[coord] = coordinate;
                }
            }
        }

        return terrainData;
    }

    private Biome GenerateBiomeNumber(Vector3 realPosition){

        //Create static noise settings 
        //Each biome a specific seed
        //Generate a float number for every biome
            //First comes sea
            //Index 0 is Grassy
            //Index 1 is Forest
        //Save the highest number and use that

        float globalWideNoise = noiseHandler.Evaluate(realPosition, biomeNoises["Global Wide Noise"]);
        float globalRoughness = noiseHandler.Evaluate(realPosition, biomeNoises["Global Biome Roughness"]);

        Biome biome = globalWideNoise + globalRoughness > 0 ? Biome.Grassy : Biome.Seabed;

        if(biome == Biome.Grassy){

            float[] biomeNumbers = new float[2];

            biomeNumbers[0] = noiseHandler.Evaluate(realPosition, biomeNoises["Grassy Biome Noise"]);
            biomeNumbers[1] = noiseHandler.Evaluate(realPosition, biomeNoises["Forest Biome Noise"]);

            biome = biomeNumbers[0] >= biomeNumbers[1] ? Biome.Grassy : Biome.Forest;
        }

        return biome;
    }

    private float GenerateHeightFactor(Biome biome, Vector3 realPosition){

        //Have compatibility with multiple noise functions for every biome.
        //Create mask noises for everybiome

        float multiple = 1;

        float globalWideNoise = noiseHandler.Evaluate(realPosition, biomeNoises["Global Wide Noise"]);
        multiple += noiseHandler.Evaluate(realPosition, biomeNoises["Global Roughness"]) + globalWideNoise;

        switch(biome){
            case Biome.Seabed:{

                
                break;
            }
            case Biome.Grassy:{

                float maskSea = Mathf.Clamp(globalWideNoise * 2f, 0, 1);
                float hillyness = noiseHandler.Evaluate(realPosition, biomeNoises["Land Hillyness"]) * maskSea;

                float levelGround = Mathf.Clamp(globalWideNoise / 1.5f, 0f, 0.5f);

                multiple += hillyness;
                multiple -= levelGround;

                break;
            }
            case Biome.Forest:{

                float maskSea = Mathf.Clamp(globalWideNoise * 2f, 0, 1);
                float hillyness = noiseHandler.Evaluate(realPosition, biomeNoises["Land Hillyness"]) * maskSea;

                float levelGround = Mathf.Clamp(globalWideNoise / 1.5f, 0f, 0.75f);

                multiple += hillyness;
                multiple -= levelGround;

                break;  
            }
        }

        return multiple;
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
    Divide
}
public enum Biome{

    Seabed,
    Grassy = 1,
    Forest = 2
}
