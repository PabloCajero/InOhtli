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
            gameData = JsonUtility.FromJson<ClassGameData>(RawData);
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
        File.WriteAllText(Archive,StringJson);
        Debug.Log("Guardado");
    }
}
