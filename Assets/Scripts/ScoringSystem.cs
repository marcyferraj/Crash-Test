using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SocialPlatforms.Impl;
//Textmeshpro or whatever text UI is in your version goes here
public class ScoringSystem : MonoBehaviour
{
    public static int score;
    public TextMeshProUGUI PlaceholderScoreText;
    public UpgradeSystem upgrade; 
    private bool upgradeAvailable = false;
    void Start()
    {
        score = 1;
        SetText();
        upgrade.GetComponent<UpgradeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
        if(score == 50)
        {
            upgrade.Upgrade();
        }
        
    }
    void SetText()
    {
        PlaceholderScoreText.text = "Score: " + score.ToString(); 
    }

    public void AddScore()
    {
        score++;
        SetText();
    }

    public void MinusScore()
    {
        score--;
        SetText();
    }
}
