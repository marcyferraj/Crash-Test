using UnityEngine;
using TMPro;
//Textmeshpro or whatever text UI is in your version goes here
public class ScoringSystem : MonoBehaviour
{
    public static int score;
    public TextMeshProUGUI PlaceholderScoreText;
    public UpgradeSystem upgrade; 
    void Start()
    {
        score = 0;
        SetText();
        upgrade.GetComponent<UpgradeSystem>();


    }

    // Update is called once per frame
    void Update()
    {
        SetText();
        if (score == 1)
        {
            score += 1; //upgrade bonus point purely to fix constant calling, could add in later on as actual score bonus or not
            upgrade.Upgrade();
        }
    }
    void SetText()
    {
        PlaceholderScoreText.text = "Score: " + score.ToString();
        
    }
}
