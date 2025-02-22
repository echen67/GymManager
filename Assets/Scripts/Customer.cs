using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private enum Status { InQueue, WalkingToMachine, WorkingOut, Finished }
    
    [SerializeField] private GameObject targetMachine;

    private MoneyManager moneyManager;
    private Transform spawnLocation;
    private Transform exitLocation;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    private Status customerStatus = Status.InQueue;

    void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        spawnLocation = GameObject.Find("SpawnLocation").transform;
        exitLocation = GameObject.Find("ExitLocation").transform;
        targetMachine = GameObject.Find("Machine");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.Play("Idle_A");
    }

    void Update()
    {
        // if status is inqueue, setdestination to machine and update status to walking (later we will move this to customer manager)
        if (customerStatus == Status.InQueue)
        {
            agent.SetDestination(targetMachine.transform.position + new Vector3(0,0,-1));
            customerStatus = Status.WalkingToMachine;
            animator.Play("Walk");
        }

        // if status is walking and agent.remainingdistance = 0, start working out (position customer in front of machine and play animation)
        if (customerStatus == Status.WalkingToMachine && !agent.pathPending && agent.remainingDistance == 0)
        {
            agent.ResetPath();
            transform.position = targetMachine.transform.position;
            transform.eulerAngles = new Vector3(0, 90, 0);
            animator.Play("Bounce");
            customerStatus = Status.WorkingOut;
            audioSource.Play();
            StartCoroutine(Workout());
        }

        // if status is finished and remaining destination = 0, despawn and add money
        if (customerStatus == Status.Finished && !agent.pathPending && agent.remainingDistance == 0)
        {
            moneyManager.AddMoney(10);
            Destroy(gameObject);
        }
    }

    IEnumerator Workout()
    {
        yield return new WaitForSeconds(5);

        audioSource.Stop();
        customerStatus = Status.Finished;
        animator.Play("Walk");
        agent.SetDestination(exitLocation.position);
    }
}
