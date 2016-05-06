using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {
  [SerializeField] private GameObject birdPrefab;

  void Start() {
    var bird = GameObject.Instantiate(birdPrefab);
    bird.transform.SetParent(this.transform);
  }
}
