using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highscoreText;

    private void Awake(){
        scoreText = transform.Find("scoreText").GetComponent<Text>();//scoreText e TExt kısmı senindir dedik
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();
    }


    private void Start(){
        highscoreText.text = "HIGHSCORE: " + Score.GetHighscore().ToString();//oyun başladığında üstte highScore u yaz dedik
        GameHandler.GetInstance().OnDied += ScoreWindow_OnDied;
        GameHandler.GetInstance().OnStartedPlaying += ScoreWindow_OnStartedPlaying;
        Hide();//başlangıçta skor filan gözükmesin
    }

    private void ScoreWindow_OnDied(object sender, System.EventArgs e){//ölünce bu ekran görünmez olsun
        Hide();
    }

    private void ScoreWindow_OnStartedPlaying(object sender, System.EventArgs e){//oynama başlayınca gözüksün
        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    

    private void Update(){
        scoreText.text = Level.GetInstance().GetpipesPassedCount().ToString();//texti level den gelen bilgiler yardımıyla değiştiriyoruz
    }


    /*private void highscoreTextChange(){//zorluğa göre üstte  text değişiyor
        if(MainMenuWindow.difficulty==1){
            highscoreText.text = "HIGHSCORE(Easy Mode): " + Score.GetHighscore().ToString();//oyun başladığında üstte highScore u yaz dedik
        }
        else if(MainMenuWindow.difficulty==2){
            highscoreText.text = "HIGHSCORE(Medium Mode): " + Score.GetHighscore().ToString();//oyun başladığında üstte highScore u yaz dedik
        }
        else if(MainMenuWindow.difficulty==3){
            highscoreText.text = "HIGHSCORE(Hard Mode): " + Score.GetHighscore().ToString();//oyun başladığında üstte highScore u yaz dedik
        }
        else if(MainMenuWindow.difficulty==4){
            highscoreText.text = "HIGHSCORE(Extreme Mode): " + Score.GetHighscore().ToString();//oyun başladığında üstte highScore u yaz dedik
        }
    }*/
}
