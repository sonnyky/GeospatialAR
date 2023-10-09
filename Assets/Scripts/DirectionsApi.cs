using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DirectionsApi : IApiRequest
{
    private readonly string _baseURL = "https://maps.googleapis.com/maps/api/directions/json?";

    public IEnumerator GetRequestAsync(string origin, string destination, Action<string> callback)
    {
        string fullUri = _baseURL + "origin=" + "Coaska Bayside Stores" + "&destination=" + "Yokosuka Chuo station" + "&mode=walking&key=AIzaSyAmU69Xc-N2Ht8ztYS8D2e9CGt3GQpNFEE";
        Debug.Log(fullUri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = fullUri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    callback?.Invoke(null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    callback?.Invoke(null);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback?.Invoke(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
