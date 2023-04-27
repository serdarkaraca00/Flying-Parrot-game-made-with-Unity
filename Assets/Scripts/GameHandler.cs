using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

public class GameHandler : MonoBehaviour
{
    

    private static GameHandler instance;

    public static GameHandler GetInstance(){
        return instance;
    }


    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;


    private void Awake(){
        instance = this;
    }

    public void OnStartedPlaying_Uygula(){
        if(OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
    }

    public void OnDied_Uygula(){
        if(OnDied != null) OnDied(this, EventArgs.Empty);
    }

    
    private void Start()
    {    
        Score.Start();//Score static bir class o yüzden event olayını uygulayabilmek için bunu yapmak gerekliydi. bu olmadan bird e ulaşamayız ve highscore güncellenmez
    }
        

}
