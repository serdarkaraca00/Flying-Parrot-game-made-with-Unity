using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;//buton sesi için

public static class SoundManager{//static yaptık ki yanlışlıkla nesnesini oluşturmayalım
    

    public enum Sound{//bu enumlarlar editörde direk gözüktü ordan tıkladık. 
        BirdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }


    public static void PlaySound(Sound sound){
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));//AudioSource tipinde obje oluştu
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();//burda sanırım ses özellği ekledik
        audioSource.PlayOneShot(GetAudioClip(sound));//return edilen sesi oynat dedik
    }


    private static AudioClip GetAudioClip(Sound sound){//tüm sound lar geziliyo ve ismi eşleşen sesi return ettik
        foreach (GameAssets.SoundAudioClip SoundAudioClip in GameAssets.GetInstance().soundAudioClipArray){
            if(SoundAudioClip.sound == sound){
                return SoundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + " not found!");//eşleşen isim yoksa hata verir
        return null;
    }




    public static void AddButtonSounds(this Button_UI buttonUI){//bu bir extension method(this den anladık). Extension metodu sayesinde olan fonksiyona(yani Button_UI a) yeni özellik ekledik
        buttonUI.MouseOverOnceFunc += () => PlaySound(Sound.ButtonOver);//üstüne gelince bunu çal
        buttonUI.ClickFunc += () => PlaySound(Sound.ButtonClick);//tıklayınca bunu
    }
    

}
