using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float repairAmount = 2f;
    [SerializeField] private float cleanAmount = 4f;
    [SerializeField] private int repairCost = 5;
    [SerializeField] private int cleanCost = 5;

    private Animator animator;
    private AudioSource audioSource;
    private Machine selectedMachine;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Movement
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (!move.Equals(Vector3.zero))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Rotate player to face right direction
        if (move.Equals(new Vector3(1, 0, 0)))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (move.Equals(new Vector3(-1, 0, 0)))
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (move.Equals(new Vector3(0, 0, 1)))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (move.Equals(new Vector3(0, 0, -1)))
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

        // Repair or clean machine
        if (Input.GetKeyDown(KeyCode.R) && selectedMachine)
        {
            selectedMachine.RepairMachine(repairAmount, repairCost);
        }
        if(Input.GetKeyDown(KeyCode.C) && selectedMachine)
        {
            selectedMachine.CleanMachine(cleanAmount, cleanCost);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Machine")
        {
            Renderer machineRenderer = collision.gameObject.GetComponent<Renderer>();
            Material[] materials = machineRenderer.materials;
            foreach(Material mat in materials)
            {
                mat.SetColor("_EmissionColor", new Color(0, 0, .4f, 0.25f));
            }
            machineRenderer.materials = materials;
            selectedMachine = collision.gameObject.GetComponent<Machine>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Renderer machineRenderer = collision.gameObject.GetComponent<Renderer>();
        Material[] materials = machineRenderer.materials;
        foreach (Material mat in materials)
        {
            mat.SetColor("_EmissionColor", new Color(0, 0, .01f));
        }
        machineRenderer.materials = materials;
        if (selectedMachine && selectedMachine.Equals(collision.gameObject.GetComponent<Machine>()))
        {
            selectedMachine = null;
        }
    }
}
