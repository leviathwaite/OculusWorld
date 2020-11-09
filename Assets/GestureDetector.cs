using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Valem - Hand Tracking Gesture Detection - Unity Oculus Quest Tutorial
// https://youtu.be/lBzwUKQ3tbw


// https://stackoverflow.com/questions/12673440/how-can-i-check-whether-a-struct-has-been-instantiated/38402354
// public struct Nullable<T> where T : struct
[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureDetector : MonoBehaviour
{
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
   
    // Start is called before the first frame update
    void Start()
    {
        // fingerBones = new List<OVRBone>(skeleton.Bones);
        StartCoroutine(GetFingerBones());
        previousGesture = new Gesture();
    }

    IEnumerator GetFingerBones()
    {
        do
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            yield return null;
        } while (fingerBones.Count <= 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        if (gestures.Count > 0)
        { 
            Gesture currentGesture = Recognize();
            bool hasRecognized = !currentGesture.Equals(new Gesture());
            // Check if new Gesture

        
            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                // New Gesture
                Debug.Log("New Geseture Found: " + currentGesture.name);
                previousGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
        }
    }

    private void Save()
    {
        if(fingerBones.Count > 0)
        { 
            Gesture g = new Gesture();
            g.name = "New Gesture";
            List<Vector3> data = new List<Vector3>();
            foreach (var bone in fingerBones)
            {
                // check position distance from hand root
                data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
            }

            g.fingerDatas = data;
            gestures.Add(g);
        }
    }

    // TODO improve performance by checking thumb first. Just like Tactile in ASL
    private Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if(distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }
        return currentGesture;
    }
}
