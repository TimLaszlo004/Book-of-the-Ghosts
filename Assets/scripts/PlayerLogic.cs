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
    [SerializeField] private float healthReloadSpeed = 0.5f;
    [SerializeField] private float attackRange = 5f;
    [Header("Spell Management")]
    [SerializeField] private float reloadTime = 0.15f;
    [SerializeField] private Transform placement;
    private float runningReload = 0f;

    private bool updateOn = false;

    [Header("prefabs")]
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject green;
    [SerializeField] private GameObject white;

    [SerializeField] private StarterAssetsInputs inputs;
    [SerializeField] private Animator anim;

    void Start()
    {
        armature.transform.position = GameplayRegister.Instance.startPoint.position;
        health = initHealth;
        FakeUpdateCaller();
    }
    void Update()
    {
        // mostly input handling
        // Debug.Log(inputs.spellId);
        if(GameplayRegister.Instance.isEnded){return;}
        if(health <= 0f){
            die();
        }
        health = Mathf.Min(health + healthReloadSpeed * Time.deltaTime, initHealth);
        UIController.Instance.setSlider(health);
        if(runningReload > 0f){
            runningReload -= Time.deltaTime;
        }
        else{
            runningReload = 0f;
            switch(inputs.spellId){
                case 0:
                    anim.SetBool("spell", false);
                    break;
                case 1:
                    attack(DemonColor.Red);
                    Instantiate(red, placement.position, placement.rotation);
                    runningReload = reloadTime;
                    break;
                case 2:
                    attack(DemonColor.Blue);
                    Instantiate(blue, placement.position, placement.rotation);
                    runningReload = reloadTime;
                    break;
                case 3:
                    attack(DemonColor.Green);
                    Instantiate(green, placement.position, placement.rotation);
                    runningReload = reloadTime;
                    break;
                case 4:
                    attack(DemonColor.White);
                    Instantiate(white, placement.position, placement.rotation);
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
        anim.SetBool("spell", true);
        Collider[] hitColliders = Physics.OverlapSphere(armature.transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("enemy")){
                hitCollider.GetComponent<DemonController>().getSpell(color);
            }
        }
    }

    void die(){
        // updateOn = false;
        GameplayRegister.Instance.playerDied();
        armature.transform.position = GameplayRegister.Instance.startPoint.position;
        health = initHealth;
    }


}
