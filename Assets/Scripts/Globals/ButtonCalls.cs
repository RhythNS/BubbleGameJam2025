using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonCalls : MonoBehaviour
{
    //[SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private int[] upgradeCosts = new int[4] { 50, 100, 200, 500};
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
    [HideInInspector] public int Points
    {
        get { return _points; }
        set 
        { 
            _points = value;
            pointsText.text = "Points\r\n" + _points.ToString();
        }
    }
    private int _points = 9999;

    private int healthLevel = 0;
    private int regenLevel = 0;
    private int moveSpeedLevel = 0;
    private int airLossLevel = 0;
    private int boostLevel = 0;

    void Start()
    {
        Points = 9999;
        healthUpgradeText.text = "HEALTH\r\n" + upgradeCosts[healthLevel].ToString();
        regenUpgradeText.text = "REGEN\r\n" + upgradeCosts[regenLevel].ToString();
        moveSpeedUpgradeText.text = "MOVESPEED\r\n" + upgradeCosts[moveSpeedLevel].ToString();
        airLossUpgradeText.text = "AIR LOSS\r\n" + upgradeCosts[airLossLevel].ToString();
        boostUpgradeText.text = "BOOST\r\n" + upgradeCosts[boostLevel].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickPlay()
    {
        //GameManager.Instance.RequestStart();
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
