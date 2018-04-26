using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class DataReader {
    public static T JsonToObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
