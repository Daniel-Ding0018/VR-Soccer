using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    // store the right foot for kicking actions
    public GameObject RightShoe;
    public Vector3 PenaltyPosition;
    public Quaternion PenaltyRotation;
    public Vector3 PenaltyBallPosition;
    public Vector3 FreeKickPosition;
    public Quaternion FreeKickRotation;
    public Vector3 FreeKickBallPosition;
    

    // Start is called before the first frame update
    protected Animator animator;
    protected int isWalkingHash;
    protected int isRunningHash;
    protected int isLaceKickingHash;
    protected int isSideKickingHash;

    // Init player input
    PlayerInput input;

    // variable to store player input
    Vector2 currentMovement;
    protected bool movementPressed;
    protected bool runPressed;
    protected bool sideKickPressed;
    protected bool laceKickPressed;
    protected bool randomisePressed;
    protected bool targetDecreasePressed;
    protected bool moveTargetPressed;
    /*protected bool targetIncreasePressed;*/
    protected bool targetSpeedIncreasePressed;
    protected bool targetSpeedDecreasePressed;

    protected bool zoomInPressed;
    protected bool zoomOutPressed;
    protected bool slowGamePressed;
    private float slowdownFactor = 0.75f;
    private float currentTimeScale = 1;

    // string to store restart button pressed
    bool restartPressed;

    // varied distance bool
    bool setPiece1Pressed;
    bool setPiece2Pressed;

    // reference to ball and camera
    public GameObject ball;
    public Rigidbody ballRigidBody;

    // reference to the goal targets
    public TargetController targetController;

    // reference to main camera
    public Camera mainCamera;
    
    // Awake is called when script is being loaded
    private void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.IncreaseTargetMovement.performed += ctx =>
        {
            targetSpeedIncreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.DecreaseTargetMovement.performed += ctx =>
        {
            targetSpeedDecreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.Move.performed += ctx => {
            onMovementInput(ctx);
        };

        input.CharacterControls.Move.canceled += ctx => {
            onMovementInput(ctx);
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Run.canceled += ctx => runPressed = ctx.ReadValueAsButton();


        input.CharacterControls.SideKick.performed += ctx =>
        {
            sideKickPressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.LaceKick.performed += ctx =>
        {
            laceKickPressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.Restart.performed += ctx =>
        {
            restartPressed = ctx.ReadValueAsButton();
        };


        // input for changing distances
        input.CharacterControls.SetPiece1.performed += ctx =>
        {
            setPiece1Pressed = ctx.ReadValueAsButton();

        };

        input.CharacterControls.SetPiece2.performed += ctx =>
        {
            setPiece2Pressed = ctx.ReadValueAsButton();
        };

        // input for altering targets

        input.CharacterControls.MoveTarget.performed += ctx =>
        {
            moveTargetPressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.RandomiseTarget.performed += ctx =>
        {
            randomisePressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.DecreaseTargetSize.performed += ctx =>
        {
            targetDecreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.Zoomin.performed += ctx =>
        {
            zoomInPressed = ctx.ReadValueAsButton();
        };
        input.CharacterControls.Zoomout.performed += ctx =>
        {
            zoomOutPressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.SlowGame.performed += ctx =>
        {
            slowGamePressed = ctx.ReadValueAsButton();
        };
    }
    void Start()
    {
        // Get the animator component within the Unity Project
        animator = GetComponent<Animator>();
        // Converts string type to its own type
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isSideKickingHash = Animator.StringToHash("isSideKicking");
        isLaceKickingHash = Animator.StringToHash("isLaceKicking");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ad hoc fix to prevent player from sinking
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0;
        transform.position = currentPosition;

        // call handlers for all inputs
        handleRotation();
        handleMovement();
        handleShoot();
        handleRestart();
        HandleInitialPosition();
        handleRandomise();
        handleTargetSizeDecrease();
        handleTargetMovement();
        /*handleTargetSizeIncrease();*/

        handleTargetSpeed();
        handleZoom();
        handleSlowGame();
        

    }

    void onMovementInput(InputAction.CallbackContext ctx)
    {
        currentMovement = ctx.ReadValue<Vector2>();
        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    }
    

    void handleRestart()
    {
        if (restartPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            resetTimeScale();
            
        }
    }

    protected virtual void handleShoot()
    {
        if(sideKickPressed)
        {
            Debug.Log("Side Kick Pressed");
            RightShoe.GetComponent<KickBall>().enableSideKicking();
            animator.SetBool(isSideKickingHash, true);
        }
        if(!sideKickPressed)
        {
            RightShoe.GetComponent<KickBall>().disableSideKicking();
            animator.SetBool(isSideKickingHash, false);
        }
        if(laceKickPressed)
        {
            Debug.Log("Lace Kick Pressed");
            RightShoe.GetComponent<KickBall>().enableLaceKicking();
            animator.SetBool(isLaceKickingHash, true);
        }
        if (!laceKickPressed)
        {
            RightShoe.GetComponent<KickBall>().disableLaceKicking();
            animator.SetBool(isLaceKickingHash, false);
        }
    }
    
    void handleRotation()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }
    void handleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        /*
        Debug.Log("Is running is " + isRunning);
        Debug.Log("Is walking is " + isWalking);
        Debug.Log("Movement pressed is " + movementPressed);
        Debug.Log("Running pressed is " + runPressed);
        */
        /*
        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        
        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning) 
        {
            animator.SetBool(isRunningHash, false);
        }

        */

        if (movementPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }
        if (!runPressed)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    private void HandleInitialPosition()
    {
        if (setPiece1Pressed)
        {
            ball.transform.position = FreeKickBallPosition;
            ballRigidBody.velocity = new Vector3(0, 0, 0);
            transform.position = FreeKickPosition;
            transform.rotation = FreeKickRotation;
            
            setPiece1Pressed = false;
        }

        if (setPiece2Pressed)
        {
            ball.transform.position = PenaltyBallPosition;
            ballRigidBody.velocity = new Vector3(0, 0, 0);
            transform.position = PenaltyPosition;
            transform.rotation = PenaltyRotation;
            setPiece2Pressed = false;
        }

    }
    
    void handleRandomise()
    {
        // Do the ranodmising here
        
        if (randomisePressed)
        {
            targetController.randomiseTargets();
            randomisePressed = false;
        }
    }

    void handleTargetSpeed()
    {
        if (targetSpeedIncreasePressed)
        {
            targetController.increaseTargetMovementSpeed();
            targetSpeedIncreasePressed = false;
        }

        if (targetSpeedDecreasePressed)
        {
            targetController.decreaseTargetMovementSpeed(); 
            targetSpeedDecreasePressed = false;
        }
    }


    void handleTargetMovement()
    {
        if (moveTargetPressed)
        {
            targetController.moveTarget();
        }
    }
    
    void handleTargetSizeDecrease()
    {
        // Do the target decreasing here
        if (targetDecreasePressed)
        {
            
            targetController.ShrinkTargets();
            targetDecreasePressed = false;
        }
        
    }
    /*void handleTargetSizeIncrease()
    {
        // Do the target decreasing here
        if (targetIncreasePressed)
        {
            
            targetController.ExpandTargets();
            targetIncreasePressed= false;
        }
        
    }*/

    private void OnEnable()
    {
        // enable character action map
        input.CharacterControls.Enable();

    }
    

    private void OnDisable()
    {
        // enable character action map
        input.CharacterControls.Disable();
    }

    void handleZoom()
    {
        if (zoomInPressed)
        {
            Debug.Log("Zoom in");

            mainCamera.fieldOfView = 40;
            zoomInPressed = false;
        }

        if (zoomOutPressed)
        {
            Debug.Log("Zoom Out");
            mainCamera.fieldOfView = 80;
            zoomOutPressed = false;
        }
    }

    private void setTimeScale()
    {
        currentTimeScale = slowdownFactor * currentTimeScale;
        Debug.Log(slowdownFactor);
        Time.timeScale = currentTimeScale;
        Debug.Log(currentTimeScale);
        Time.fixedDeltaTime = currentTimeScale * 0.02f;
    }

    private void resetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    
    void handleSlowGame()
    {
        if (slowGamePressed)
        {
            Debug.Log("slow Game");

            setTimeScale();
            slowGamePressed = false;
        }
    }
}


