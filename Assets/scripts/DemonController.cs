using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private List<DemonColor> lifeStack = new List<DemonColor>();
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 4f;
    [SerializeField] private float eyeSight = 40f;
    [Header("Updating")]
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
            yield return new WaitForSeconds(updateInterval + Random.value);
        }
        yield break; // for avoiding errors on invalid calling (should not appear though)
    }

    void FakeUpdate(){

    }

    bool isPlayerVisible(){ // I don't wanna write raycasts as it could get costly and I don't see it as relevant
        if(General.Distance2(transform.position, PlayerLogic.position) < eyeSight){
            return true;
        }
        return false;
    }
}
