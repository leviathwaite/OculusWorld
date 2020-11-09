using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://forum.unity.com/threads/rotate-object-to-colliders-normal-on-collision.10026/
public class BallEffectController : MonoBehaviour
{
    public GameObject hitPinEffectPrefab;
    public GameObject hitFloorEffectPrefab;

    private GameObject hitPinEffect;
    private GameObject hitFloorEffect;

    private string pinTag = "Pin";
    private string floorTag = "Ground";
  

    // Start is called before the first frame update
    void Start()
    {
       CreateEffects(); 
    }

    private void CreateEffects()
    {
        hitPinEffect = Instantiate(hitPinEffectPrefab, transform.position, transform.rotation);
        hitPinEffect.transform.parent = transform;
        hitPinEffect.AddComponent<LimitedLifetime>();
        hitPinEffect.SetActive(false);

        hitFloorEffect = Instantiate(hitFloorEffectPrefab, transform.position, transform.rotation);
        hitFloorEffect.transform.parent = transform;
        hitFloorEffect.AddComponent<LimitedLifetime>();
        hitFloorEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(pinTag))
        {
            hitPinEffect.transform.position = collision.contacts[0].point;

            var rot = Quaternion.FromToRotation(transform.up, collision.contacts[0].normal);
            hitPinEffect.transform.rotation *= rot;

            hitPinEffect.SetActive(true);
        }

        if(collision.collider.CompareTag(floorTag))
        {
            hitFloorEffect.transform.position = collision.contacts[0].point;

            var rot = Quaternion.FromToRotation(transform.up, collision.contacts[0].normal);
            hitFloorEffect.transform.rotation *= rot;

            hitFloorEffect.SetActive(true);
        }
    }
}
