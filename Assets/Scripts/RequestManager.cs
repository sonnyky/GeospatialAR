using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    // Directions API
    DirectionsApi directionsApi;

    public void RequestDirections(string origin, string destination, Action<string> callback)
    {
        if (directionsApi == null)
        {
            directionsApi = new DirectionsApi();
        }
        StartCoroutine(directionsApi.GetRequestAsync(origin, destination, callback));
    }
}
