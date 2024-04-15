using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private List<DemonColor> lifeStack = new List<DemonColor>(); // queue would be better, but I need to be fast now
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 4f;
    [SerializeField] private float biteRange = 0.75f;
    [SerializeField] private float eyeSight = 40f;
    [SerializeField] private float looseSight = 60f;
    [SerializeField] private float spellShockTime = 1f;
    private float runningSpellShockTime = 0.25f;
    [SerializeField] private GameObject destroyObj;
    [Header("Updating")]
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private float screamInterval = 5f;
    [SerializeField] private AudioSource screamAudio;
    [SerializeField] private AudioSource biteAudio;

    [Header("colors")]
    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    [SerializeField] private Material green;
    [SerializeField] private Material white;
    [SerializeField] private GameObject holder;

    private bool updateOn = false;

    private bool isAttacking = false;

    void Start()
    {
        colorUpdate();
        FakeUpdateCaller();
        InvokeRepeating("scream", 0.1f, screamInterval);
    }

    void scream(){
        screamAudio.Play();
    }

    void colorUpdate(){
        float id;
        if(lifeStack.Count > GameplayRegister.Instance.demonScales.Count){
            id = GameplayRegister.Instance.demonScales[GameplayRegister.Instance.demonScales.Count-1];
        }
        else{
            id = GameplayRegister.Instance.demonScales[lifeStack.Count-1];
        }
        transform.localScale = Vector3.one * id;
        switch(lifeStack[0]){
            case DemonColor.Red:
                holder.GetComponent<Renderer>().material = red;
                break;
            case DemonColor.Blue:
                holder.GetComponent<Renderer>().material = blue;
                break;
            case DemonColor.Green:
                holder.GetComponent<Renderer>().material = green;
                break;
            case DemonColor.White:
                holder.GetComponent<Renderer>().material = white;
                break;

        }
    }

    


    void Update()
    {
        if(GameplayRegister.Instance.isEnded){updateOn = false;return;}

        //shock
        runningSpellShockTime = Mathf.Max(runningSpellShockTime - Time.deltaTime, -1f); // avoid overflow
        if(runningSpellShockTime > 0f){return;}
        
        // go towards player if attacking
        if(isAttacking){
            float dist = General.Distance3(transform.position, PlayerLogic.position);
            //chase player
            if(dist > 0.5f){
                transform.position = Vector3.Lerp(transform.position, PlayerLogic.position, (1f/dist)*speed*Time.deltaTime);
            }

            transform.rotation = Quaternion.LookRotation(PlayerLogic.position-transform.position, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);


            // transform.position = Vector3.Lerp(transform.position, PlayerLogic.position, speed*Time.deltaTime); // (this is cheaper, but not linear --> stress test will decide)
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
        if(General.Distance3(transform.position, PlayerLogic.position) < biteRange){
            PlayerLogic.health -= damage;
            if(!biteAudio.isPlaying)
                biteAudio.Play();
        }
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

    public void getSpell(DemonColor color){
        if(lifeStack.Count == 0){return;}
        if(color.Equals(lifeStack[0])){
            lifeStack.RemoveAt(0);
            runningSpellShockTime = spellShockTime;

            if(lifeStack.Count == 0){
                die();
            }
            else{
                colorUpdate();
            }
        }
    }

    void die(){
        DemonGenerator.count--;
        Instantiate(destroyObj, transform.position, transform.rotation);
        Destroy(gameObject, spellShockTime);
    }

}
