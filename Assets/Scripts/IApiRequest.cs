using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public interface IApiRequest
{
    IEnumerator GetRequestAsync(string origin, string destination, Action<string> callback);
    // Add other common API methods like PostRequestAsync, etc.
}