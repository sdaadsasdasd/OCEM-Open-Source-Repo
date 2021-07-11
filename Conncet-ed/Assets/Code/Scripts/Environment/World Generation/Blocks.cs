using System.Collections.Generic;
using UnityEngine;

public static class Blocks
{
    public static Dictionary<int, BlockType> blockTypes = new Dictionary<int, BlockType>();

    public static void initializeBlocks(){

        blockTypes.Add(1, new BlockType(1, new Vector2(0,0), "Grass"));
        blockTypes.Add(2, new BlockType(2, new Vector2(1,0), "Stone"));
        blockTypes.Add(3, new BlockType(3, new Vector2(2,0), "Sand"));
        blockTypes.Add(4, new BlockType(4, new Vector2(3,0), "Forest_Grass"));
    } //Add enums to this
}

public class BlockType{

    public Vector2 textureCoords;
    public int id;
    public string name;

    public BlockType(int _id, Vector2 _textureCoords, string _name){
        id = _id;
        textureCoords = _textureCoords;
        name = _name;
    }
}
