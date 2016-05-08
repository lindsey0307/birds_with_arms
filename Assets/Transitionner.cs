using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Transitionner : MonoBehaviour {
  public GameObject titlePanel;
  public GameObject controlsPanel;
  private DateTime transition1;
  private DateTime transition2;
  private DateTime transition3;

	void Start () {
    transition1 = DateTime.Now.AddSeconds(10);
    transition2 = DateTime.Now.AddSeconds(20);
    transition3 = DateTime.Now.AddSeconds(30);
	}
	
	// Update is called once per frame
	void Update () {
    if (transition1 != null && DateTime.Now > transition1) {
      titlePanel.SetActive(true);
    }

    if (transition2 != null && DateTime.Now > transition2) {
      controlsPanel.SetActive(true);
      titlePanel.SetActive(false);
    }

    if (transition3 != null && DateTime.Now > transition3) {
      SceneManager.LoadScene("2d Top Down");
    }
	}
}
