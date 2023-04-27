using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highscoreText;

    private void Awake(){
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();


        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () =>{ Loader.Load(Loader.Scene.GameScene); };//retry butonuna basarsan oyun sahnesi baştan yüklensin
        transform.Find("retryBtn").GetComponent<Button_UI>().AddButtonSounds();//extension metodu sayesinde olan fonksiyona yeni özellik ekledik
        
        transform.Find("mainMenuBtn").GetComponent<Button_UI>().ClickFunc = () =>{ Loader.Load(Loader.Scene.MainMenu); };//main menu butonuna basarsam o scene açılsın
        transform.Find("mainMenuBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//gaemover ekranını ortaladık
    }


    private void Start(){
        GameHandler.GetInstance().OnDied += Bird_OnDied;//bird öldüğü an bu fonksiyon çalışsın
        Hide();//başlangıçta gameover ekranı gözükmesin
    }

    
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){//space e  basarsamda başlasın dedik
            Loader.Load(Loader.Scene.GameScene);
        }
    }


    private void Bird_OnDied(object sender, System.EventArgs e){//ölünce bird classı tarafından bu fonksiyon çağırılcak
        scoreText.text = Level.GetInstance().GetpipesPassedCount().ToString();//texti level den gelen bilgiler yardımıyla değiştiriyoruz

        if(Level.GetInstance().GetpipesPassedCount() >= Score.GetHighscore()){
            //highscoreText.text = "NEW HIGHSCORE";//yeni high score ise bu yazsın
            NewhighscoreText();
        } else {
            //highscoreText.text = "HIGHSCORE: " + Score.GetHighscore();//değilse bu
            OldhighscoreText();
        }

        Show();//ölünce gameover ekranı gelsin dedik
    }


    private void Hide(){
        gameObject.SetActive(false);//gameObject dediğimiz şey aslında GameOverWindow altındaki her şey
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void NewhighscoreText(){
        if(MainMenuWindow.difficulty==1){
            highscoreText.text = "NEW HIGHSCORE (EASY MODE)";
        }
        else if(MainMenuWindow.difficulty==2){
            highscoreText.text = "NEW HIGHSCORE (MEDIUM MODE)";
        }
        else if(MainMenuWindow.difficulty==3){
            highscoreText.text = "NEW HIGHSCORE (HARD MODE)";
        }
        else if(MainMenuWindow.difficulty==4){
            highscoreText.text = "NEW HIGHSCORE (EXTREME MODE)";
        }
    }

    private void OldhighscoreText(){
        if(MainMenuWindow.difficulty==1){
            highscoreText.text = "HIGHSCORE IS " + Score.GetHighscore() + " (EASY MODE)";
        }
        else if(MainMenuWindow.difficulty==2){
            highscoreText.text = "HIGHSCORE IS " + Score.GetHighscore() + " (MEDIUM MODE)";
        }
        else if(MainMenuWindow.difficulty==3){
            highscoreText.text = "HIGHSCORE IS " + Score.GetHighscore() + " (HARD MODE)";
        }
        else if(MainMenuWindow.difficulty==4){
            highscoreText.text = "HIGHSCORE IS " + Score.GetHighscore() + " (EXTREME MODE)";
        }
    }
}
