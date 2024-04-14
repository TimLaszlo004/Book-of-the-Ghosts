using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    public float time = 1f;
    public float scale = 1f;

    void Awake(){
        transform.localScale = Vector3.one*scale;
        Destroy(gameObject, time);
    }
}
