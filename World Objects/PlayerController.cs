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
	private CircleCollider2D	circleColl;
	private Animator			anim;

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
		circleColl = GetComponent<CircleCollider2D>();
		anim = GetComponent<Animator>();

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

	void InputWalk() {
		if (Input.GetAxisRaw("Attack/Run") <= 0 && Input.GetAxisRaw("Horizontal") > 0f) {
			mode = ePlayerMode.walkRight;
		} else if (Input.GetAxisRaw("Attack/Run") <= 0 && Input.GetAxisRaw("Horizontal") < 0f) {
			mode = ePlayerMode.walkLeft;
		}
	}

	void InputRun() {
		if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") > 0) {
			mode = ePlayerMode.runRight;
		} else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") > 0) {
			mode = ePlayerMode.runLeft;
		}
	}

	void InputJump(float speed) {
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

	public void Loop () {
        if (!GameManager.S.paused && canMove) {
			switch (mode) {
				case ePlayerMode.idle:
					// Attack
					if (Input.GetAxisRaw("Attack/Run") > 0 && isGrounded) { mode = ePlayerMode.attack; }
					// Walk 
					InputWalk();
					// Run
					InputRun();
					// Jump
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.walkLeft:
					// Idle
					if (Input.GetAxisRaw("Horizontal") >= 0) {
						SetMode();
					}
					// Run Left
					if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runLeft;
					}
					// Jump
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.walkRight:
					// Idle
					if (Input.GetAxisRaw("Horizontal") <= 0) {
						SetMode();
					}
					// Run Right
					if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Attack/Run") > 0) {
						mode = ePlayerMode.runRight;
					}
					// Jump
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.runLeft:
					// Idle
					if (Input.GetAxisRaw("Horizontal") >= 0) {
						SetMode();
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
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.runRight:
					// Idle
					if (Input.GetAxisRaw("Horizontal") <= 0) {
						SetMode();
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
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.attack:
					// Idle
					if (Input.GetAxisRaw("Attack/Run") <= 0) { SetMode(); }
					// Walk
					InputWalk();
					// Run
					InputRun();
					// Jump
					InputJump(jumpSpeed);
					break;
				case ePlayerMode.jumpFull:
					if (isGrounded) {
						// Attack
						if (Input.GetAxisRaw("Attack/Run") > 0) { mode = ePlayerMode.attack; }
						// Walk 
						InputWalk();
						// Run
						InputRun();
					}
					break;
				case ePlayerMode.jumpHalf:
					if (isGrounded) {
						// Attack
						if (Input.GetAxisRaw("Attack/Run") > 0) { mode = ePlayerMode.attack; }
						// Walk 
						InputWalk();
						// Run
						InputRun();
					}
					break;
			}
		}
	}

	public void FixedLoop() {
        if (gameObject.activeInHierarchy) {
			if (!GameManager.S.paused && canMove) {
				// Grounded
				isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

				// Flip
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

				// Jump Animation
				if (!isGrounded) {
					if (mode != ePlayerMode.attack && mode != ePlayerMode.nada) {
						SetAnim("Player_Jump", 0);
					}
				}

				switch (mode) {
					case ePlayerMode.idle:
						// Anim
						if (isGrounded) {
							SetAnim("Player_Idle");
						} else {
							SetAnim("Player_Jump", 0);
						}
						break;
					case ePlayerMode.walkLeft:
						// Set velocity
						rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);

						// Anim
						if (isGrounded) {
							SetAnim("Player_Walk");
						}
						break;
					case ePlayerMode.walkRight:
						// Set velocity
						rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);

						// Anim
						if (isGrounded) {
							SetAnim("Player_Walk");
						}
						break;
					case ePlayerMode.attack:
						// Anim
						SetAnim("Player_Attack", 0);
						break;
					case ePlayerMode.jumpFull:
						SetMode();
						break;
					case ePlayerMode.jumpHalf:
						// Set velocity
						rigid.velocity *= 0.5f;

						SetMode();
						break;
					case ePlayerMode.runLeft:
						// Set velocity
						rigid.velocity = new Vector2(-moveSpeed * 2, rigid.velocity.y);

						// Anim
						if (isGrounded) {
							SetAnim("Player_Run");
						}
						break;
					case ePlayerMode.runRight:
						// Set velocity
						rigid.velocity = new Vector2(moveSpeed * 2, rigid.velocity.y);

						// Anim
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

	public void SetMode(ePlayerMode newMode = ePlayerMode.idle, int gravity = 99, bool zeroVelocity = false) {
		// Reset velocity
		if (zeroVelocity) {
			rigid.velocity = Vector3.zero;
		}

		// Set mode
		mode = newMode;

		// Set gravity scale
		if(gravity != 99) {
			rigid.gravityScale = gravity;
		}

        // Set Collider size
        switch (mode) {
			case ePlayerMode.idle:
				circleColl.radius = 0.35f;
				circleColl.offset = new Vector2(0, -0.15f);
				break;
        }
	}

	public void SetAnim(string animName = "Player_Idle", int animSpeed = 1) {
        anim.speed = animSpeed;
        anim.CrossFade(animName, 0);
    }
}