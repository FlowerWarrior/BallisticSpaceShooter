using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Sprite fullImg;
    [SerializeField] Sprite emptyImg;
    [SerializeField] Transform barPartPrefab;
    [SerializeField] HorizontalLayoutGroup horLayout;

    public void UpdateBar(int hp)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < hp)
            {
                transform.GetChild(i).gameObject.GetComponent<Image>().sprite = fullImg;
            }
            else
            {
                transform.GetChild(i).gameObject.GetComponent<Image>().sprite = emptyImg;
            }    
        }
    }

    public void InitiateBars(int parts)
    {
        // Destroy template
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Set correct spacing 10 -> -7, 20 -> -20
        horLayout.spacing = -7 - (parts - 10) * 1.3F;

        // Create hp bar parts
        for (int i = 0; i < parts; i++)
        {
            Transform partT = Instantiate(barPartPrefab, transform.position, transform.rotation);
            partT.parent = transform;
        }
    }
}
