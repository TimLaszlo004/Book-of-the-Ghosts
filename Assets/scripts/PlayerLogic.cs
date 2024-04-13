using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public static Vector3 position;
    [SerializeField] private float updateInterval = 0.2f;
    private bool updateOn = false;

    void Start()
    {
        FakeUpdateCaller();
    }


    void Update()
    {
        
    }

    void FakeUpdateCaller(){
        StartCoroutine(FakeUpdateCycle());
    }

    IEnumerator FakeUpdateCycle(){
        updateOn = true;
        while(updateOn){
            FakeUpdate();
            yield return new WaitForSeconds(updateInterval);
        }
        yield break; // for avoiding errors on invalid calling (should not appear though)
    }

    void FakeUpdate(){
        position = transform.position;
    }


}
