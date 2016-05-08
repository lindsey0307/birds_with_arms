using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
  [SerializeField] private SeedController seedController;
  [SerializeField] private GameObject birdPrefab;
	[SerializeField] private GameObject blueLightPrefab;
	[SerializeField] private GameObject pinkLightPrefab;
  [SerializeField] private Camera camera;
  [SerializeField] private int playerCount;
  [SerializeField] private int totalBirdCount;

  public List<BirdController> Birds { get; private set; }
  public List<GameObject> Seeds { get; private set; }

  private System.Random random = new System.Random();

  public void AddSeed(GameObject seed) {
    this.Seeds.Add(seed);
  }

  void Start() {
    GameObject.Instantiate(blueLightPrefab);
    GameObject.Instantiate(pinkLightPrefab);

    float vertExtent = camera.orthographicSize;
    float horzExtent = camera.orthographicSize * Screen.width / Screen.height;
    float vertShift = vertExtent * 0.1f;
    vertExtent *= 0.75f;
    horzExtent *= 0.9f;

    var bounds = new Rect(-horzExtent, -vertExtent - vertShift, 2*horzExtent, 2*vertExtent);

    Birds = new List<BirdController>();
    Seeds = new List<GameObject>();

    this.seedController.Init(random, this, bounds);

    for (int i = 1; i <= totalBirdCount; i++) {
      var bird = GameObject.Instantiate(birdPrefab);
      bird.transform.SetParent(this.transform);
      BirdController birdController = bird.GetComponent<BirdController>();
      birdController.Init(i, random, bounds, this, i > playerCount);
      Birds.Add(birdController);
    }
  }
}
