using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : MonoBehaviour
{
    public OnlineData data;

    private void Update()
    {
        if (data.id == ServerSystem.playerid)
            return;

        if (data.state == "stand")
            return;

        Vector3 pos = data.desirePos - transform.position;
        transform.position += pos.normalized * data.speed * Time.deltaTime;
    }
}
