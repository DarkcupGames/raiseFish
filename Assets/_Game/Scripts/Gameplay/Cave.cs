using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{
    float SPAWN_RATE = 20f;
    public GameObject go;
    float timeCount;
    float timeSpawn;

    void Start()
    {
        timeSpawn = SPAWN_RATE;
        timeCount = 0f;
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > timeSpawn)
        {
            timeSpawn += SPAWN_RATE;
            Vector3 pos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            Instantiate(go, pos, Quaternion.identity);
        }
    }
}
