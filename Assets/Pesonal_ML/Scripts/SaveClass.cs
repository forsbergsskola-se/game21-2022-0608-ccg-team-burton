using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SaveClass
{
    [JsonProperty("posX")] 
    public float PositionX { get; set; }
    
    [JsonProperty("posY")] 
    public float PositionY { get; set; }

}
