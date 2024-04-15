using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GameplayRegister : MonoBehaviour
{
    public static GameplayRegister Instance { get; private set; }
    public bool isEnded = false;
    public bool isTargetReached = false;
    public bool isSuccess = false;

    public List<float> demonScales = new List<float>();

    private LineRenderer targetIndicator;

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
        targetIndicator = GetComponent<LineRenderer>();
        setTargetFlag(targetPoint.position);
    }

    public void setTargetFlag(Vector3 point){
        // Debug.Log("set to " + point.ToString());
        targetIndicator.SetPosition(0, new Vector3(point.x, 0f, point.z));
        targetIndicator.SetPosition(1, new Vector3(point.x, 500f, point.z));
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
        isEnded = true;
        UIController.Instance.win();

    }

}
