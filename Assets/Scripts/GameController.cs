using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
  [SerializeField] private GameObject birdPrefab;
  [SerializeField] private Camera camera;

  public List<BirdController> Birds { get; private set; }

  private System.Random random = new System.Random();

  void Start() {
    float vertExtent = camera.orthographicSize;
    float horzExtent = camera.orthographicSize * Screen.width / Screen.height;

    float minX = -horzExtent;
    float maxX = horzExtent;
    float minY = -vertExtent;
    float maxY = vertExtent;

    var bounds = new Rect(minX, minY, maxX - minX, maxY - minY);

    Birds = new List<BirdController>();

    for (int i = 1; i < 70; i++) {
      var bird = GameObject.Instantiate(birdPrefab);
      bird.transform.SetParent(this.transform);
      BirdController birdController = bird.GetComponent<BirdController>();
      birdController.Init(i, random, bounds, this, i > 1);
      Birds.Add(birdController);
    }
  }
}
