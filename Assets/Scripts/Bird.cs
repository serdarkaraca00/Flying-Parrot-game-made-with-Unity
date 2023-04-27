using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using System;//event şeyini ekledik diye bunu ekledik

public class Bird : MonoBehaviour
{
    private const float JUMP_AMOUNT = 90f;//zıplama miktarını burda değiştiriyoruz. sabit olduğundan const kullandık.

    //private static Bird instance;//başka classdan burdaki değişkenlere ve fonskiyonlara ulaşmak için instance yaptık

    /*public static Bird GetInstance(){
        return instance;
    }*/

    //public event EventHandler OnDied;//bu event olayları sayesinde diğer classdaki fonksiyonu burdan çalıştırabiliyoruz
    //public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdRigidbody2D;
    private State state;

    private enum State{
        WaitingToStart,
        Playing,
        Dead,
    }


    private void Awake(){
        //instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();//bizim kuşun componentiyle eşleştirdik yani atadık.
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;//oyun başında kuş sabit kalsın istedik
        state = State.WaitingToStart;//bekleme modunda oyun başladı
    }


    private void Update(){
        switch (state){
        default:
        case State.WaitingToStart://ilk zıplamayı bekliyoruz, bu sırada kuş sabit
            /*if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){//space yada sol tıkla zıpla dedik
                state = State.Playing;//ilk zıplama geldi artık playing state ine geçtik ve oyun akmaya başladı
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;//kuş hareket eder kıvama getirdik
                Jump();
                //if(OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);//Level daki fonksiyon çalışsın dedik ve ordaki state i playing yaptık. ve WaitingToStarWindow da
                GameHandler.GetInstance().OnStartedPlaying_Uygula();
            }*/
            if( WaitingToStartWindow.PlayPressedCounter==1){//bu if kısmı mobil için. en az bir parmak algılanırsa 

                WaitingToStartWindow.PlayPressedCounter = 0;
                state = State.Playing;//ilk zıplama geldi artık playing state ine geçtik ve oyun akmaya başladı
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;//kuş hareket eder kıvama getirdik
                Jump();
                //if(OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);//Level daki fonksiyon çalışsın dedik ve ordaki state i playing yaptık. ve WaitingToStarWindow da
                GameHandler.GetInstance().OnStartedPlaying_Uygula();
            }
            break;
        case State.Playing://kuş harekete başladı
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){//space yada sol tıkla zıpla dedik
                Jump();
            }
            /*if(Input.touchCount > 0){//bu if kısmı mobil için. en az bir parmak algılanırsa 

                Touch touch = Input.GetTouch(0);//ilk parmağı al.

                if(touch.phase == TouchPhase.Began){//ekrana basılır basılmaz.
                    Jump(); 
                }
            }*/

            transform.eulerAngles = new Vector3(0  , 0 , birdRigidbody2D.velocity.y * .1f);//bununla bird sprite ını zıpladığımızda öne doğru döndürüyoruz. 0.1 ile çarptım yoksa çok fazla dönüyo
            break;
        case State.Dead:
            break;
        }
    }


    private void Jump(){
        if(birdRigidbody2D.bodyType != RigidbodyType2D.Static){//body static değilse zıplasın dedik. yoksa uyarı mesajı geliyodu
            birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;//verilen miktarda yukarı git dedik.
            SoundManager.PlaySound(SoundManager.Sound.BirdJump);//ismi Sound enmunda BirdJump ile eşleşeni çal dedik.
        }

        
    }



    private void OnTriggerEnter2D(Collider2D collider){//bu hazır fonksiyon gibi bişey sanırım. herhangi bir şeekilde triger olunca içindekini yap diyo. 
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;//ölme durumunda kuş hareketi sabit kalsın dedik
        SoundManager.PlaySound(SoundManager.Sound.Lose);//ismi Sound enmunda Lose ile eşleşeni çal dedik.
        GameHandler.GetInstance().OnDied_Uygula();//şimdi Level classında bu fonksiyonu çalıştır dedik ve oyun durdu bu sayede. VE Score dan dan çağırdık ve high score oldu mu diye kontrol ettik.
    }




}
