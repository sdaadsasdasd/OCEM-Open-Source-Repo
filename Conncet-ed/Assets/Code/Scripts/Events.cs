using UnityEngine;
using System;

//This class/script handles all the custom events.
//Events make funtions that span multiple scripts much easier to implement.
//It also reduces the amount of references between scripts, which makes updating/deleting scripts that interact with one another much easier.
//What they do is, they enable an entire set of functions, from different scripts to be executed all at once.
//All you have to do is execute the event, then all functions subscribed to it will execute.

//The way to do it is:

//1. Declare an event Action: "public event Action example". Depending on the usage of it, you can also assign a return type: "public event Action<float> example"
/*
    public event Action exampleEvent;                   Note: the event's return type has to match the function it is subsribing to.
    public event Action<float> exampleEventFloat;       <== Example of an event with return type float.

    //If the return type is ommited, the event considers it as a void return type.
*/  

//2. Encase the event in a public void, so that when calling the event, you call it through the function. 
//   Within the encasing, check if the event is not null. If it isnt, execute the event.
//   This is because if the event were called while there was no function assigned, it will return an error.
/*
    public event Action<float> exampleEventFloat

    public void ExampleEventFloat(float returnFloat){
        if(exampleEventFloat != null){
            exampleEventFloat(returnFloat);
        }
    }
*/ 

//3. Go to the script which has the function you want this event to execute, and create a reference to this script there.
/*
    [SerializeField]private Events events;              <== SerializeField is a good practice to keep variables private, while displaying in the editor.
*/

//4. In the Start/enable method there, simply add the desired function to the event:
/*
    void Start(){
        events.exampleEvent += ExampleFunction;         Note that there is no parenthesis() when adding a function
        events.exampleEventFloat += ExampleFunctionFloat;          
        . . .    
    }
*/
//This subsribes the function to the event
//Now, any script that has a reference to this events script, can call those functions subscribed to the event.
/*
    events.ExampleEvent();                              <== For void return type
    events.ExampleEventFloat(someFloat);                <== For other return types
*/
public class Events : MonoBehaviour
{
    public event Action<Vector2Int> movedChunk;

    public void startMovedChunk(Vector2Int chunkID){
        if(chunkID != null && movedChunk != null){
            movedChunk(chunkID);
        }
    }
}
