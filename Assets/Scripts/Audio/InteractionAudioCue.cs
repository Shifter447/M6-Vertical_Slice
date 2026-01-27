using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractionAudioCue : MonoBehaviour
{
    [SerializeField] private AudioClip interactionClip;

    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayOnce()
    {
        if (hasPlayed)
            return;

        if (interactionClip == null)
        {
            Debug.LogWarning("InteractionAudioCue: No AudioClip assigned.");
            return;
        }

        audioSource.PlayOneShot(interactionClip);
        hasPlayed = true;
    }
}
