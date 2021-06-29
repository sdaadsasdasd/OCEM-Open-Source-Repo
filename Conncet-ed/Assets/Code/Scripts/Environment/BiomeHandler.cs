using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeHandler : MonoBehaviour
{
    public static Dictionary<Biome, BiomeData> biomeTypes = new Dictionary<Biome, BiomeData>();

    Noise mapNoise;
    Noise biomeNoise;
    NoiseHandler noiseHandler = new NoiseHandler();

    [SerializeField]private int seed;

    public static void initializeBiomes(){

        biomeTypes.Add(Biome.Grassy, new BiomeData(0.01f, 5f, 20f, BlocksTypes.blockTypes[1]));
        biomeTypes.Add(Biome.Beachy, new BiomeData(0.01f, 5f, 20f, BlocksTypes.blockTypes[3]));
        biomeTypes.Add(Biome.Seabed, new BiomeData(0.01f, 5f, 20f, BlocksTypes.blockTypes[2]));
        biomeTypes.Add(Biome.Forest, new BiomeData(0.01f, 5f, 20f, BlocksTypes.blockTypes[4]));

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

    public int[,,] generateChunkInfo(Vector2Int chunkPosition, int chunkSize, bool meshPass){

        int[,,] chunkData = new int[chunkSize * 2, UVariables.chunkHeight, chunkSize * 2];

        for(int z = -chunkSize; z < chunkSize; z++){
            for(int x = -chunkSize; x < chunkSize; x++){

                Vector2Int realCoord = new Vector2Int(x + (chunkPosition.x) + chunkSize, z + (chunkPosition.y) + chunkSize);

                CoordinateData tempCoordData;
                BiomeData biome;
                int noiseValue;

                if(meshPass){
                    
                    Vector3 coordPosition = new Vector3(realCoord.x, 0, realCoord.y);

                    float biomeId = noiseHandler.Evaluate(coordPosition, 0.007f, 1, 0);
                    biome = BiomeHandler.biomeTypes[Biome.Grassy];
                    noiseValue = (int)noiseHandler.Evaluate(new Vector3(realCoord.x,0,realCoord.y), biome.scale, biome.amplitude, biome.height);

                    for(int i = 0; i < noiseValue; i++){
                        chunkData[x + chunkSize,i, z + chunkSize] = biome.blockType.id;
                    }
                    
                } else {

                    tempCoordData = WorldData.coordinateData[realCoord];
                    biome = BiomeHandler.biomeTypes[Biome.Grassy];
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
}

public class BiomeData{

    public float scale {get;}
    public float frequency {get;}
    public float curve {get;}
    public BlockType blockType {get;}

    public float amplitude {get; private set;}
    public float height {get; private set;}

    public BiomeData(float _scale, float _amplitude, float _height, BlockType _blockType){

        scale = _scale;
        amplitude = _amplitude;
        height = _height;
        blockType = _blockType;
    }
}

public enum Biome{

    Grassy,
    Beachy,
    Seabed,
    Forest
}
