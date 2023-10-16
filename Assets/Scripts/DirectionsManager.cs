using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DirectionsManager : MonoBehaviour
{
    [SerializeField] TMP_InputField origin;
    [SerializeField] TMP_InputField destination;

    [SerializeField] RequestManager requestManager;

    List<StartLocation> allLocations;
    DirectionsDataRoot directionsDataRoot;
    List<GameObject> spawnedMarkerObjects;

    [SerializeField] TMP_Text debugLongLat;

    [SerializeField] GeospatialController geospatialController;

    // Start is called before the first frame update
    void Start()
    {
        allLocations = new List<StartLocation>();
        spawnedMarkerObjects = new List<GameObject>();
    }

   public void GetDirectionsToDestination()
    {
        Debug.Log("getting directions");
        allLocations.Clear();
        string longLatCurrent = geospatialController.currentPose.Latitude.ToString() + "," + geospatialController.currentPose.Longitude.ToString();
        debugLongLat.text = geospatialController.currentPose.Latitude.ToString() + ", " + geospatialController.currentPose.Longitude.ToString();
        requestManager.RequestDirections(longLatCurrent, destination.text, Process);
    }

    public void CheckRaycast()
    {
      
       
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
                for(int j=0; j < leg.steps.Count; j++)
                {
                    //allLocations.Add(step.start_location);
                    int numberOfPoints = 10;

                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        float t = (i + 1) / (float)(numberOfPoints + 1);

                        allLocations.Add(Lerp(leg.steps[j].start_location, leg.steps[j].end_location, t));
                    }
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
       foreach (StartLocation loc in allLocations)
        {
            geospatialController.PlaceStreetMarkerAnchor(loc.lat, loc.lng);
        }
    }

    public StartLocation Lerp(StartLocation start, EndLocation end, float t)
    {
        StartLocation midpoint = new StartLocation();
        midpoint.lat = LerpDouble(start.lat, end.lat, t);
        midpoint.lng = LerpDouble(start.lng, end.lng, t);
        return midpoint;
    }

    public double LerpDouble(double a, double b, double t)
    {
        return a + t * (b - a);
    }
}
