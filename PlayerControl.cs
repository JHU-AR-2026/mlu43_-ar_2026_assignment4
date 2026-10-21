using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;
    public float speed = 0f;
    public float jumpForce = 0f;
    private bool isGrounded = false;
    private bool jumpQueued = false;
    public GameObject DisappearingWall;

    public TextMeshProUGUI numLeftText;
    public int numCubes = 10;
    private int cubesRemaining;
    public GameObject AllKnowingCapsuleofKnowledgeandPower;
    public Material PowerUpMaterial;
    private bool isPoweredup = false;
    public GameObject sphereDone;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI powerupText;

    public Button startButton;
    private bool gameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        cubesRemaining = numCubes;
        AllKnowingCapsuleofKnowledgeandPower.SetActive(false);
        
        gameOverText.gameObject.SetActive(false);
        powerupText.gameObject.SetActive(false);
        numLeftText.gameObject.SetActive(false);
        startButton.onClick.AddListener(StartGame); // start the game when you click the button
    }

    void StartGame()
    {
        rb.useGravity = true; // allow sphere to drop
        startButton.gameObject.SetActive(false); // hide the button
        gameStarted = true;
        numLeftText.gameObject.SetActive(true); // unhide the number of cubes left
        UpdateCountText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) { return; } // don't allow movement until the game starts
        if (isGrounded && jumpAction.triggered)
        {
            jumpQueued = true;
        }
    }

    private void FixedUpdate() { 
        if (!gameStarted) { return; } // don't allow movement until the game starts
       // can only jump if you're ready to jump
        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpQueued = false;
        }

        // update movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // see if it's on the floor
        {
            isGrounded = true;
        }

        if (collision.gameObject == DisappearingWall && isPoweredup) // if you hit the wall while powerful it disappears
        {
            DisappearingWall.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("Cube")) // if you hit a cube, it disappears and the count goes down
        {
            other.gameObject.SetActive(false);
            cubesRemaining--;
            UpdateCountText();
            if (cubesRemaining <= 0)
            {
                AllKnowingCapsuleofKnowledgeandPower.SetActive(true);
            }
        }
        
        if (other.gameObject == AllKnowingCapsuleofKnowledgeandPower) // if you hit the powerup, it disappears and you get powered up
        {
            AllKnowingCapsuleofKnowledgeandPower.SetActive(false);
            GetComponent<Renderer>().material = PowerUpMaterial;
            isPoweredup = true;
            powerupText.gameObject.SetActive(true);
            speed *= 2; // double the speed
        }


        if (other.gameObject == sphereDone) // if you hit the sphere, the game ends
        {
            gameOverText.gameObject.SetActive(true);
            Time.timeScale = 0f;
            GetComponent<Renderer>().enabled = false; // hide the ball
        }
    }

    void UpdateCountText() // update the text that shows how many cubes are left
    {
        numLeftText.text = (10 - cubesRemaining).ToString() + "/10";
    }
}
