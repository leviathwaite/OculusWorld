using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedLifetime : MonoBehaviour
{
    public enum DisableType
    {
        DISABLE, DESTROY
    }

    [SerializeField]
    private DisableType currentDisableType = DisableType.DISABLE;
    [SerializeField]
    private float lifeTime = 2;

    private float lifeTimer;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (currentDisableType == DisableType.DESTROY)
        {
            Destroy(gameObject, lifeTime);
        }
        else
        {
            lifeTimer = lifeTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer < 0)
        {
            lifeTimer = 0;
            gameObject.SetActive(false);
        }
    }
}
