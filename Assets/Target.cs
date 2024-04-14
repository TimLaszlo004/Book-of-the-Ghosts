using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Tooltip("If isGate then it is at the gate, else it is the target with the key")]
    [SerializeField] private bool isGate = true;

    void OnTriggerEnter(Collider obj)
    {
        if(obj.CompareTag("Player")){
            if(isGate){
                if(GameplayRegister.Instance.isTargetReached){
                    GameplayRegister.Instance.isSuccess = true;
                    GameplayRegister.Instance.win();
                }
                else{
                    Debug.Log("First get the key");
                }
            }
            else{
                GameplayRegister.Instance.isTargetReached = true;
                dissolve();
            }
        }
    }

    void dissolve(){
        Destroy(gameObject);
    }
}
