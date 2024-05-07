using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI txt;
    /*
	public float deltaTime;
    void Awake(){
        Fpsc();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		txt.text = Mathf.Ceil (fps).ToString ();
    }
    void Fpsc(){
        Application.targetFrameRate=60;
    }*/
}
