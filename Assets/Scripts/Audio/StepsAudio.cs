using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class MovementAudio : MonoBehaviour
{
    [SerializeField] private AudioClip movementLoop;
    [SerializeField] private float minVelocityToPlay = 0.1f;

    private AudioSource audioSource;
    private NavMeshAgent agent;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }

    void Update()
    {
        bool isMoving = agent.velocity.magnitude > minVelocityToPlay;

        if (isMoving && !audioSource.isPlaying)
        {
            PlayMovementSound();
        }
        else if (!isMoving && audioSource.isPlaying)
        {
            StopMovementSound();
        }
    }

    private void PlayMovementSound()
    {
        if (movementLoop == null) return;

        audioSource.clip = movementLoop; // <-- lowercase 'clip'
        audioSource.Play();
    }

    private void StopMovementSound()
    {
        audioSource.Stop();
    }

    void OnDisable()
    {
        StopMovementSound();
    }
}
