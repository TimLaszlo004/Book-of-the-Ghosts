using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    [SerializeField] private AudioSource lobbyMusic;

    void OnDisable(){
        lobbyMusic.Stop();
    }
    
    void OnEnable(){
        lobbyMusic.Play();
    }
}
