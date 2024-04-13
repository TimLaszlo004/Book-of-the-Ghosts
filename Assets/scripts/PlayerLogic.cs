using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerLogic : MonoBehaviour
{
    public static Vector3 position;
    public static float health;
    [SerializeField] private float updateInterval = 0.2f;
    [SerializeField] private GameObject armature;
    [SerializeField] private float initHealth = 100f;
    [SerializeField] private float attackRange = 5f;
    [Header("Spell Management")]
    [SerializeField] private float reloadTime = 0.15f;
    private float runningReload = 0f;

    private bool updateOn = false;

    [SerializeField] private StarterAssetsInputs inputs;

    void Start()
    {
        health = initHealth;
        FakeUpdateCaller();
    }


    void Update()
    {
        // mostly input handling
        // Debug.Log(inputs.spellId);

        if(runningReload > 0f){
            runningReload -= Time.deltaTime;
        }
        else{
            runningReload = 0f;
            switch(inputs.spellId){
                case 1:
                    attack(DemonColor.Red);
                    runningReload = reloadTime;
                    break;
                case 2:
                    attack(DemonColor.Blue);
                    runningReload = reloadTime;
                    break;
                case 3:
                    attack(DemonColor.Green);
                    runningReload = reloadTime;
                    break;
                case 4:
                    attack(DemonColor.White);
                    runningReload = reloadTime;
                    break;
            }
        }
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

    void attack(DemonColor color){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        // Debug.Log(hitColliders.Length);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.tag);
            if(hitCollider.CompareTag("enemy")){
                hitCollider.GetComponent<DemonController>().getSpell(color);
            }
        }
    }


}
