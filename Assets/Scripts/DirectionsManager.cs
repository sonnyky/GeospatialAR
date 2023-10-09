using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectionsManager : MonoBehaviour
{
    [SerializeField] TMP_InputField origin;
    [SerializeField] TMP_InputField destination;

    RequestManager requestManager;

    List<StartLocation> allLocations;
    DirectionsDataRoot directionsDataRoot;

    [SerializeField] GameObject markerObject;
    List<GameObject> spawnedMarkerObjects;

    // Start is called before the first frame update
    void Start()
    {
        requestManager = new RequestManager().Instance;
        allLocations = new List<StartLocation>();
        spawnedMarkerObjects = new List<GameObject>();
    }

   public void GetDirectionsToDestination()
    {
        Debug.Log("getting directions");
        allLocations.Clear();
        requestManager.RequestDirections(origin.text, destination.text, Process);
    }

    public void Process(string result)
    {
        Debug.Log("after callback: " + result);
        directionsDataRoot = new DirectionsDataRoot();
        directionsDataRoot = JsonConvert.DeserializeObject<DirectionsDataRoot>(result);
        GetAllLocations(directionsDataRoot);
        ClearMarkerObjects();
        SpawnMarkerObjects();
    }
    public void GetAllLocations(DirectionsDataRoot root)
    {
        Debug.Log(root.routes);
        foreach (var route in root.routes)
        {
            foreach (var leg in route.legs)
            {
                foreach (var step in leg.steps)
                {
                    allLocations.Add(step.start_location);
                }
            }
        }
    }

    void ClearMarkerObjects()
    {
        for (int i = spawnedMarkerObjects.Count - 1; i >= 0; i--)
        {
           GameObject.Destroy(spawnedMarkerObjects[i]);
        }
        spawnedMarkerObjects.Clear();
    }

    void SpawnMarkerObjects()
    {
        foreach(StartLocation loc in allLocations)
        {
            // spawn marker
            GameObject marker = Instantiate(markerObject);

            ARGeospatialCreatorAnchor anchor = marker.GetComponent<ARGeospatialCreatorAnchor>();
            anchor.Longitude = loc.lng;
            anchor.Latitude = loc.lat;
            marker.transform.position = anchor.transform.position;
            marker.transform.rotation = anchor.transform.rotation;
            Vector3 newScale = anchor.transform.localScale * 200f;
            marker.transform.localScale = newScale;
        }
    }

}
