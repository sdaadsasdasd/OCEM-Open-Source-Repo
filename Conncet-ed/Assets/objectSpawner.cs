using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects to spawn")]
    public List<GameObject> objects = new List<GameObject>();

    [Header("Reference")]
    GameObject reference;

    // Start is called before the first frame update
    void Start()
    {
        //Renderer deaktivieren
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
            renderer.enabled = false;

        //Referenzobjekt wird zerstört, falls vorhanden
        if (reference)
            Destroy(reference);

        //Random Objekt spawnen
        int index = Random.Range(0, objects.Count);
        Instantiate(objects[index], transform);
    }
}