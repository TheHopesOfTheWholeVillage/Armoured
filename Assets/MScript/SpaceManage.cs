  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManage : MonoBehaviour {
    [SerializeField]
    private GameObject[] obj;
    private bool Dco;
	// Use this for initialization
	void Start () {
        if (Dco==false) { StartCoroutine(wait()); }
           
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator wait()
    {
        yield return new WaitForSeconds( 2f);
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(true);
        }
        Dco = true;
    }
    IEnumerable waitOff()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(false);
        }
        Dco = false;
    }
}
