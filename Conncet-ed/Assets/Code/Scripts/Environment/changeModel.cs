using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeModel : MonoBehaviour
{

    int model_num = 0;
    [SerializeField] GameObject[] models_array;
    [SerializeField] float[] timer_array;
    GameObject current_model;
    [SerializeField] bool it_loops = false;

    Coroutine change_model_coroutine;


    void Start()
    {
        current_model = Instantiate(models_array[model_num], transform.position, transform.rotation) as GameObject;
        current_model.transform.parent = transform;

        change_model_coroutine = StartCoroutine(ChangeModelCoroutine());
    }

    void Update()
    {
        
    }

    private IEnumerator ChangeModelCoroutine()
    {
        yield return new WaitForSeconds(timer_array[model_num]);
        ChangeModel();

        StartCoroutine(ChangeModelCoroutine());
    }

    private void ChangeModel()
    {
        if(model_num + 1 < models_array.Length)
        {
            model_num++;
            GameObject temp_model = Instantiate(models_array[model_num], transform.position, transform.rotation) as GameObject;
            Destroy(current_model);
            temp_model.transform.parent = transform;
            current_model = temp_model;
        }
        else if(it_loops)
        {
            model_num = -1;
            ChangeModel();
        }
        
    }

}
