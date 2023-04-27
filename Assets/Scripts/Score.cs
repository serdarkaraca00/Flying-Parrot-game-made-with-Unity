using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score{//static yaptık


    public static void Start(){
        //ResetHighscore();
        GameHandler.GetInstance().OnDied += Bird_OnDied;//bu Score static bir class o yüzden ekstra olarakda GameHandler a Score.Start() eklememiz gerekti. o olmadan bird e ulaşamayız ve Highscore güncellenmez
    }

    private static void Bird_OnDied(object sender, System.EventArgs e){
        TrySetNewHighscore(Level.GetInstance().GetpipesPassedCount());//kuş ölünce aldığımız score highscore mu diye kontrol ettik
    }




    public static int GetHighscore(){//zorluk seviyesine göre high score döndürdük
        if(MainMenuWindow.difficulty==1){
            return PlayerPrefs.GetInt("easyhighscore");
        }
        if(MainMenuWindow.difficulty==2){
            return PlayerPrefs.GetInt("mediumhighscore");
        }
        if(MainMenuWindow.difficulty==3){
            return PlayerPrefs.GetInt("hardhighscore");
        }
        else{//difficulty = 4
            return PlayerPrefs.GetInt("extremehighscore");
        }
        //return PlayerPrefs.GetInt("highscore");
    }



    public static bool TrySetNewHighscore(int score){//aldığımız score high score dan daha büyükse highscore u güncelledik ve true döndürdük. daha büyük değilse false döndü

        /*int currentHighscore = GetHighscore();
        if(score > currentHighscore){
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        } else{
            return false;
        }*/

        int currentHighscore = GetHighscore();
        if(score > currentHighscore){
            if(MainMenuWindow.difficulty==1){
                PlayerPrefs.SetInt("easyhighscore", score);
                PlayerPrefs.Save();
                return true;
            }
            if(MainMenuWindow.difficulty==2){
                PlayerPrefs.SetInt("mediumhighscore", score);
                PlayerPrefs.Save();
                return true;
            }
            if(MainMenuWindow.difficulty==3){
                PlayerPrefs.SetInt("hardhighscore", score);
                PlayerPrefs.Save();
                return true;
            }
            else{//difficulty = 4
                PlayerPrefs.SetInt("extremehighscore", score);
                PlayerPrefs.Save();
                return true;
            }
        }else{
            return false;
        }
    }


    public static void ResetHighscore(){//high score u sıfırladık
        PlayerPrefs.SetInt("easyhighscore", 0);
        
        PlayerPrefs.SetInt("mediumhighscore", 0);
        
        PlayerPrefs.SetInt("hardhighscore", 0);
        
        PlayerPrefs.SetInt("extremehighscore", 0);
        
        PlayerPrefs.Save();
    }

    
}
