using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMG : MonoBehaviour
{
    public static UIMG instance;
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    [SerializeField] TMPro.TextMeshProUGUI pointsText; 
    [SerializeField] TMPro.TextMeshProUGUI multiplierText; 
    [SerializeField] TMPro.TextMeshProUGUI comboText;
    [SerializeField] Canvas gameplayCanvas;
    [SerializeField] Canvas endScreenCanvas;
    [SerializeField] TMPro.TextMeshProUGUI endScoreText;
    [HideInInspector] public Transform player;
    [SerializeField] ComboAnimScript m_ComboAnimScript;
    [SerializeField] healthUIAnim m_healthUIAnim;
    [SerializeField] HealthBarManager m_HealthBarManager;
    [SerializeField] GameObject TargetIndicator;
    [SerializeField] Slider boostSlider;
    [SerializeField] OutlineShake m_OutlineShake;
    [SerializeField] Transform bonusPointsParent;
    [SerializeField] Transform bonusPointsElement;
    [SerializeField] GemCountAnimScript m_GemCountAnimScript;
    int lastCombo = 0;
    int comboStartScore = 0;

    void Awake()
    {
        instance = this;
    }

    public void UpdateGemCount(int amount)
    {
        m_GemCountAnimScript.UpdateGemText(amount.ToString());
    }

    void Start()
    {
        UpdateGemCount(PlayerPrefs.GetInt("gems", 0));
        comboText.text = "x0";
        multiplierText.gameObject.SetActive(false);
        endScreenCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(true);
    }

    public void InitiateHealthBar(int parts)
    {
        m_HealthBarManager.InitiateBars(parts);
    }

    public void UpdateBoostBar(float val)
    {
        boostSlider.value = val;
    }

    public void UpdatePlayerHealth()
    {
        //pointsText.text = pointsText.text + GetComponent<StarshipLifeManager>().starshipHP.ToString();
        healthText.text = "Player HP: " + player.gameObject.GetComponent<StarshipLifeManager>().starshipHP.ToString(); 
        m_healthUIAnim.AnimateDamageTaken();   
        m_OutlineShake.ShakeCamera();
        m_HealthBarManager.UpdateBar(player.gameObject.GetComponent<StarshipLifeManager>().starshipHP);
        Camera.main.gameObject.GetComponent<CameraShake>().ShakeCamera();
    }
    public void UpdateScore(float score)
    {
        pointsText.text = "Score: " + ((int)score).ToString(); 
    }

    public void UpdateMultiplier(float combo)
    {
        multiplierText.text = "Score multiplier:" + combo.ToString(); 
    }

    public void UpdateTargetingIndicator(Vector3 screenPos, Vector3 worldPos, int targetID)
    {
        float distance = Vector3.Distance(worldPos, Camera.main.transform.position);

        if (targetID == 0) // Asteroid
        {
            TargetIndicator.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F) * (54F / distance);
            TargetIndicator.GetComponent<Image>().color = new Color32(255, 100, 0, 255);
        }
        else if (targetID == 1) // Enemy
        {
            TargetIndicator.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F) * (56F / distance);
            TargetIndicator.GetComponent<Image>().color = new Color32(255, 40, 0, 255);
        }
        else if (targetID == 2) // Boss 1
        {
            TargetIndicator.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F) * (144F / distance);
            TargetIndicator.GetComponent<Image>().color = new Color32(255, 40, 0, 255);
        }

        TargetIndicator.transform.position = screenPos;
        TargetIndicator.SetActive(true);
    }

    public void HideTargetingIndicator()
    {
        TargetIndicator.SetActive(false);
    }

    public void UpdateCombo(int combo, int score)
    {
        if (combo == 0 && lastCombo != combo)
        {
            comboText.text = "x0";
            m_ComboAnimScript.LostCombo();
            ShowComboBonus(GM.instance.scoreMultiplier * ((float)score - (float)comboStartScore), 0);
            GM.instance.scoreMultiplier = 1;
        }
        else if (lastCombo == 0 && combo > lastCombo)
        {
            comboStartScore = score;
            comboText.text = "x" + combo.ToString();
            m_ComboAnimScript.UpdatedCombo();
        }
        else if(combo > 0)
        {
            comboText.text = "x" + combo.ToString(); 
            m_ComboAnimScript.UpdatedCombo();
        }
        lastCombo = combo;
    }

    public void AddBonus(int x, int actionID)
    {
        ShowComboBonus(x, actionID);
    }
    
    public void showFinalScore(){
        int score = Mathf.RoundToInt((int)GM.instance.getScore());
        endScoreText.text = "End Score: " + GM.instance.getScore().ToString("f0");
    }
    public void hideInGameUI() {
        gameplayCanvas.gameObject.SetActive(false);
        endScreenCanvas.gameObject.SetActive(true);
    }

    private void ShowComboBonus(float x, int actionID)
    {
        Transform newT = Instantiate(bonusPointsElement);
        Debug.Log(x);
        newT.parent = bonusPointsParent;
        newT.GetComponent<PointsBonus>().bonus = (int)x;

        if (actionID == 0)
        {
            newT.GetComponent<PointsBonus>().leftText = "Combo bonus + ";
        }
        if (actionID == 1)
        {
            newT.GetComponent<PointsBonus>().leftText = "No damage taken + ";
        }
        if (actionID == 2)
        {
            newT.GetComponent<PointsBonus>().leftText = "Enemy defeated + ";
        }
        if (actionID == 3)
        {
            newT.GetComponent<PointsBonus>().leftText = "BOSS DEFEATED! + ";
        }
        
    }
}
