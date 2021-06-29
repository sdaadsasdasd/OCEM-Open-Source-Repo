using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Biomes
{
    public static Dictionary<int, BiomeType> biomeTypes = new Dictionary<int, BiomeType>();
    public static int numBiomes {get; private set;}

    public static void initializeBiomes(){

        biomeTypes.Add(1, new BiomeType(1, "Grassy", 0.01f, 5f, 20f, BlocksTypes.blockTypes[1]));
        biomeTypes.Add(2, new BiomeType(2, "Beach" , 0.01f, 5f, 20f,  BlocksTypes.blockTypes[3]));
        biomeTypes.Add(3, new BiomeType(3, "Seabed" , 0.01f, 5f, 20f, BlocksTypes.blockTypes[2]));
        biomeTypes.Add(4, new BiomeType(4, "Forest" , 0.01f, 5f, 20f, BlocksTypes.blockTypes[4]));

    } //Add enums to this
}

public class BiomeType{

    public int id {get;}
    public string name {get;}
    public float scale {get;}
    public float frequency {get;}
    public float curve {get;}
    public BlockType blockType {get;}

    public float amplitude {get; private set;}
    public float height {get; private set;}

    public BiomeType(int _id, string _name, float _scale, float _amplitude, float _height, BlockType _blockType){

        id = _id;
        name = _name;
        scale = _scale;
        amplitude = _amplitude;
        height = _height;
        blockType = _blockType;
    }
}
