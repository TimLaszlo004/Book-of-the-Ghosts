using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGenerator : MonoBehaviour
{

    [SerializeField] private GameObject ghost;
    [SerializeField] private float range = 100f;
    [SerializeField] private float rate = 0.2f;
    [SerializeField] private int maxPopulation = 20;
    public static int count = 0;

    void Start(){
        Spawn(rate);
    }


    public void Spawn(float r){
        InvokeRepeating("one", r, r);
    }

    void one(){
        if(count < maxPopulation){
            Vector3 pos = PlayerLogic.position + new Vector3((Random.value-0.5f)*range, (Random.value-0.5f)*range, (Random.value-0.5f)*range);
            Instantiate(ghost, pos, Quaternion.identity);
            count++;
        }
    }
}
