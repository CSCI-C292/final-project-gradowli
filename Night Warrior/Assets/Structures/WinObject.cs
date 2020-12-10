using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinObject : MonoBehaviour
{
    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnResetPlayer(object sender, EventArgs args) {
        this.gameObject.SetActive(false);
    }
}
