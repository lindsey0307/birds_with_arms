using UnityEngine;
using System.Collections;
using System;

public class PlayerMobility : MonoBehaviour {
  [SerializeField] private float speed;

	private string horizontalCtrl;
	private string verticalCtrl;
  private Rect levelBounds;
  private bool isAi;

  // stoopid ai birb movement tingz
  private int xMove = 0;
  private int yMove = 0;
  private DateTime nextDirectionSwtichTime;
  private System.Random random;

  public void Init(int playerId, System.Random random, Rect levelBounds) {
    this.levelBounds = levelBounds;
    this.random = random;
    isAi = playerId == 0;
    if (isAi) {
      nextDirectionSwtichTime = DateTime.Now;
    } else {
      horizontalCtrl = "Horizontal_P" + playerId;
      verticalCtrl = "Vertical_P" + playerId;
    }

    RandomizeStartPosition();
  }

	protected void FixedUpdate() {
    if (isAi) {
      DoAiMovement();
    } else {
      DoPlayerMovement();
    }

    RestrictMovementToLevelBounds();
	}

  private void RandomizeStartPosition() {
    Vector3 randomPosition = new Vector3(
      (float)(random.NextDouble() - .5) * levelBounds.width,
      (float)(random.NextDouble() - .5) * levelBounds.height,
      0);
    this.transform.position = randomPosition;
  }

  private void RestrictMovementToLevelBounds() {
    Vector3 boundedPosition = this.transform.position;
    if (boundedPosition.x < levelBounds.xMin) {
      boundedPosition.x = levelBounds.xMin;
    } else if (boundedPosition.x > levelBounds.xMax) {
      boundedPosition.x = levelBounds.xMax;
    }

    if (boundedPosition.y < levelBounds.yMin) {
      boundedPosition.y = levelBounds.yMin;
    } else if (boundedPosition.y > levelBounds.yMax) {
      boundedPosition.y = levelBounds.yMax;
    }

    this.transform.position = boundedPosition;
  }

  private void DoAiMovement() {
    if (DateTime.Now >= nextDirectionSwtichTime) {
      xMove = random.Next(-1, 2);
      yMove = random.Next(-1, 2);
      nextDirectionSwtichTime = DateTime.Now.AddSeconds(random.Next(800)/100f);
    }

    transform.position += (Vector3.right * xMove + Vector3.up * yMove) * speed * Time.deltaTime;
  }

  private void DoPlayerMovement() {
    float h = Input.GetAxis(horizontalCtrl);
    float v = Input.GetAxis(verticalCtrl);
    float t = 0.01f;

    if (h < -t) {
      Debug.Log(levelBounds + " - " + this.transform.position);
      transform.position += Vector3.left * speed * Time.deltaTime;
    }

    if (h > t) {
      transform.position += Vector3.right * speed * Time.deltaTime;
    }

    if (v > t) {
      transform.position += Vector3.up * speed * Time.deltaTime;
    }

    if (v < -t) {
      transform.position += Vector3.down * speed * Time.deltaTime;
    }
  }
}
