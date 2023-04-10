using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{ 
    public Image fill;
    public static PlayerUI instanceUI;
    public Text AmmoText;


    private void Awake()
    {
        if (instanceUI == null)
        {
            instanceUI = this;
            return;

        }
        Debug.LogError("Sahnede birden fazla GameManager örneði");
        
    }
    


}
