using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Testing")]
    public bool invincible = true;

    public void Die()
    {
        if (invincible)
        {
            Debug.Log("Player was hit, but invincibility is ON.");
            return;
        }

        Debug.Log("Player died.");

        // For now, just disable the player.
        // Later, replace this with game over logic.
        gameObject.SetActive(false);
    }
}