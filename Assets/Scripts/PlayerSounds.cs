using UnityEngine;
public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] footstepClips; // Array for variety
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [Header("Settings")]
    [SerializeField] private float footstepInterval = 0.4f;
    [SerializeField] private float pitchVariation = 0.1f;
    private AudioSource audioSource;
    private float footstepTimer;
    private AudioClip lastFootstep;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Call this from your movement script every frame the player is
    /// moving on the ground.
    /// </summary>
    public void PlayFootstep()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            AudioClip clip;

            do
            {
                clip = footstepClips[Random.Range(0, footstepClips.Length)];
            }
            while (footstepClips.Length > 1 && clip == lastFootstep);

            lastFootstep = clip;

            audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(clip);

            footstepTimer = footstepInterval;

            audioSource.pitch = 1f;
        }
    }
    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }
    public void PlayLand()
    {
        audioSource.PlayOneShot(landClip);
    }

    public void PlayEvade()
    {
        audioSource.PlayOneShot(jumpClip); // Reusing jump sound for evade, can be changed to a different clip if desired
    }
}
