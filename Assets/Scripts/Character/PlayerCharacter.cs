using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    [SerializeField]
    StateMachine machine;

    [SerializeField]
    protected CharacterController m_CharacterController2D;
    protected Animator m_Animator;
    protected CapsuleCollider2D m_Capsule;
    protected Transform m_Transform;
    protected Vector2 m_MoveVector;
   
    static protected PlayerCharacter s_PlayerInstance;
    static public PlayerCharacter PlayerInstance { get { return s_PlayerInstance; } }

    public SpriteRenderer spriteRenderer;
    public Transform facingLeftBulletSpawnPoint;
    public Transform facingRightBulletSpawnPoint;
    public Transform cameraFollowTarget;

    public float maxSpeed = 10f;
    public float groundAcceleration = 100f;
    public float groundDeceleration = 100f;

    [Range(0f, 1f)] public float airborneAccelProportion;
    [Range(0f, 1f)] public float airborneDecelProportion;
    public float gravity = 50f;
    public float jumpSpeed = 20f;
    public float jumpAbortSpeedReduction = 100f;

    //define the parameters for the different states
    protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
    protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash("VerticalSpeed");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
    protected readonly int m_HashCrouchingPara = Animator.StringToHash("Crouching");
    protected readonly int m_HashPushingPara = Animator.StringToHash("Pushing");
    protected readonly int m_HashTimeoutPara = Animator.StringToHash("Timeout");
    protected readonly int m_HashRespawnPara = Animator.StringToHash("Respawn");
    protected readonly int m_HashDeadPara = Animator.StringToHash("Dead");
    protected readonly int m_HashHurtPara = Animator.StringToHash("Hurt");
    protected readonly int m_HashForcedRespawnPara = Animator.StringToHash("ForcedRespawn");
    protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashHoldingGunPara = Animator.StringToHash("HoldingGun");


    protected TileBase m_CurrentSurface;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {

    }


    //called to move the character
    public void Move()
    {
        m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);
    }

    //move on the ground horizontally
    public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
    {
        float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * maxSpeed * speedScale : 0f;
        float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    //move horiztonally in air
    public void AirborneHorizontalMovement()
    {
        float desiredSpeed = PlayerInput.Instance.Horizontal.Value * maxSpeed;

        float acceleration;

        if (PlayerInput.Instance.Horizontal.ReceivingInput)
            acceleration = groundAcceleration * airborneAccelProportion;
        else
            acceleration = groundDeceleration * airborneDecelProportion;

        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void AirborneVerticalMovement()
    {
        if (Mathf.Approximately(m_MoveVector.y, 0f) || m_CharacterController2D.IsCeilinged && m_MoveVector.y > 0f)
        {
            m_MoveVector.y = 0f;
        }
        m_MoveVector.y -= gravity * Time.deltaTime;
    }

    public void SetHorizontalMovement(float newHorizontalMovement)
    {
        m_MoveVector.x = newHorizontalMovement;
    }

    public void SetVerticalMovement(float newVerticalMovement)
    {
        m_MoveVector.y = newVerticalMovement;
    }



    //checks
    public bool CheckForGrounded()
    {
        bool wasGrounded = m_Animator.GetBool(m_HashGroundedPara);
        bool grounded = m_CharacterController2D.IsGrounded;

        if (grounded)
        {
            FindCurrentSurface();

            if (!wasGrounded && m_MoveVector.y < -1.0f)
            {//only play the landing sound if falling "fast" enough (avoid small bump playing the landing sound)
            }
        }
        else
            m_CurrentSurface = null;

        m_Animator.SetBool(m_HashGroundedPara, grounded);

        return grounded;
    }

    public void FindCurrentSurface()
    {
        Collider2D groundCollider = m_CharacterController2D.GroundColliders[0];

        if (groundCollider == null)
            groundCollider = m_CharacterController2D.GroundColliders[1];

        if (groundCollider == null)
            return;

        TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
        if (b != null)
        {
            m_CurrentSurface = b;
        }
    }

    //input functions
    public bool CheckForJumpInput()
    {
        return PlayerInput.Instance.Jump.Down;
    }


}
