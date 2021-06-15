using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    public event Action<Vector2Int> movedChunk;
    public event Action<Vector3> leftClick;

    public void LeftClick(Vector3 position){
        if(leftClick != null){
            leftClick(position);
        }
    }

    public void MovedChunk(Vector2Int chunkID){
        if(chunkID != null && movedChunk != null){
            movedChunk(chunkID);
        }
    }
}
