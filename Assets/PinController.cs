using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour, IPin
{
    // used to track if pin has fallen over
    [SerializeField]
    private bool isOut = false;
    [SerializeField]
    private float fallenOverDotProduct = 0.9f;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Rigidbody rb;
    private Renderer _renderer;
    private Collider _collider;

    /*
    public bool IsOut
    {
        get
        {
            return isOut;
        }
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
        Reset();
    }

    public void Reset()
    {
        rb.isKinematic = false;
        rb.MovePosition(startingPosition);
        rb.MoveRotation(startingRotation);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isOut = false;
        _renderer.enabled = true;
        _collider.enabled = true;
    }


    // Update is called once per frame
    void Update()
    {

    }
    
    public bool IsOut()
    {
        return isOut;
    }

    public void CheckIfIsOut()
    {
        Debug.Log("Checking if out..." + Vector3.Dot(transform.up, Vector3.up));
        // The Dot product of two unit vectors will never be greater than 1. If the object is lying on its side, the Dot product will be 0
        if (Vector3.Dot(transform.up, Vector3.up) < fallenOverDotProduct)
        {
            isOut = true;
            // TODO Increase score here
            Disable();
        }
    }

    private void Disable()
    {
        // Disabling is probably too much...
        // gameObject.SetActive(false);
        rb.isKinematic = true;
        _renderer.enabled = false;
        _collider.enabled = false;
    }
}
