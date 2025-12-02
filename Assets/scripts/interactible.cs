using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform player; 
    public GameObject prompt; // The child canvas / icon

    void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance)
        {
            if (!prompt.activeSelf)
                prompt.SetActive(true);
        }
        else
        {
            if (prompt.activeSelf)
                prompt.SetActive(false);
        }
    }
}
