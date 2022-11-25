using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBonus : MonoBehaviour
{
    [SerializeField] float hideAfterSec = 1.5F;
    [HideInInspector] public int bonus = 0;
    [HideInInspector] public string leftText = "+ ";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddAfter());
        transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator AddAfter()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = leftText + bonus.ToString();
        yield return new WaitForSeconds(1.5F);
        GM.instance.AddToScore(bonus);
        Destroy(gameObject);
    }
}
