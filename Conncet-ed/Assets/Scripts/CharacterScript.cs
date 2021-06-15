using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    CharacterController cc;
    Events events;
    Vector2Int currentChunk;
    Rigidbody rb;

    
    int speed = 10;

    private float mass;

    private Vector3 movingTo = new Vector3();

    void Start()
    {
        events = FindObjectOfType<Events>();
        rb = this.GetComponent<Rigidbody>();
        cc = this.GetComponent<CharacterController>();

        currentChunk = CurrentChunk();
        events.MovedChunk(CurrentChunk());
    }

    private void Update()
    {
        ChunkCheck();
    }

    private void FixedUpdate()
    {
        Hover();
        SlowDown();
    }

    private void ChunkCheck(){

        Vector2Int playerChunk = CurrentChunk();
        if(playerChunk.x != currentChunk.x || playerChunk.y != currentChunk.y){

            currentChunk = playerChunk;
            events.MovedChunk(playerChunk);
        }
    }

    private Vector2Int CurrentChunk(){

        Vector3 playerPos = this.transform.position;
        Vector2Int playerChunk = Vector2Int.RoundToInt(new Vector2(playerPos.x, playerPos.z)) / 10;

        return playerChunk;
    }

    private void Hover(){
        
         //Gravity
        float upForce = 9.81f;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit)){

            upForce += -(hit.distance * 2f) + 2f;
        }

        Mathf.Clamp(upForce, 20f, 0f);
        
        
        if(upForce >= 9.75f && upForce <= 9.85f){
            upForce = 9.81f;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        rb.AddForce(Vector3.up * upForce, ForceMode.Force);
        //Make sure distance is 1 and down/up velocity is 0 when force is 9.81
    }

    private void SlowDown(){
        if(InRange()){
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private bool InRange(){

        Vector3 distance = new Vector3(movingTo.x, 0, movingTo.z) - new Vector3(transform.position.x, 0, transform.position.z);
        return Mathf.Abs((distance).magnitude) <= 0.1f;
    }

    private void MoveToClick(Vector3 mousePos){

        Vector3 relativePoint = mousePos - transform.position;
        Vector3 velocity = relativePoint.normalized * speed;
        movingTo = mousePos;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }
}
