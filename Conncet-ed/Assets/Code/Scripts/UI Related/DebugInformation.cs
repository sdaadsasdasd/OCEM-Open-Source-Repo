using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInformation : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private TMP_Text txtRealPosition;
    [SerializeField]private TMP_Text txtBlockCoord;
    [SerializeField]private TMP_Text txtChunk;
    [SerializeField]private TMP_Text txtChunkCoord;
    [SerializeField]private TMP_Text txtRealCoord;
    [SerializeField]private TMP_Text txtBiome;
    [SerializeField]private Events events;

    private Vector2Int chunkPosition;
    
    void Start()
    {
        events.movedChunk += UpdateChunk;
    }

    private void UpdateChunk(Vector2Int chunkPosition){

        this.chunkPosition = chunkPosition;
    }

    
    void LateUpdate()
    {
        txtChunk.text = "Chunk: " + chunkPosition.ToString();
        Vector3 position = player.position;
        txtRealPosition.text = "Real Position: " + position.ToString();
        Vector2Int blockCoordinate = new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.z));
        txtBlockCoord.text = "Block Coordinate: " + blockCoordinate.ToString();
        Vector2Int chunkCoordinate = blockCoordinate - (chunkPosition * 10);
        txtChunkCoord.text = "Chunk Coordinate: " + chunkCoordinate.ToString();
        Vector2Int realCoods =  WorldData.loadedChunks[chunkPosition].realCoords[chunkCoordinate.x, chunkCoordinate.y];
        txtRealCoord.text = "Real Coordinate: " + realCoods.ToString();
        Biome biome = WorldData.coordinateData[blockCoordinate].biome;
        txtBiome.text = "Biome: " + biome;

    }
}
