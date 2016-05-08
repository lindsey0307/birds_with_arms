using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Transitionner : MonoBehaviour {
  private DateTime transition1;

	void Start () {
    transition1 = DateTime.Now.AddSeconds(10);
	}
	
	// Update is called once per frame
	void Update () {
    if (transition1 != null && DateTime.Now > transition1) {
      SceneManager.LoadScene("2d Top Down");
    }
	}
}
