using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCalls : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private int[] upgradeCosts = new int[4] { 50, 100, 200, 500};
    public float[] healthMults = new float[5] {1.0f, 1.25f, 1.5f, 1.75f, 2.0f };
    public float[] regenMults = new float[5] {1.0f, 1.25f, 1.5f, 1.75f, 2.0f };
    public float[] moveSpeedMults = new float[5] {1.0f, 1.25f, 1.5f, 1.75f, 2.0f };
    public float[] airLossMults = new float[5] {1.0f, 0.85f, 0.7f, 0.55f, 0.4f };
    public float[] boostMults = new float[5] {1.0f, 1.25f, 1.5f, 1.75f, 2.0f };
    [SerializeField] private TMP_Text healthUpgradeText;
    [SerializeField] private Image[] healthUpgradeImages;
    [SerializeField] private TMP_Text regenUpgradeText;
    [SerializeField] private Image[] regenUpgradeImages;
    [SerializeField] private TMP_Text moveSpeedUpgradeText;
    [SerializeField] private Image[] moveSpeedUpgradeImages;
    [SerializeField] private TMP_Text airLossUpgradeText;
    [SerializeField] private Image[] airLossUpgradeImages;
    [SerializeField] private TMP_Text boostUpgradeText;
    [SerializeField] private Image[] boostUpgradeImages;
    [SerializeField] private TMP_Text pointsText;
    [HideInInspector] public float Points
    {
        get { return _points; }
        set 
        { 
            _points = value;
            pointsText.text = "Points\r\n" + ((int)_points).ToString();
        }
    }
    private float _points = 0;

    [HideInInspector] public int healthLevel = 0;
    [HideInInspector] public int regenLevel = 0;
    [HideInInspector] public int moveSpeedLevel = 0;
    [HideInInspector] public int airLossLevel = 0;
    [HideInInspector] public int boostLevel = 0;

    void Start()
    {
        Points = 9999;
        healthUpgradeText.text = "HEALTH\r\n" + upgradeCosts[healthLevel].ToString();
        regenUpgradeText.text = "REGEN\r\n" + upgradeCosts[regenLevel].ToString();
        moveSpeedUpgradeText.text = "MOVESPEED\r\n" + upgradeCosts[moveSpeedLevel].ToString();
        airLossUpgradeText.text = "AIR LOSS\r\n" + upgradeCosts[airLossLevel].ToString();
        boostUpgradeText.text = "BOOST\r\n" + upgradeCosts[boostLevel].ToString();
    }

    public void OnClickPlay()
    {
        GameManager.Instance.RequestStart();
        mainMenu.SetActive(false);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickUpgrades()
    {
        mainMenu.SetActive(false);
        upgradeMenu.SetActive(true);
    }

    public void OnClickMainMenu()
    {
        upgradeMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private bool OnClickUpgrade(ref int level, Image[] upgradeImages, TMP_Text upgradeText, string text)
    {
        if (level == 4) { return false; }
        if (Points < upgradeCosts[level]) { return false; }
        Points -= upgradeCosts[level];
        upgradeImages[level].color = Color.green;
        level++;
        if (level == 4) { upgradeText.text = text + "\r\nMAX"; }
        else { upgradeText.text =  text + "\r\n" + upgradeCosts[level].ToString(); }
        return true;
    }

    public void OnClickHealthUpgrade()
    {
        OnClickUpgrade(ref healthLevel, healthUpgradeImages, healthUpgradeText, "HEALTH");
    }

    public void OnClickRegenUpgrade()
    {
        OnClickUpgrade(ref regenLevel, regenUpgradeImages, regenUpgradeText, "REGEN");
    }

    public void OnClickMoveSpeedUpgrade()
    {
        OnClickUpgrade(ref moveSpeedLevel, moveSpeedUpgradeImages, moveSpeedUpgradeText, "MOVESPEED");
    }

    public void OnClickAirLossUpgrade()
    {
        OnClickUpgrade(ref airLossLevel, airLossUpgradeImages, airLossUpgradeText, "AIR LOSS");
    }

    public void OnClickBoostUpgrade()
    {
        OnClickUpgrade(ref boostLevel, boostUpgradeImages, boostUpgradeText, "BOOST");
    }
}
