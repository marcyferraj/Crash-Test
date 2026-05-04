using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    private List<string> upgrades = new List<string>
    {
        "Increase Movement Speed",
        "Enemy Multiplier",
        "Score Multiplier",
        "Jump Height",
        "Luck"
    };

    [Header("UI Options")]
    public TMP_Text option1;
    public TMP_Text option2;
    public TMP_Text option3;

    [Header("Scene References")]
    public GameObject Menu;
    public CharacterMovement player;

    public static int scoreMultiplier = 1;

    private bool moveSpeedUpgrade;
    private bool jumpUpgrade;
    private bool scoreUpgrade;

    private string upgrade1;
    private string upgrade2;
    private string upgrade3;

    void Start()
    {
        Menu.SetActive(false);
    }

    void Update()
    {
        ApplyUpgrades();
    }

    // =========================
    // APPLY UPGRADES
    // =========================
    void ApplyUpgrades()
    {
        if (moveSpeedUpgrade)
            player.moveSpeed = 10f;

        if (jumpUpgrade)
            player.jumpHeight = 8f;

        if (scoreUpgrade)
            scoreMultiplier = 2;
    }

    // =========================
    // OPEN MENU
    // =========================
    public void Upgrade()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;

        List<string> pool = new List<string>(upgrades);

        upgrade1 = Draw(pool);
        upgrade2 = Draw(pool);
        upgrade3 = Draw(pool);

        option1.text = upgrade1;
        option2.text = upgrade2;
        option3.text = upgrade3;
    }

    string Draw(List<string> pool)
    {
        int index = Random.Range(0, pool.Count);
        string chosen = pool[index];
        pool.RemoveAt(index);
        return chosen;
    }

    // =========================
    // BUTTON FUNCTIONS
    // =========================
    public void Upgrade1Picked() => ApplyAndClose(upgrade1);
    public void Upgrade2Picked() => ApplyAndClose(upgrade2);
    public void Upgrade3Picked() => ApplyAndClose(upgrade3);

    void ApplyAndClose(string type)
    {
        ApplyUpgrade(type);
        CloseMenu();
    }

    // =========================
    // APPLY UPGRADE
    // =========================
    void ApplyUpgrade(string type)
    {
        if (type == "Increase Movement Speed")
        {
            moveSpeedUpgrade = true;
            player.moveSpeed = 10f;
        }
        else if (type == "Jump Height")
        {
            jumpUpgrade = true;
            player.jumpHeight = 8f;
        }
        else if (type == "Score Multiplier")
        {
            scoreUpgrade = true;
            scoreMultiplier = 2;
        }
        else if (type == "Enemy Multiplier")
        {
            Debug.Log("not implemented yet");
        }
        else if (type == "Luck")
        {
            Debug.Log("not implemented yet");
        }
    }

    // =========================
    // CLOSE MENU
    // =========================
    void CloseMenu()
    {
        Menu.SetActive(false);
        Time.timeScale = 1f;
    }
}