using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Transitionner : MonoBehaviour {
  public GameObject titlePanel;
  private DateTime transition1;
  private DateTime transition2;

	void Start () {
    transition1 = DateTime.Now.AddSeconds(10);
    transition2 = DateTime.Now.AddSeconds(20);
	}
	
	// Update is called once per frame
	void Update () {
    if (transition1 != null && DateTime.Now > transition1) {
      titlePanel.SetActive(true);
    }

    if (transition2 != null && DateTime.Now > transition2) {
      SceneManager.LoadScene("2d Top Down");
    }
	}
}
