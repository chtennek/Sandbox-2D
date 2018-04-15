using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camAnim : MonoBehaviour {
    public maxCamera camscript;
    public float t = 1;
    public float mult = 2;
    private float d = 0;
	// Use this for initialization

    void OnEnable()
    {
        StartCoroutine(_Start());
    }


	IEnumerator _Start () {
        yield return new WaitForSeconds(t);
        d = camscript.desiredDistance;
		StartCoroutine(zoomIn());
	}
	
	// Update is called once per frame
    IEnumerator zoomIn()
    {
        d = camscript.desiredDistance;
        camscript.desiredDistance = d*mult;
        yield return new WaitForSeconds(t);
        StartCoroutine(zoomOut());    
    }

    IEnumerator zoomOut()
    {
        camscript.desiredDistance = d;
        yield return new WaitForSeconds(t);
        //StartCoroutine(zoomOut());
    }
    void Update() { }
}
