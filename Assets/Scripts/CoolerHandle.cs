using UnityEngine;

public class CoolerHandle : MonoBehaviour
{
    public float interactDistance = 2f;
    private bool isAttached = false;
    private ConfigurableJoint joint;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // grab
        if (!isAttached && distance < interactDistance && Input.GetMouseButtonDown(1))
        {
            Attach();
        }

        // release
        if (isAttached && Input.GetMouseButtonUp(1))
        {
            Release();
        }
    }

    void Attach()
    {
        isAttached = true;

        Rigidbody coolerRB = transform.parent.GetComponent<Rigidbody>();
        coolerRB.useGravity = true;
        coolerRB.isKinematic = false;

        joint = coolerRB.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = player.GetComponent<Rigidbody>();

        // Distance of drag
        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 0.5f;
        joint.linearLimit = limit;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        // --- rotation stability ---
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit rotLimit = new SoftJointLimit();
        rotLimit.limit = 10f; // small tilt allowed – change to 0 for no tilt
        joint.lowAngularXLimit = rotLimit;
        joint.highAngularXLimit = rotLimit;
        joint.angularYLimit = rotLimit;
        joint.angularZLimit = rotLimit;
    }


    void Release()
    {
        isAttached = false;
        Destroy(joint);
    }
}
