using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    public GameObject Player;
    public String Archive;
    public ClassGameData gameData = new ClassGameData();
     private string keyWord = "Password";
    private void Awake(){
        Archive = Application.dataPath+"/gameData.Json";
        Player = GameObject.FindGameObjectWithTag("Player");
        DataLoad();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.C)){
            DataSave();
        }
        if(Input.GetKeyDown(KeyCode.G)){
            DataLoad();
        }
    }

    private void DataLoad(){
        if(File.Exists(Archive)){
            string RawData = File.ReadAllText(Archive);
            gameData = JsonUtility.FromJson<ClassGameData>(EncryptDecrypt(RawData));
            Debug.Log("Position "+gameData.position);
            Player.transform.position = gameData.position;
            Player.GetComponent<hpSystem>().SetHP(gameData.HP);
        }else{
            Debug.Log("El archivo no existe");
        }
    }

    private void DataSave(){
        ClassGameData newData= new ClassGameData(){
            position = Player.transform.position,
            HP = Player.GetComponent<hpSystem>().GetHP()
        };
        string StringJson = JsonUtility.ToJson(newData);
        File.WriteAllText(Archive,EncryptDecrypt(StringJson));
        Debug.Log("Guardado");
    }
    private string EncryptDecrypt( string Data )
    {
        string result = "";

        for (int i = 0; i < Data.Length; i++)
            result += (char) ( Data[i] ^ keyWord[i % keyWord.Length] );
        
        return(result);
    }
}
