using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//bunu ekledik

public static class Loader{//nesnesini oluşturmayalım diye static yaptım dedi.

    public enum Scene{
        GameScene,
        Loading,
        MainMenu,
    }

    private static Scene targetScene;

    public static void Load(Scene scene){//parametrede girilen scene i yükle dedik
        SceneManager.LoadScene(Scene.Loading.ToString());//başta loading ekranını yükledik

        targetScene = scene;//targetScene e istediğimiz scene i atadık
        
    }
    
    public static void LoadTargetScene(){//targetScene e atadığımız scene i yükle dedik
        SceneManager.LoadScene(targetScene.ToString());
    }
}
