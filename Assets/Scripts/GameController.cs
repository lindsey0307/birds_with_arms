using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
  [SerializeField] private GameObject birdPrefab;
  [SerializeField] private Camera camera;
  [SerializeField] private int playerCount;
  [SerializeField] private int totalBirdCount;

  public List<BirdController> Birds { get; private set; }

  private System.Random random = new System.Random();

  void Start() {
    float vertExtent = camera.orthographicSize;
    float horzExtent = camera.orthographicSize * Screen.width / Screen.height;
    var bounds = new Rect(-horzExtent, -vertExtent, 2*horzExtent, 2*vertExtent);

    Birds = new List<BirdController>();

    for (int i = 1; i <= totalBirdCount; i++) {
      var bird = GameObject.Instantiate(birdPrefab);
      bird.transform.SetParent(this.transform);
      BirdController birdController = bird.GetComponent<BirdController>();
      birdController.Init(i, random, bounds, this, i > playerCount);
      Birds.Add(birdController);
    }
  }
}
