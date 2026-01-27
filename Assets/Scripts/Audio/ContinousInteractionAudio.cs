using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ContinuousInteractionAudio : MonoBehaviour
{
    [SerializeField] private AudioClip loopClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }

    public void StartLoop()
    {
        if (loopClip == null)
        {
            Debug.LogWarning("ContinuousInteractionAudio: No AudioClip assigned.");
            return;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.clip = loopClip;
            audioSource.Play();
        }
    }

    public void StopLoop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnDisable()
    {
        StopLoop();
    }
}
