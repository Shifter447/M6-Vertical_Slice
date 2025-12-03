using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName)
    {
        if (anim != null)
        {
            anim.Play(animationName);
        }
    }
}
