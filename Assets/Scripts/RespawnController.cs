using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public GameObject target;
    void Start()
    {
        InvokeRepeating("SpawnObject",0,2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SpawnObject()
    {
        Instantiate(target, new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
    }
}
