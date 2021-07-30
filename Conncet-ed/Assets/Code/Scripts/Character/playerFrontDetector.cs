
using System.Collections;
using TMPro;
using UnityEngine;

public class playerFrontDetector : MonoBehaviour
{
    public bool nearWater;
    public TextMeshProUGUI textNotif;
    bool isShowNotif = false ;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            checkIfRayCastHit();
          
        }
    }
    
    private void checkIfRayCastHit()
    {
        //just check what is realy shore ?
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.collider.gameObject.tag == "Water")
            {
                Debug.DrawRay(transform.position, -transform.up, Color.red);
                //interact with water here
               // Debug.Log("Near water");
                nearWater = true;
                if (!isShowNotif) {
                    isShowNotif = true;
                    textNotif.text = "Press F for Drink";
                    // hide notification after 5 second 
                    StartCoroutine(hideNotif());
                }
            }
            else {
                //for now just check water but in future we can check other object
                nearWater = false;
                textNotif.text = "";
            }
         
        }
    }
    IEnumerator hideNotif() {

        yield return new WaitForSeconds(5);
        textNotif.text = "";
    }
}
