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

    [SerializeField] GameObject markerObject;
    List<GameObject> spawnedMarkerObjects;

    [SerializeField] ARAnchorManager anchorManager;

    [SerializeField] TMP_Text debug;

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
        requestManager.RequestDirections(origin.text, destination.text, Process);
    }

    private void Update()
    {
        foreach(GameObject go in spawnedMarkerObjects)
        {
            Vector3 directionToCamera = Camera.main.transform.position - go.transform.position;
            float rayLength = directionToCamera.magnitude;
            Ray ray = new Ray(go.transform.position, directionToCamera.normalized);
            if (!geospatialController.ObjectIsObstructedByGeometry(ray))
            {
                go.SetActive(true);
            }
        }
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
            ARGeospatialAnchor anchor = null;

            anchor = anchorManager.AddAnchor(
                    loc.lat, loc.lng, 39.0, Quaternion.identity);
            anchor.gameObject.SetActive(true);

            if (anchor != null )
            {
                // spawn marker
                GameObject marker = Instantiate(markerObject, anchor.transform);
                marker.transform.parent = anchor.gameObject.transform;
                spawnedMarkerObjects.Add(marker);
                marker.SetActive(false);
                debug.text = "spawned objects: " + spawnedMarkerObjects.Count.ToString();
            }
            else
            {
                debug.text = "anchor is null";
            }
        }
    }

}
