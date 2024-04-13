using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayRegister : MonoBehaviour
{
    public static GameplayRegister Instance { get; private set; }
    public bool isEnded = false;
    
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
        isEnded = true;
    }
}
