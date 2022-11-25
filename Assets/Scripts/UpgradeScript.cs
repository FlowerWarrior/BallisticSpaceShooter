using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeScript : MonoBehaviour
{
    [SerializeField] string savedVarName;
    [SerializeField] string displayName;
    [SerializeField] int defaultValue;
    [SerializeField] int maxValue;
    [SerializeField] int initialCost;
    [SerializeField] TMPro.TextMeshProUGUI upgradeNameText;
    [SerializeField] TMPro.TextMeshProUGUI costText;
    [SerializeField] GameObject costIconObj;
    [SerializeField] Animation notEnoughAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUIValues();
    }

    void UpdateUIValues()
    {
        int upgradeLevel = PlayerPrefs.GetInt(savedVarName, defaultValue);
        int cost = initialCost + initialCost * (upgradeLevel - defaultValue);

        upgradeNameText.text = displayName + ": " + upgradeLevel.ToString();
        costText.text = cost.ToString();

        if (upgradeLevel >= maxValue)
        {
            upgradeNameText.text = displayName + ": " + upgradeLevel.ToString() + " (max)";
            costIconObj.SetActive(false);
            costText.gameObject.SetActive(false);
        }
    }

    public void ButtonClicked()
    {
        Debug.Log("button clicked");

        int gems = PlayerPrefs.GetInt("gems", 0);
        int upgradeLevel = PlayerPrefs.GetInt(savedVarName, defaultValue);
        int cost = initialCost + initialCost * (upgradeLevel - defaultValue);

        if (upgradeLevel < maxValue)
        {
            if (gems >= cost)
            {
                // Increase upgrade value
                PlayerPrefs.SetInt(savedVarName, upgradeLevel + 1);
                // Take player gems
                PlayerPrefs.SetInt("gems", gems - cost);
                // Update gem count display value
                GemUpdateValue.instance.UpdateGemCount();

                UpdateUIValues();
            }
            else
            {
                notEnoughAnim.Play();
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
