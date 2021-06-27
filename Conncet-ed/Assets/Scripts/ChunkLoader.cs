using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    Vector2Int currentChunk;
    [SerializeField] private Events events;

    // Start is called before the first frame update
    void Start()
    {
        currentChunk = CurrentChunk();
        events.MovedChunk(CurrentChunk());
    }

    private Vector2Int CurrentChunk()
    {//Looks at coordinates to determine the chunk its in

        Vector3 playerPos = this.transform.position;
        Vector2Int playerChunk = Vector2Int.RoundToInt(new Vector2(playerPos.x, playerPos.z)) / 10;

        return playerChunk;
    }

    private void ChunkCheck()
    {//Checks if the player has moved from a chunk

        Vector2Int playerChunk = CurrentChunk();
        if (playerChunk.x != currentChunk.x || playerChunk.y != currentChunk.y)
        {

            currentChunk = playerChunk;
            events.MovedChunk(playerChunk);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChunkCheck();
    }
}
