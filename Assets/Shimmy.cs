using UnityEngine;
using System.Collections;

public class Shimmy : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
    this.transform.position += new Vector3(0.02f, 0, 0);
	}
}
