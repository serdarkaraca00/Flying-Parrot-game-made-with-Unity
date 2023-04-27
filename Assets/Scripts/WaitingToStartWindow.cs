using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//bu ikisini ekransa skin seçmek için import ettik
using UnityEditor;

public class WaitingToStartWindow : MonoBehaviour
{
    



    [SerializeField]
    private GameObject[] characters;



    private static int selectedSkin =0;//ekranda gözüken skin

    public void NextOption(){//next e basınca bu fonskiyon çalışcak ve sonraki skin ekrana gelicek
        HideAllSkins();
        selectedSkin = selectedSkin +1;
        if(selectedSkin == characters.Length){
            selectedSkin = 0;
        }
        characters[selectedSkin].SetActive(true);//yeni skini göster
        //transform.Find("NextButton").GetComponent<Button_UI>().AddButtonSounds();
    }

    public void BackOption(){
        HideAllSkins();
        selectedSkin = selectedSkin -1;
        if(selectedSkin < 0){
            selectedSkin = characters.Length -1;//ilk skinde iken back e basarsam en sondakini getir dedik
        }
        characters[selectedSkin].SetActive(true);
    }


    public static int PlayPressedCounter=0;//0 ise daha play e basılmadı demek
    public void PlayPressed(){
        if(PlayPressedCounter==0){
            PlayPressedCounter=1;//1 ise oyun başlasın demek
        }
    }





    private void Start(){
        GameHandler.GetInstance().OnStartedPlaying += WaitingToStartWindow_OnStartedPlaying;//bird de OnStartedPlaying çalışınca WaitingToStartWindow_OnStartedPlaying içindeki şeyler çalışcak
    }


    private void WaitingToStartWindow_OnStartedPlaying(object sender, System.EventArgs e){//bird classından bu fonksiyonu çalıştır talimatı geldi ve bu window görünmez oldu
        Hide();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }



    private void HideAllSkins(){//bazen ekranda ilk kuş kalabilyodu. onu önlemek için butonlara basıldığında tüm skinleri görünmez yaptık ilk olarak
        for(int i = 0; i < characters.Length; i++){//başta hepsini gizle dedik
            characters[i].SetActive(false);
        }
    }

}
