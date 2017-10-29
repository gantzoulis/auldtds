﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes_bck : MonoBehaviour
{

    public UnitBaseClass unitAttributes = new UnitBaseClass();
    private UnitAnimationPlayer unitAnimPlayer;
    private Animator unitAnimator;

	// Use this for initialization
	void Start ()
    {
        unitAnimPlayer = GetComponent<UnitAnimationPlayer>();
        unitAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (unitAttributes.unitHealthPoints <=0 && unitAttributes.unitIsAlive == true)
        {
            //Debug.Log("AAARRRGG i am dying");
            unitAttributes.unitIsAlive = false;
            unitAnimator.SetTrigger("Death");
            StartCoroutine(DestroyOnDeath());
        }
	}

    private IEnumerator DestroyOnDeath()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }

    public void AwardMinionXP()
    {
        GameObject xpManagerObject = GameObject.Find("XPBarManager");
        XPMan xpMan = xpManagerObject.GetComponent<XPMan>();
        xpMan.AwardXP(this.unitAttributes.unitEXPValue);
    }
}