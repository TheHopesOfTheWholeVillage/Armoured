using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoutn : MonoBehaviour {
    [SerializeField]
    private Button UIbuton;
    private Ray ray;
    private Camera ARcam;
    private RaycastHit hit;
    // Use this for initialization
    
    void Start () {
        ARcam = gameObject.GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {
      
        //Debug.Log(hit.collider.tag);
        ray = ARcam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100))
        {
            
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            if (hit.collider.tag =="uib")
            {
                if (UIbuton.GetComponent<Image>().fillAmount <1)
                {
                    UIbuton.GetComponent<Image>().fillAmount += 0.01f;
                }

            }
            else
            {
                if (UIbuton.GetComponent<Image>().fillAmount >0)
                {
                    UIbuton.GetComponent<Image>().fillAmount -= 0.01f;
                }
            }
            if (hit.collider == null)
            {
                if (UIbuton.GetComponent<Image>().fillAmount >0)
                {
                    UIbuton.GetComponent<Image>().fillAmount -= 0.01f;
                }
            }


        }
        
    }
    void ButtonDcs()
    {
        if (UIbuton.GetComponent<Image>().fillAmount < 1)
        {
            UIbuton.GetComponent<Image>().fillAmount += 0.1f;
        }
    }
}
