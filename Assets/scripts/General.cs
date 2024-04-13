using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
// implements a revolutionary method for distance measuring
{
    public static float Distance2(Vector3 a, Vector3 b){ 
        float x = Mathf.Abs(a.x-b.x);
        float y = Mathf.Abs(a.z-b.z);
        float max = x;
        if(y > x){
            max = y;
        }
        return 0.5f * (y + x + max);
    }

    public static float Distance3(Vector3 a, Vector3 b){ 
        float x = Mathf.Abs(a.x-b.x);
        float y = Mathf.Abs(a.y-b.y);
        float z = Mathf.Abs(a.z-b.z);
        float min = x;
        float max = x;
        if(y > max){
            max = y;
        }
        else{
            min = y;
        }
        if(z > max){
            max = z;
        }
        if(z < min){
            min = z;
        }
        return 0.5f * (y + x + z) + 0.5f*max - 0.25f*min; // gets a little bit wrong but needless to write the actual formula (also that is not for free :))
    }
}
