using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;//pipe ların genişliği
    private const float PIPE_HEAD_HEIGHT = 3.75f;//pipe ın başının yüksekliği
    private const float PIPE_MOVE_SPEED = 30f;//boruların hareket hızı
    private const float PIPE_DESTROY_X_POSITION = -125f;//pipe ı yok etmek için
    private const float PIPE_SPAWN_X_POSITION = +125f;//bu nokstadan pipe lar spawn olcak
    private const float GROUND_DESTROY_X_POSITION = -240;//GROUND u yok etmek için
    private const float GROUND_SPAWN_X_POSITION = +125f;//bu nokstadan ground lar spawn olcak
    private const float CLOUD_DESTROY_X_POSITION = -240;
    private const float CLOUD_SPAWN_X_POSITION = +180;
    private const float CLOUD_SPAWN_Y_POSITION = 30;
    private const float BIRD_X_POSITION = 0f;


    private static Level instance;//başka class lardan buraya ulaşabilmek için instance tanımladık

    public static Level GetInstance(){
        return instance;
    }


    private List<Transform> groundList;
    private List<Transform> cloudList;
    private float cloudSpawnTimer;
    private List<Pipe> pipeList;//tüm pipe objeleri bu list de
    private int pipesPassedCount;
    private int pipesSpawned;//zorluğu ayarlamak için pipe sayılarını saydık
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;//ne kadar sürede bir pipe lar spawn olsun onu ayarlıyo
    private float gapSize;
    private State state;

    public enum Difficulty{
        Easy,
        Medium,
        Hard,
        Extreme,
    }


    private enum State{//ölümüyüz oynuyomuyuz yoksa başlamasını mı bekliyoruz
        WaitingToStart,
        Playing,
        BirdDead,
    }


    private void Awake(){
        instance = this;
        SpawnInitialGround();
        SpawnInitialClouds();
        pipeList = new List<Pipe>();//atama yapmak gibi bişey
        pipeSpawnTimerMax = 1f;//şu kadar saniyede bir spawnla dedik
        //SetDifficulty(Difficulty.Easy);//başlangıçta zorluk için easy dedik
        SetDifficulty(GetDifficulty());
        state = State.WaitingToStart;//başlangıçta başlamayı bekliyoruz
    }


    private void Start(){
        GameHandler.GetInstance().OnDied += Bird_OnDied;//bird class ında OnDied değişkenine Bird_OnDied fonksiyonunun içindeki eventi ata dedik
        GameHandler.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e){//bu fonksiyon event olayları sayesinde, Bird classından bu fonksiyonu çağırıyoruz ve state i playing yapıyoruz
        state = State.Playing;
    }



    private void Bird_OnDied(object sender, System.EventArgs e){//bu fonksiyonun ne zaman çalışcağına Bird class ı karar verdi
        state = State.BirdDead;//kuş öldü diye state i değiştirdik
    }


    private void Update(){
        if(state == State.Playing){//state durumu playing ise oyunaksın dedik
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }
        
    }

    private void SpawnInitialClouds(){
        cloudList = new List<Transform>();
        Transform cloudTransform;
        cloudTransform = Instantiate(GetCloudPrefabTransform() , new Vector3(0 , CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
    }


    private Transform GetCloudPrefabTransform(){//random şekilde 3 bulut şeklinde birini return et dedik
        switch(Random.Range(0,3)){
            default:
            case 0: return GameAssets.GetInstance().pfCloud_1;
            case 1: return GameAssets.GetInstance().pfCloud_2;
            case 2: return GameAssets.GetInstance().pfCloud_3;
        }
    }


    private void HandleClouds(){
        cloudSpawnTimer -= Time.deltaTime;
        if(cloudSpawnTimer < 0){//zamanı gelince 
            float cloudSpawnTimerMax = 6f;//şu kadar saniyede spawn olsun
            cloudSpawnTimer = cloudSpawnTimerMax;
            Transform cloudTransform = Instantiate(GetCloudPrefabTransform() , new Vector3(CLOUD_SPAWN_X_POSITION , CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
            cloudList.Add(cloudTransform);
        }


        for(int i= 0; i< cloudList.Count ; i++){
            Transform cloudTransform = cloudList[i];
            cloudTransform.position += new Vector3(-1 , 0 , 0) * PIPE_MOVE_SPEED * Time.deltaTime * .7f;//bulut daha yavaş ilerlesin diye 0.7 ile çarptık

            if(cloudTransform.position.x < CLOUD_DESTROY_X_POSITION){
                Destroy(cloudTransform.gameObject);
                cloudList.RemoveAt(i);
                i--;
            }
        }

    }
    


    private void SpawnInitialGround(){//ground u spawn et dedik
        groundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -45.6f;//spawn olcak ground un y konumu
        float groundWidth = 249.3f;//x konumu
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0,groundY,0), Quaternion.identity);//ilki prefabin objesini oluşturmak, ikincisi spawn olcak konum. zaten aşağıda yönünü ayarlıyoruz
        groundList.Add(groundTransform);//oluşanı objeyi list e ekledik
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth , groundY,0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth * 2f, groundY,0), Quaternion.identity);
        groundList.Add(groundTransform);
    }

    private void HandleGround(){//burda üstte oluşa 3 ground un konumunu tekrar değiştiriyoruz, yani mesela en soal gelince ve ekrandan çıkınca en sağa aynı olcak şekilde ground un konumunu ayarlıyoruz
        foreach(Transform groundTransform in groundList){//tüm groundaları döndük
            groundTransform.position += new Vector3(-1 , 0 , 0) * PIPE_MOVE_SPEED * Time.deltaTime;// sol yöne doğru git dedik 
        

            if(groundTransform.position.x < GROUND_DESTROY_X_POSITION){//ground u yoketceğimiz noktaya geldiğinde
                float rightMostXposition = -125f;
                for(int i =0; i< groundList.Count; i++){//tüm groundları dön ve bana en sağdaki groundun x konumunu ver, böylece ben hemen onun yanına yeni ground spawn ediyim
                    if(groundList[i].position.x > rightMostXposition){
                        rightMostXposition = groundList[i].position.x;
                    }
                }

                float groundWidth = 356f;
                groundTransform.position = new Vector3(rightMostXposition + groundWidth-107f, groundTransform.position.y, groundTransform.position.z);//burda en sağa tekrar gelicek ground un pozisyonunu ayarladık
            } 
        }
    }

    private void HandlePipeSpawning(){
        pipeSpawnTimer -= Time.deltaTime;//sıfır olana kadar zamanla azaldı, sıfır olunca ife girdi ve yeni pipe spwan oldu
        if(pipeSpawnTimer < 0){
            pipeSpawnTimer += pipeSpawnTimerMax;//sıfır olduğundan yine süre ekledik

            //buralarda max ve min height ı bulcak işlemler yaptık
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight =  totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);//pipe boşluklarının konumunu rastgele verlen değerler arasında bişey ver dedik
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    private void HandlePipeMovement(){//tüm pipeları hareket ettiriyoruz
        for (int i=0; i < pipeList.Count; i++){//tüm pipeları dolaştık.
            Pipe pipe = pipeList[i];

            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;//kuş borunun solundaysa yani boru daha kuşu geçmediyse true
            pipe.Move();//hareketi sağlıyon fonksiyonu çağırdık.
            if(isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom()){//eğer kuş yukardaki durumda true ise ve hareket sonrası artık kuşu geçtiyse if e girer. IsBottom ile de skor un 2 kez olmasını önlemiş olduk
                pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);//ismi Sound enumunda Score ile eşleşeni çal dedik.
            }
            
            if(pipe.GetXPosition() < PIPE_DESTROY_X_POSITION){//ekranın solundan çıktıysa yok et dedik
                pipe.DestroySelf();//yok et
                pipeList.Remove(pipe);//list den sildik yoksa null olarak gözükürdü list de
                i--;//listde azaltmaya gittik o yüzden bir tane pipe arada kaynayıp gierdi o yüzden bir azalttık. mesela 4.yü sildik, 5den bakmaya devam edersek, yeni 4e kayan sayıyı es geçerdik

            }
        }
    }



    private void SetDifficulty(Difficulty difficulty){//zorluğa göre aralıkları düzenledik
        switch (difficulty){
            case Difficulty.Easy:
                gapSize = 50f;//borular arası mesafe
                pipeSpawnTimerMax = 1.4f;//boruların spawn olma hızı
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.3f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Extreme:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.0f;
                break;            
        }
    }


    /*private Difficulty GetDifficulty(){//pipe sayısına göre zorluğu değiştirdik
        if (pipesSpawned >= 30) return Difficulty.Extreme;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }*/

    private Difficulty GetDifficulty(){//menu deki seçime göre zorluk değişiyor
        if(MainMenuWindow.difficulty==1){
            return Difficulty.Easy;
        }
        if(MainMenuWindow.difficulty==2){
            return Difficulty.Medium;
        }
        if(MainMenuWindow.difficulty==3){
            return Difficulty.Hard;
        }
        else{//difficulty = 4
            return Difficulty.Extreme;
        }
    }


    private void CreateGapPipes(float gapY, float gapSize, float xPosition){//burda pipe ları aralıklarla birlikte oluşturduk. gapY aralığın yüksekliği, gapSize aralığın boyutu.
        CreatePipe(gapY - gapSize * .5f, xPosition , true);//alt pipe için. 
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f , xPosition , false);
        pipesSpawned++;//pipe create ettik diye arttırdık
        SetDifficulty(GetDifficulty());//zorluk seviyesine göre gapSize ı değiştirdim. mesela 29 pipe vardı ve 30. şimdi oluştu, Difficulty yi Extreme a getirmiş oldum burda, bundan sonra oluşcak pipe boşluğu ona göre ayarlancak

    }

    private void CreatePipe(float height, float xPosition , bool isBottom){
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);//Instantiate ile PRefab i oluşturduk sanırım. yani Instantiate hazır bir şey galiba.
        float pipeHeadYPosition;
        if(isBottom){//eğer aşağıda ise pipe
            pipeHeadYPosition =  -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;//pipe ın kafasının yüksekliğini ayarladık. yüksekliği ayarlarken başının orta kısmı tam vücudun üstüne gelsin diye orda çıkarma çarpma vs. yaptık.
        } else{
            pipeHeadYPosition =  +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition,pipeHeadYPosition);//bu Prefab a x konumu ve yükseklik tayin ettik. ilk kısım x konumu. ikincisi yükseklik
        



        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if(isBottom){//eğer aşağıda ise pipe
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;//yüksekliğini ayarladık
        }else{
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1 , -1 , 1);//yukarda iken pipe lar yukarı doğru çıkıyordu bunu düzeltmek için y yönünde ters çevirdik
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);
        


        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();//üstte oluşturduğumuz objenin spriteında düzenleme yapabilmek için bunu oluşturduk.
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);//pipe ın genişliğini ve y konumunu düzenledik. aslında sprite ın uzunluğunu kod ile düzneliyoruz. burda height ile yüksekliğini ayarlıyoruz ve genişliğide.

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();//üstte sprite ın boyunu düzenledik, burda collider ın
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);//burda collider ın boyutunu pipe gibi yaptık
        pipeBodyBoxCollider.offset = new Vector2(0f , height * .5f);//burda ise box collider ı borunun boyunun yarısı kadar yukarı kaydırdık. çünkü biz borunun merkezi için bottom demiştk o yüzden borunun yarısı kadar da yuakrı taşımamız gerekti


        Pipe pipe = new Pipe(pipeHead, pipeBody, isBottom);//Create etmiştik şimdi objesini oluşturduk
        pipeList.Add(pipe);//oluşan objeyi list e attık.
    }


    public int GetPipesSpawned(){
        return pipesSpawned;
    }

    public int GetpipesPassedCount(){//Score dan buna ulaşdık
        return pipesPassedCount;
    }



    private class Pipe{//her pipe için objesini oluşturuyoruz
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;//pipe üst mü alt mı onun için

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom){
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;//pipe üst mü alt mı onun için
        }

        public void Move(){
            pipeHeadTransform.position += new Vector3(-1 ,0 ,0) * PIPE_MOVE_SPEED * Time.deltaTime;//sol yönüne doğru şu hızda zamanla ilerlesinler dedik.
            pipeBodyTransform.position += new Vector3(-1 ,0 ,0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition(){//pipe kamerayı geçince yok etmemiz için x konumunu aldık
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom(){//bu pipe objesi alt mı üstü mü onu döndürüyo
            return isBottom;
        }

        public void DestroySelf(){//objeleri yok eden fonksiyon
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

    }

}
