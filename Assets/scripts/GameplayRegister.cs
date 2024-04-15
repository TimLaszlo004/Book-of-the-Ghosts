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

    public Transform[] startPoints;
    public Transform startPoint;
    public Transform[] targetPoints;
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
        setStart();
        targetIndicator = GetComponent<LineRenderer>();
        setTargetFlag(targetPoint.position);
    }

    void setStart(){
        int s = (int)(startPoints.Length * Random.value);
        int e = (int)(targetPoints.Length * Random.value);
        startPoint = startPoints[s];
        targetPoint = targetPoints[e];

        for(int i = 0; i<startPoints.Length; i++){
            if(i != s){
                startPoints[i].gameObject.SetActive(false);
            }
            else{
                startPoints[i].gameObject.SetActive(true);
            }
        }

        for(int i = 0; i<targetPoints.Length; i++){
            if(i != e){
                targetPoints[i].gameObject.SetActive(false);
            }
            else{
                targetPoints[i].gameObject.SetActive(true);
            }
        }
    }

    public void setTargetFlag(Vector3 point){
        // Debug.Log("set to " + point.ToString());
        targetIndicator.SetPosition(0, new Vector3(point.x, 0f, point.z));
        targetIndicator.SetPosition(1, new Vector3(point.x, 500f, point.z));
    }

    public void playerDied(){
        // isEnded = true;
        restart();
    }

    public void restart(){
        isEnded = false;
        isTargetReached = false;
        isSuccess = false;
        setTargetFlag(targetPoint.position);
    }

    public void win(){
        isEnded = true;
        UIController.Instance.win();

    }

}
