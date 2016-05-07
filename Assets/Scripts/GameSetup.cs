using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {
  [SerializeField] private GameObject birdPrefab;
  [SerializeField] private Camera camera;

  private System.Random random = new System.Random();

  void Start() {
    float vertExtent = camera.orthographicSize;
    float horzExtent = camera.orthographicSize * Screen.width / Screen.height;

    float minX = -horzExtent;
    float maxX = horzExtent;
    float minY = -vertExtent;
    float maxY = vertExtent;

    var bounds = new Rect(minX, minY, maxX - minX, maxY - minY);

    for (int i = 0; i < 70; i++) {
      var bird = GameObject.Instantiate(birdPrefab);
      bird.transform.SetParent(this.transform);
      PlayerMobility birdController = bird.GetComponent<PlayerMobility>();
      birdController.Init(i < 2 ? i : 0, random, bounds);
    }
  }
}
