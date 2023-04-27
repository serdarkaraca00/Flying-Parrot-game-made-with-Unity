using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class MainMenuWindow : MonoBehaviour
{
    public static int difficulty = 0;

    
    private void Awake(){

        transform.Find("difficultyWindow").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//ortaladık
        transform.Find("difficultyWindow").gameObject.SetActive(false);//başlangıçta gözükmesin

        transform.Find("difficultyWindow").Find("BackButton").GetComponent<Button_UI>().ClickFunc = () =>{transform.Find("difficultyWindow").gameObject.SetActive(false);};//back e basarsam görünmez olsun
        transform.Find("difficultyWindow").Find("BackButton").GetComponent<Button_UI>().AddButtonSounds();

        //transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => {Loader.Load(Loader.Scene.GameScene); };

        transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => {transform.Find("difficultyWindow").gameObject.SetActive(true);} ;
        //burda sesi başta ekleyemedik çünkü gameAssets MainMenu sahnesinde yok, yani sesi çal dediğimde gameAssets e ulaşamadığı için hata verdi. ulaşablsin diye GameAssets i prefab yaptık ve MainMenu sahnesine de attık.
        transform.Find("playBtn").GetComponent<Button_UI>().AddButtonSounds();//extension metodu sayesinde olan fonksiyona yeni özellik ekledik

        transform.Find("difficultyWindow").Find("easyBtn").GetComponent<Button_UI>().ClickFunc = () =>{easySelected();};
        transform.Find("difficultyWindow").Find("easyBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.Find("difficultyWindow").Find("mediumBtn").GetComponent<Button_UI>().ClickFunc = () =>{mediumSelected();};
        transform.Find("difficultyWindow").Find("mediumBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.Find("difficultyWindow").Find("hardBtn").GetComponent<Button_UI>().ClickFunc = () =>{hardSelected();};
        transform.Find("difficultyWindow").Find("hardBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.Find("difficultyWindow").Find("extremeBtn").GetComponent<Button_UI>().ClickFunc = () =>{extremeSelected();};
        transform.Find("difficultyWindow").Find("extremeBtn").GetComponent<Button_UI>().AddButtonSounds();


        transform.Find("quitBtn").GetComponent<Button_UI>().ClickFunc = () => { Application.Quit(); };
        transform.Find("quitBtn").GetComponent<Button_UI>().AddButtonSounds();
    }

    private void easySelected(){
        Loader.Load(Loader.Scene.GameScene);
        difficulty=1;
    }

    private void mediumSelected(){
        Loader.Load(Loader.Scene.GameScene);
        difficulty=2;
    }

    private void hardSelected(){
        Loader.Load(Loader.Scene.GameScene);
        difficulty=3;
    }

    private void extremeSelected(){
        Loader.Load(Loader.Scene.GameScene);
        difficulty=4;
    }
}
