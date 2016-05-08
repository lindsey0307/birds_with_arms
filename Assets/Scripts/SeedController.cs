using UnityEngine;
using System;

public class SeedController : MonoBehaviour {
  [SerializeField] float maxSpawnDelaySeconds;
  [SerializeField] GameObject seedPrefab;

  private DateTime nextSpawnTime;
  private System.Random random;
  private GameController gameController;
  private Rect levelBounds;
	
  public void Init(System.Random random, GameController gameController, Rect levelBounds) {
    this.random = random;
    this.levelBounds = levelBounds;
    this.gameController = gameController;
    this.nextSpawnTime = DateTime.Now.AddSeconds((float)random.NextDouble() * maxSpawnDelaySeconds);
  }

	void Update () {
    if (random != null && nextSpawnTime < DateTime.Now) {
      Debug.Log(nextSpawnTime + " / " + random);
      this.nextSpawnTime = DateTime.Now.AddSeconds((float)random.NextDouble() * maxSpawnDelaySeconds);
      var seed = GameObject.Instantiate(seedPrefab);
      float x = (float)(random.NextDouble()) * this.levelBounds.width + this.levelBounds.xMin;
      float y = (float)(random.NextDouble()) * this.levelBounds.height + this.levelBounds.yMin;
      seed.transform.position = new Vector3(x, y, y + 1.0f);
      gameController.AddSeed(seed);
    }
	}
}
