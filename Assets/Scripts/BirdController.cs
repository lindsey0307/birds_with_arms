using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BirdController : MonoBehaviour {
  private const double ProbabilityBirdStandsIdle = 0.4;
  private const double ProbabilityBirdFlexes = 0.1;
  private const double ProbabilityBirdPecks = 0.4;
  private const float InputThreshold = 0.01f;

  private const float FlexLockTimerSeconds = 3.0f;
  private const float PeckLockTimerSeconds = 3.0f;
  private const float DeathTimerSeconds = 3.0f;

  [SerializeField] private float speed;
  [SerializeField] private Animator animator;
  [SerializeField] private string[] birdAnimationResources;
	       
  public int Id { get; private set; }

	private string horizontalCtrl;
	private string verticalCtrl;
  private string flexInput;
  private string killInput;
  private string peckInput;

  private Rect levelBounds;
  private bool isAi;
  private GameController gameController;

  private enum State { NORMAL, FLEXING, DEATH_FLEXING, PECKING, DEAD }
  private State birdState;
  private DateTime movemetLockTimer;
  private DateTime impendingDeathTimer;

  // stoopid ai birb movement tingz
  private int xMove = 0;
  private int yMove = 0;
  private DateTime nextDirectionSwtichTime;
  private System.Random random;
  private bool wasPreviouslyFacingLeft;

  public void Init(int playerId, System.Random random, Rect levelBounds, GameController controller, bool ai) {
    this.Id = playerId;
    this.gameController = controller;
    this.levelBounds = levelBounds;
    this.random = random;
    this.isAi = ai;
    this.birdState = State.NORMAL;
    this.movemetLockTimer = DateTime.Now;
    this.impendingDeathTimer = DateTime.Now.AddSeconds(120.0f + (float)random.NextDouble() * 5f);

    InitBirdAnim();

    if (isAi) {
      nextDirectionSwtichTime = DateTime.Now;
    } else {
      horizontalCtrl = "Horizontal_P" + playerId;
      verticalCtrl = "Vertical_P" + playerId;
      flexInput = "Flex_P" + playerId;
      killInput = "Kill_P" + playerId;
      peckInput = "Peck_P" + playerId;
    }

    RandomizeStartPosition();
  }

  public void KillWithFlex() {
    if (this.birdState != State.DEAD) {
      this.impendingDeathTimer = DateTime.Now.AddSeconds(DeathTimerSeconds);
    }
  }

  private void HandleDeath() {
    this.birdState = State.DEAD;
    animator.SetBool("birdDeath", true);
    xMove = 0;
    yMove = 0;
  }

	protected void FixedUpdate() {
    if (this.birdState == State.DEAD) {
      return;
    }

    if (this.impendingDeathTimer < DateTime.Now) {
      HandleDeath();
    }

    if (movemetLockTimer < DateTime.Now) {
			animator.SetBool("birdFlex", false);
			animator.SetBool("birdPeck", false);
      this.birdState = State.NORMAL;
    }

    UpdatePositionAndState();

    RestrictMovementToLevelBounds();

    Vector3 adjustedZPos = this.transform.position;
    adjustedZPos.z = this.transform.position.y - this.transform.localScale.z*0.4f;
    this.transform.position = adjustedZPos;
	}

  private void UpdatePositionAndState() {
    if (isAi) {
      DoAiUpdate();
    } else {
      if (this.birdState == State.NORMAL) {
        DoPlayerMovement();
      }
    }
  }

  private void InitBirdAnim() {
    int birdAnimResId = random.Next(this.birdAnimationResources.Length);
    string birdAnimRes = birdAnimationResources[birdAnimResId];
    animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(
      birdAnimRes, 
      typeof(RuntimeAnimatorController));
  }

  private void RandomizeStartPosition() {
    Vector3 randomPosition = new Vector3(
      (float)(random.NextDouble() - .5) * levelBounds.width,
      (float)(random.NextDouble() - .5) * levelBounds.height,
      0);
    randomPosition.z = randomPosition.y;
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

    boundedPosition.z = boundedPosition.y;
    this.transform.position = boundedPosition;
  }

  private void DoAiUpdate() {
    if (DateTime.Now >= nextDirectionSwtichTime) {
      xMove = 0;
      yMove = 0;

      if (random.NextDouble() < ProbabilityBirdFlexes) {
        HandleNormalFlex();
      } else if (random.NextDouble() < ProbabilityBirdPecks) {
        HandlePeck();
      } else {
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
      }

			if (xMove != 0 || yMove != 0) {
				animator.SetBool("birdWalk", true);
			} else {
				animator.SetBool("birdWalk", false);
			}

      bool birdFacingLeft = xMove < 0;
      UpdateBirdSpriteDirection(birdFacingLeft);

      nextDirectionSwtichTime = DateTime.Now.AddSeconds(random.Next(800)/100f);
    }

    transform.position += (Vector3.right * xMove + (Vector3.up + Vector3.forward) * yMove) * speed * Time.deltaTime;
  }

  private void UpdateBirdSpriteDirection(bool facingLeft) {
    if (facingLeft && !wasPreviouslyFacingLeft) {
      transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      wasPreviouslyFacingLeft = true;
    } else if (!facingLeft && wasPreviouslyFacingLeft) {
      transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
      wasPreviouslyFacingLeft = false;
    }
  }

  private void DoPlayerMovement() {
    float h = Input.GetAxis(horizontalCtrl);
    float v = Input.GetAxis(verticalCtrl);

    h = Mathf.Abs(h) > InputThreshold ? h / Mathf.Abs(h) : 0;
    v = Mathf.Abs(v) > InputThreshold ? v / Mathf.Abs(v) : 0;

    if (h > 0.5) {
      UpdateBirdSpriteDirection(false);
    } else if (h < -0.5) {
      UpdateBirdSpriteDirection(true);
    }

    transform.position += h * Vector3.right * speed * Time.deltaTime;
    transform.position += v * (Vector3.up + Vector3.forward) * speed * Time.deltaTime;

    if (Input.GetButton(peckInput)) {
      HandlePeck();
    } else if (Input.GetButton(killInput)) {
      HandleDeathFlex();
    } else if (Input.GetButton(flexInput)) {
      HandleNormalFlex();
    }

		if (h == 0 && v == 0) {
			animator.SetBool("birdWalk", false);
		} else {
			animator.SetBool("birdWalk", true);
		}
  }

  private void HandlePeck() {
    this.movemetLockTimer = DateTime.Now.AddSeconds(PeckLockTimerSeconds);
    this.birdState = State.PECKING;
		animator.SetBool("birdPeck", true);
    List<GameObject> seedsInRange = new List<GameObject>();

    foreach (var seed in this.gameController.Seeds) {
      if (InRange(seed.transform.position + new Vector3(0, -1f*this.transform.localScale.y, 0), 0.3f)) {
        seedsInRange.Add(seed);
      }
    }

    foreach (var seed in seedsInRange) {
      this.transform.localScale *= 1.1f;
      gameController.Seeds.Remove(seed);
      GameObject.Destroy(seed);
    }
  }

  private void HandleNormalFlex() {
    this.movemetLockTimer = DateTime.Now.AddSeconds(FlexLockTimerSeconds);
    this.birdState = State.FLEXING;
		animator.SetBool("birdFlex", true);
  }

  private void HandleDeathFlex() {
    this.movemetLockTimer = DateTime.Now.AddSeconds(FlexLockTimerSeconds);
    this.birdState = State.DEATH_FLEXING;
		animator.SetBool("birdFlex", true);

    List<BirdController> birds = gameController.Birds;
    foreach (var bird in birds) {
      if (bird.Id != this.Id && InRange(bird.transform.position)) {
        bird.KillWithFlex();
      }
    }
  }

  private bool InRange(Vector3 position, float rangeFactor = 1) {
    return Math.Abs(this.transform.position.x - position.x) < this.transform.localScale.z*rangeFactor &&
      Math.Abs(this.transform.position.y - position.y) < this.transform.localScale.z*rangeFactor;
  }
}
