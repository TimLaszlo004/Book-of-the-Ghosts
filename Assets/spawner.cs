using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    void Start()
    {
        spawn();
    }

    void spawn(){
        for(int i = 0; i < transform.childCount; i++){
            Vector3 pos = transform.GetChild(i).position;
            RaycastHit hit;
            if(Physics.Raycast(pos, Vector3.down, out hit, 5000f)){
                transform.GetChild(i).position = new Vector3(pos.x, hit.point.y, pos.z);
            }
        }
    }

    
}
