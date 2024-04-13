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
    [SerializeField] private float looseSight = 60f;
    [Header("Updating")]
    [SerializeField] private float updateInterval = 0.1f;
    private bool updateOn = false;

    private bool isAttacking = false;

    void Start()
    {
        FakeUpdateCaller();
    }


    void Update()
    {
        // go towards player if attacking
        if(isAttacking){
            //chase player
            transform.position = Vector3.Lerp(transform.position, PlayerLogic.position, speed*Time.deltaTime); // TODO ???
        }
    }

    void FakeUpdateCaller(){
        StartCoroutine(FakeUpdateCycle());
    }

    IEnumerator FakeUpdateCycle(){
        updateOn = true;
        while(updateOn){
            FakeUpdate();
            yield return new WaitForSeconds(updateInterval + 0.002f*General.Distance2(transform.position, PlayerLogic.position)); // for optimizing
        }
        yield break; // for avoiding errors on invalid calling (should not appear though)
    }

    void FakeUpdate(){
        attackStatusUpdate();
    }

    void attackStatusUpdate(){
        if(isAttacking){
            if(!isPlayerVisible(true)){
                isAttacking = false;
            }
        }
        else{
            if(isPlayerVisible()){
                isAttacking = true;
            }
        }
    }

    bool isPlayerVisible(bool isSeeingNow = false){ // I don't wanna write raycasts as it could get costly and I don't see it as relevant
        float _dist = eyeSight;
        if(isSeeingNow){
            _dist = looseSight;
        }
        if(General.Distance2(transform.position, PlayerLogic.position) < _dist){
            return true;
        }
        return false;
    }
}
