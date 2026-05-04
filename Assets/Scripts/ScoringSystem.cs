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
    public static int scoreMultiplier = 1;
    public int upgradeInterval = 50;
    private int nextUpgradeAt = 50;
    void Start()
    {
        score = 1;
        SetText();
        upgrade.GetComponent<UpgradeSystem>();
    }

    void Update()
    {
        SetText();

        if (score >= nextUpgradeAt)
        {
            upgrade.Upgrade();
            nextUpgradeAt += upgradeInterval;
        }
    }
    void SetText()
    {
        PlaceholderScoreText.text = "Score: " + score.ToString();
    }

    public void AddScore()
    {
        score += scoreMultiplier;
        SetText();
    }

    public void MinusScore()
    {
        score--;
        SetText();
    }
}
