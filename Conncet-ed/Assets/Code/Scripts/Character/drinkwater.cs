using StarterAssets;
using System.Collections;
using UnityEngine;

public class drinkwater : MonoBehaviour
{
    public playerFrontDetector _detect;
    private StarterAssetsInputs _input;
    //for test purpose 
    public int _thirst = 50;

    // Start is called before the first frame update
    void Start()
    {
        _detect = this.gameObject.GetComponentInChildren<playerFrontDetector>();
        _input = GetComponent<StarterAssetsInputs>();
        StartCoroutine(consumeThirs());
    }

    // Update is called once per frame
    void Update()
    {
      
        if (_input.interact)
        {
            if (_detect.nearWater)
            {
                Debug.Log("Player Drinking");
                drink();
                _input.interact = false;
                //this is for drinking
            }
        }
        
    }
    //every 3 second thirst -1 max thirst 100;
     IEnumerator consumeThirs() {
        yield return new WaitForSeconds(3f);
        _thirst -= 1;
        StartCoroutine(consumeThirs());
    }
    private void drink() {
        if (_thirst < 100)
        {
            _thirst += 10;
        }
        else {
            Debug.Log("You Not Thirst For Now!");
        }
    }


}
