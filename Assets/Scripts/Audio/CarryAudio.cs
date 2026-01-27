using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarryAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip carryClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // keep playing while carried
    }

    /// <summary>
    /// Call this when the object is picked up
    /// </summary>
    public void StartCarryAudio()
    {
        if (carryClip == null) return;

        if (!audioSource.isPlaying)
        {
            audioSource.clip = carryClip;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Call this when the object is dropped
    /// </summary>
    public void StopCarryAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnDisable()
    {
        StopCarryAudio();
    }
}
