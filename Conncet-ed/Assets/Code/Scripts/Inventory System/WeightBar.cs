using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightBar : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private InventoryManager manager;
    [SerializeField] private TMPro.TMP_Text text;

    void Update()
    {
        bar.anchorMax = new Vector2((float)(manager.getWeight()/manager.getWeightCap()), 1);
        text.text = manager.getWeight() + " / " + manager.getWeightCap();
    }
}
