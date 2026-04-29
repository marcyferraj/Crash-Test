using NUnit.Framework;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public float GameTime = 300f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float ElapsedTime;
    private bool IsRunning = true;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsRunning) return;

        ElapsedTime += Time.deltaTime;
        if (ElapsedTime >= GameTime)
        {
            ElapsedTime = GameTime;
            IsRunning = false;
            OnTimerFinished();
            
        }
    }

    void OnTimerFinished()
    {
        
    }
}
