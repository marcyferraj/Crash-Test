using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class UpgradeSystem : MonoBehaviour
{
    private List<string> Upgrades = new List<string> { "Increase Movement Speed", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6" };
    public  TMP_Text option1;
    public  TMP_Text option2;
    public  TMP_Text option3;
    int upgrade1;
    int upgrade2;
    int upgrade3;
    public static GameObject Menu;
    public CharacterMovement player;
    public bool moveSpeedUpgradeToggle;


    void Start()
    {
        Menu = GameObject.Find("Menu");
        Menu.gameObject.SetActive(false);
        player.GetComponent<CharacterMovement>();
        moveSpeedUpgradeToggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveSpeedUpgradeToggle == true)
        {
            player.moveSpeed = 10f;
        }
    }

    public void Upgrade()
    {
        
        Menu.gameObject.SetActive(true);
        upgrade1 = Random.Range(0, Upgrades.Count);
        option1.text = Upgrades[upgrade1];
        upgrade2 = Random.Range(0, Upgrades.Count);
        option2.text = Upgrades[upgrade2];
        upgrade3 = Random.Range(0, Upgrades.Count);
        option3.text = Upgrades[upgrade3];
        
    }

    public void Upgrade1Picked()
    {
        if (option1.text == "Increase Movement Speed")
        {
            moveSpeedUpgradeToggle = true;
        }
    }

    
}
