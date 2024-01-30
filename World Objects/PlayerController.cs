using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerMode { idle, walkLeft, walkRight, runLeft, runRight, 
	jumpFull, jumpHalf, attack, nada
};

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    [Header("Set in Inspector")]
	// Grounded
	public Transform		groundCheck;
	private float			groundCheckRadius = 0.25f;
	public LayerMask		groundLayer;
	public bool				isGrounded;

	[Header("Set Dynamically")]
	public float			moveSpeed = 4f;
	private float			jumpSpeed = 13f;

	// Sprite Facing 
	public bool				facingRight = true;

	// Mode
    public ePlayerMode		mode = ePlayerMode.idle;

	// Location
	public Vector3			respawnPos;

	// Drag
	private float			drag = 0.5f;

	public bool				canMove = true;

	// Components
	public Rigidbody2D			rigid;
	private Animator			anim;
    private CircleCollider2D	circleColl;
    private SpriteRenderer		sRend;

    // Singleton
    private static PlayerController _S;
	public static PlayerController S { get { return _S; } set { _S = value; } }

    // DontDestroyOnLoad
    private static bool exists;

    void Awake() {
        // Singleton
        S = this;

        // DontDestroyOnLoad
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }

        // Get Components
        rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();
        sRend = GetComponent<SpriteRenderer>();

        // Add Loop() to UpdateManager
        UpdateManager.updateDelegate += Loop;
		// Add FixedLoop() to UpdateManager
		UpdateManager.fixedUpdateDelegate += FixedLoop;
	}

	private void Start() {
		SetPlayerToStartingPosition();
	}

	// Set player to starting position
	public void SetPlayerToStartingPosition() {
		transform.position = respawnPos;
	}

	void CheckForWalkInput() {
		if (Input.GetAxisRaw("Attack/Run") <= 0 && Input.GetAxisRaw("Horizontal") > 0f) {
			mode = ePlayerMode.walkRight;
		} else if (Input.GetAxisRaw("Attack/Run") <= 0 && Input.GetAxisRaw("Horizontal") < 0f) {
			mode = ePlayerMode.walkLeft;
		}
	}

	void CheckForRunInput() {
		if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") > 0) {
			mode = ePlayerMode.runRight;
		} else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") > 0) {
			mode = ePlayerMode.runLeft;
		}
	}

	void CheckForJumpInput(float speed) {
		if (isGrounded) {
			if (Input.GetButtonDown("MyJump")) {
				rigid.velocity = Vector3.up * speed;
				mode = ePlayerMode.jumpFull;
			} else if (Input.GetButtonUp("MyJump")) {
				if (rigid.velocity.y > 0) {
					mode = ePlayerMode.jumpHalf;
				}
			}
		}
	}

    // Check for player input, then execute the desired action
    public void Loop () {
        if (!GameManager.S.paused && canMove) {
			switch (mode) {
				case ePlayerMode.idle:
					// Attack
					if (Input.GetAxisRaw("Attack/Run") > 0 && isGrounded) { mode = ePlayerMode.attack; }
					// Walk 
					CheckForWalkInput();
					// Run
					CheckForRunInput();
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.walkLeft:
					// Idle
					if (Input.GetAxisRaw("Horizontal") >= 0) {
                        mode = ePlayerMode.idle;
                    }
					// Run Left
					if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runLeft;
					}
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.walkRight:
					// Idle
					if (Input.GetAxisRaw("Horizontal") <= 0) {
                        mode = ePlayerMode.idle;
                    }
					// Run Right
					if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runRight;
					}
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.runLeft:
					// Idle
					if (Input.GetAxisRaw("Horizontal") >= 0) {
                        mode = ePlayerMode.idle;
                    }
					// Walk Left
					if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") <= 0) {
						mode = ePlayerMode.walkLeft;
					}
					// Run Right
					if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runRight;
					}
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.runRight:
					// Idle
					if (Input.GetAxisRaw("Horizontal") <= 0) {
                        mode = ePlayerMode.idle;
                    }
					// Walk Right
					if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") <= 0) {
						mode = ePlayerMode.walkRight;
					}
					// Run Left
					if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runLeft;
					}
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.attack:
					// Idle
					if (Input.GetAxisRaw("Attack/Run") <= 0) {
                        mode = ePlayerMode.idle;
                    }
					// Walk
					CheckForWalkInput();
					// Run
					CheckForRunInput();
					// Jump
					CheckForJumpInput(jumpSpeed);
					break;
				case ePlayerMode.jumpFull:
					if (isGrounded) {
						// Attack
						if (Input.GetAxisRaw("Attack/Run") > 0) { mode = ePlayerMode.attack; }
						// Walk 
						CheckForWalkInput();
						// Run
						CheckForRunInput();
					}
					break;
				case ePlayerMode.jumpHalf:
					if (isGrounded) {
						// Attack
						if (Input.GetAxisRaw("Attack/Run") > 0) { mode = ePlayerMode.attack; }
						// Walk 
						CheckForWalkInput();
						// Run
						CheckForRunInput();
					}
					break;
			}
		}
	}

	public void FixedLoop() {
        if (gameObject.activeInHierarchy) {
			if (!GameManager.S.paused && canMove) {
				// Check and set whether the player is grounded
				isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

				// Flip the player's sprite to face left or right depending on which direction button they've last pressed
				if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight) {
					Flip();
				} else if (Input.GetAxisRaw("Horizontal") < 0 && facingRight) {
					Flip();
				}

				// ************ DRAG ************ \\
				// Multiplies the x velocity component by a value slightly lower than 1 each FixedUpdate,
				// which reproduces a natural energy loss. Drag can range from 0 (no drag) to 1 (stops immediately).
				var vel = rigid.velocity;
				vel.x *= 1.0f - drag;
				rigid.velocity = vel;

				// Play jump animation if not grounded 
				if (!isGrounded) {
					if (mode != ePlayerMode.attack && mode != ePlayerMode.nada) {
						SetAnim("Player_Jump", 0);
					}
				}

                // Depending on the player's mode, set its velocity and which animation clip to play
                switch (mode) {
					case ePlayerMode.idle:
						// Set animation clip
						if (isGrounded) {
							SetAnim("Player_Idle");
						} else {
							SetAnim("Player_Jump", 0);
						}
						break;
					case ePlayerMode.walkLeft:
						// Set velocity
						rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);

						// Set animation clip
						if (isGrounded) {
							SetAnim("Player_Walk");
						}
						break;
					case ePlayerMode.walkRight:
						// Set velocity
						rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);

						// Set animation clip
						if (isGrounded) {
							SetAnim("Player_Walk");
						}
						break;
					case ePlayerMode.attack:
						// Set animation clip
						SetAnim("Player_Attack", 0);
						break;
					case ePlayerMode.jumpFull:
						mode = ePlayerMode.idle;
						break;
					case ePlayerMode.jumpHalf:
						// Set velocity
						rigid.velocity *= 0.5f;

                        mode = ePlayerMode.idle;
                        break;
					case ePlayerMode.runLeft:
						// Set velocity
						rigid.velocity = new Vector2(-moveSpeed * 2, rigid.velocity.y);

						// Set animation clip
						if (isGrounded) {
							SetAnim("Player_Run");
						}
						break;
					case ePlayerMode.runRight:
						// Set velocity
						rigid.velocity = new Vector2(moveSpeed * 2, rigid.velocity.y);

						// Set animation clip
						if (isGrounded) {
							SetAnim("Player_Run");
						}
						break;
				}
			}
		}
	}

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public void SetAnim(string animName = "Player_Idle", int animSpeed = 1) {
        anim.speed = animSpeed;
        anim.CrossFade(animName, 0);
    }

	// Methods for when interacting with a hideable spot that set whether
	// the player can move, is visible, and their collider is active
	public void Hide() {
		canMove = false;
        //circleColl.enabled = false;
		sRend.enabled = false;

        // Set player velocity to 0
        rigid.velocity = Vector2.zero;

        // Freeze camera
        CameraManager.S.camMode = eCamMode.freezeCam;
    }
    public void StopHiding() {
        canMove = true;
        //circleColl.enabled = false;
        sRend.enabled = true;

		// Unfreeze camera
		CameraManager.S.camMode = eCamMode.followAll;
    }
}