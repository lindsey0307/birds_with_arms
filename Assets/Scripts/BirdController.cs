using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BirdController : MonoBehaviour {
  private const double ProbabilityBirdStandsIdle = 0.4;

  [SerializeField] private float speed;
  [SerializeField] private BoxCollider2D colliderBox;
         
  public int Id { get; private set; }

	private string horizontalCtrl;
	private string verticalCtrl;
  private Rect levelBounds;
  private bool isAi;
  private GameController gameController;

  // stoopid ai birb movement tingz
  private int xMove = 0;
  private int yMove = 0;
  private DateTime nextDirectionSwtichTime;
  private System.Random random;

  public void Init(int playerId, System.Random random, Rect levelBounds, GameController controller) {
    this.Id = playerId;
    this.gameController = controller;
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

  public void KillWithFlex() {
    this.gameObject.SetActive(false);
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
      double nWeight = 2.0 * this.transform.position.y / this.levelBounds.height;
      double eWeight = 2.0 * this.transform.position.x / this.levelBounds.width;
      xMove = random.Next(-1, 2);
      yMove = random.Next(-1, 2);

      if (random.NextDouble() < Math.Abs(nWeight)) {
        yMove = nWeight > 0 ? -1 : 1;
      }

      if (random.NextDouble() < Math.Abs(eWeight)) {
        xMove = eWeight > 0 ? -1 : 1;
      }

      if (random.NextDouble() < ProbabilityBirdStandsIdle) {
        xMove = 0;
        yMove = 0;
      }

      nextDirectionSwtichTime = DateTime.Now.AddSeconds(random.Next(800)/100f);
    }

    transform.position += (Vector3.right * xMove + (Vector3.up + Vector3.forward) * yMove) * speed * Time.deltaTime;
  }

  private void DoPlayerMovement() {
    float h = Input.GetAxis(horizontalCtrl);
    float v = Input.GetAxis(verticalCtrl);
    float t = 0.01f;

    if (Input.GetKeyDown(KeyCode.Space)) {
      HandleDeathFlex();
    }

    if (h < -t) {
      transform.position += Vector3.left * speed * Time.deltaTime;
    }

    if (h > t) {
      transform.position += Vector3.right * speed * Time.deltaTime;
    }

    if (v > t) {
      transform.position += (Vector3.up + Vector3.forward) * speed * Time.deltaTime;
    }

    if (v < -t) {
      transform.position += (Vector3.down + Vector3.back) * speed * Time.deltaTime;
    }
  }

  void OnCollisionEnter(Collision collision) {
    Debug.Log("TEST");
    foreach (ContactPoint contact in collision.contacts) {
      Debug.DrawRay(contact.point, contact.normal, Color.white);
    }
  }

  private void HandleDeathFlex() {
    Debug.Log("FLEX!");
    List<BirdController> birds = gameController.Birds;
    foreach (var bird in birds) {
      if (bird.Id != this.Id && InRange(bird.transform)) {
//        Debug.Log("BIRD HIT!");
        bird.KillWithFlex();
      }
    }
  }

  private bool InRange(Transform birdTransform) {
    return Math.Abs(this.transform.position.x - birdTransform.position.x) < 1.0 &&
      Math.Abs(this.transform.position.y - birdTransform.position.y) < 1.0;
  }
}
