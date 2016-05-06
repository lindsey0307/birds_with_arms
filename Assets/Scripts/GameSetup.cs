using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {
  [SerializeField] private GameObject birdPrefab;

  void Start() {
    for (int i = 0; i < 10; i++) {
      var bird = GameObject.Instantiate(birdPrefab);
      bird.transform.SetParent(this.transform);
      PlayerMobility birdController = bird.GetComponent<PlayerMobility>();
      birdController.Init(i < 5 ? i : 0);
    }
  }
}
