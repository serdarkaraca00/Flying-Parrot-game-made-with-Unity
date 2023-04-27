using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Serializable için gerekti

public class GameAssets : MonoBehaviour//bu classın en baaşta çalışması lazımmış o yüzen edit >Project settings > script execution order dan gameAssets i en üste sürükledik.
{
    private static GameAssets Instance;//bu instance ı diğer classlardan buraya rahatca ulaşalım diye yaptık.

    public static GameAssets GetInstance(){
        return Instance;
    }

    private void Awake(){
        Instance = this;
    }


    public Sprite pipeHeadSprite;//bu mesela public. böylece unity ekranında direk böyle bir bölme gözüküyor.
    public Transform pfPipeHead;//Prefabs diye mi Transform dedik bilmiyorum.
    public Transform pfPipeBody;
    public Transform pfGround;
    public Transform pfCloud_1;
    public Transform pfCloud_2;
    public Transform pfCloud_3;



    public SoundAudioClip[] soundAudioClipArray;//sesliern içinde olduğu array

    [Serializable]//aşağıdaki classı editörde görmek için bu gerekliymiş
    public class SoundAudioClip{
        public SoundManager.Sound sound;//SoundMnager daki enumları editörde seçebildik
        public AudioClip audioClip;//AudioClipe de seslerimizi attık
    }

}
