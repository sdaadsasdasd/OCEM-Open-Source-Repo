using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class mushroom : MonoBehaviour
{
    //public int hunger = 0;
    public int gain = 10;
    public Slider healthSlider;
    public float maxHealth = 100;
    public float rate = 2;
    public Text energyTxt;

    private float health = 0;
   

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if(health > 0)
        {
            health -= Time.deltaTime * rate;
        }
        healthSlider.value = health;
        energyTxt.text = "ENERGY: " + health.ToString("0.00");

    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "edible")
        {
            if(health < maxHealth)
            {
                health += gain;
                Destroy(other.gameObject);
            }
                       
        }
    }
}
