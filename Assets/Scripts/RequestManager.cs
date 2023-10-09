using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    static RequestManager instance;

    // Directions API
    DirectionsApi directionsApi;

    public RequestManager Instance
    {
        get
        {
            if (instance == null && GameObject.FindFirstObjectByType<RequestManager>() == null)
            {
                instance = new GameObject("RequestManager").AddComponent<RequestManager>();
            }

            return instance;
        }
    }

    public void RequestDirections(string origin, string destination, Action<string> callback)
    {
        if (directionsApi == null)
        {
            directionsApi = new DirectionsApi();
        }
        StartCoroutine(directionsApi.GetRequestAsync(origin, destination, callback));
    }
}
