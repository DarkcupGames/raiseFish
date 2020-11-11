using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnlineData
{
    public string id;
    public Vector3 desirePos;
    public string state = "not created";
    public string objectname = "player";
    public float speed = 4f;
}
