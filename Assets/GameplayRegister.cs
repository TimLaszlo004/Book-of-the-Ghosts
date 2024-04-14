using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayRegister : MonoBehaviour
{
    public static GameplayRegister Instance { get; private set; }
    public bool isEnded = false;
    public bool isTargetReached = false;
    public bool isSuccess = false;

    public Transform startPoint;
    public Transform targetPoint;
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
        } 
        else 
        { 
            Instance = this;
        }
    }

    public void playerDied(){
        // isEnded = true;
    }

    public void restart(){
        isEnded = false;
        isTargetReached = false;
        isSuccess = false;
    }

    public void win(){
        
    }
}
