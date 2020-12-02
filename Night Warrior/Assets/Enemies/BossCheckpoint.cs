using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossCheckpoint : MonoBehaviour
{
    [SerializeField] GameObject _boss;
    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateBoss() {
        _boss.SetActive(true);
    }
    void OnResetPlayer(object sender, EventArgs args) {
        this.gameObject.SetActive(true);
    }
}
