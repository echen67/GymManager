using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private CharacterController characterController;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        characterController.Move(Vector3.Normalize(move) * Time.deltaTime * speed);

        // Rotate player to face right direction
        if(move.Equals(new Vector3(1,0,0)))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        } else if (move.Equals(new Vector3(-1,0,0)))
        {
            transform.eulerAngles = new Vector3(0, -90,0);
        } else if (move.Equals(new Vector3(0,0,1)))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (move.Equals(new Vector3(0,0,-1)))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // Play animation and sounds
        if (move.Equals(Vector3.zero))
        {
            animator.Play("Idle_A");
            audioSource.Stop();
        } else
        {
            animator.Play("Walk");
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }
    }

    private void LateUpdate()
    {
        // Don't love this but the Rigidbody's freeze position wasn't working
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
