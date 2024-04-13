using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public static Vector3 position;
    public static float health;
    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private GameObject armature;
    [SerializeField] private float initHealth = 100f;
    [SerializeField] private float attackRange = 5f;
    private bool updateOn = false;

    void Start()
    {
        health = initHealth;
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
        position = armature.transform.position;
    }

    void attack(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
                
        foreach (var hitCollider in hitColliders)
        {

            if(hitCollider.transform.parent.CompareTag("enemy")){

                

            }
        }
    }


}
