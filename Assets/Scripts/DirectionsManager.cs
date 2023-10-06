using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectionsManager : MonoBehaviour
{
    [SerializeField] TMP_InputField origin;
    [SerializeField] TMP_InputField destination;

    RequestManager requestManager;

    // Start is called before the first frame update
    void Start()
    {
        requestManager = new RequestManager().Instance;
        
    }

   public void GetDirectionsToDestination()
    {
        Debug.Log("getting directions");
        requestManager.RequestDirections(origin.text, destination.text);
    }
}
