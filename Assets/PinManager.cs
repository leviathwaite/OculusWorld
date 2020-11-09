using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO transform for effect location
public class PinManager: MonoBehaviour
{
    public GameObject spareEffectPrefab;
    public GameObject strikeEffectPrefab;
    public GameObject resetEffectPrefab;

    public GameObject pinPrefab;
    public Transform[] spawnPoints;

    [SerializeField]
    private bool isDebug = true;
    [SerializeField]
    private int numberOfRolls = 0;
    [SerializeField]
    private float pinCheckDelay = 1;
    [SerializeField]
    private bool checkPins = false;

    private float pinCheckTimer = 0;
    private List<IPin> pins;

    private GameObject spareEffect;
    private GameObject strikeEffect;
    private GameObject resetEffect;

    // Start is called before the first frame update
    void Start()
    {
        CreatePins();
        CreateEffects();
    }

    private void CreatePins()
    {
        pins = new List<IPin>();

        Debug.Log("Create " + spawnPoints.Length + "pins");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject temp = Instantiate(pinPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            temp.transform.parent = transform;
            pins.Add(temp.GetComponent<IPin>());
        }
    }

    private void CreateEffects()
    {
        spareEffect = Instantiate(spareEffectPrefab, transform.position, transform.rotation);
        spareEffect.transform.parent = transform;
        spareEffect.AddComponent<LimitedLifetime>();
        spareEffect.SetActive(false);

        strikeEffect = Instantiate(strikeEffectPrefab, transform.position, transform.rotation);
        strikeEffect.transform.parent = transform;
        strikeEffect.AddComponent<LimitedLifetime>();
        strikeEffect.SetActive(false);

        resetEffect = Instantiate(resetEffectPrefab, transform.position, transform.rotation);
        resetEffect.transform.parent = transform;
        resetEffect.AddComponent<LimitedLifetime>();
        resetEffect.SetActive(false);
    }

    private void ResetPins()
    {
        // reset numberOfRolls
        numberOfRolls = 0;

        foreach (IPin pin in pins)
        {
            pin.Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO remove
        if(Input.GetKeyUp(KeyCode.P))
        {
            ResetPins();
        }

        if(checkPins)
        {
            TickTimer();
        }
    }

    private void TickTimer()
    {
        pinCheckTimer -= Time.deltaTime;
        if(pinCheckTimer < float.Epsilon)
        {
            pinCheckTimer = 0;
            checkPins = false;
            CheckIfAllPinsAreDown();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    // TODO add timer/delay to check if all pins down
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "Leaving");
        // Ball leaves area
        if (other.CompareTag("Ball"))
        {
            // Start checkTimer/delay
            pinCheckTimer = pinCheckDelay;
            checkPins = true;
        }
    }

    private void CheckIfAllPinsAreDown()
    {
        // check if pins are still standing
        bool resetPins = true;

        Debug.Log("Ball tag");
        // Check for fallen pins
        foreach (IPin pin in pins)
        {
            pin.CheckIfIsOut();
            if (!pin.IsOut())
            {
                resetPins = false;
            }
        }

        // increment numberOfRolls
        numberOfRolls++;
        // if all the pins aren't knocked down and 2nd roll
        if(!resetPins && numberOfRolls >= 2)
        {
            numberOfRolls = 0;
            resetPins = true;
            PlayResetEffect();
        }
        
        if(resetPins)
        {
            if(numberOfRolls == 1)
            {
                // Strike
                Debug.Log("Strike");
                PlayStrikeEffect();
            }
            else if(numberOfRolls == 2)
            {
                // Spare
                Debug.Log("Spare");
                PlaySpareEffect();
            }
            else
            {
                Debug.Log("Huh? numberOfRolls = " + numberOfRolls);
            }

            ResetPins();
        }
    }

    private void PlaySpareEffect()
    {
        spareEffect.SetActive(true);
    }

    private void PlayStrikeEffect()
    {
        strikeEffect.SetActive(true);
    }

    private void PlayResetEffect()
    {
        resetEffect.SetActive(true);
    }
}
