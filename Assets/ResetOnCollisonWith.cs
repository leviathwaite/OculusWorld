using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnCollisonWith : MonoBehaviour
{
    [SerializeField]
    private string _tag;
    [SerializeField]
    private float delayTime = 1;
    [SerializeField]
    private bool collided = false;

    private float delayTimer;
    private Vector3 startingPos;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(collided)
        TickTimer();
    }

    private void TickTimer()
    {
        delayTimer -= Time.deltaTime;
        if(delayTimer < 0)
        {
            delayTimer = 0;
            // transform.position = startingPos;
            rb.MovePosition(startingPos);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            collided = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit : " + collision.collider.tag);
        if(collision.collider.CompareTag(_tag))
        {
            collided = true;
            Debug.Log("Hit floor");
            delayTimer = delayTime;
        }
    }
}
