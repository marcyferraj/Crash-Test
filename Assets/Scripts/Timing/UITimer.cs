using TMPro;
using UnityEngine;
public class UITimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameTimer.Instance.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = GameTimer.FormatTime(GameTimer.Instance.ElapsedTime);
    }
}
