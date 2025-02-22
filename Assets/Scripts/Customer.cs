using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private enum Status { InQueue, WalkingToMachine, WorkingOut, Finished }
    
    [SerializeField] private Machine targetMachine;

    private MoneyManager moneyManager;
    private CustomerManager customerManager;
    private Transform exitLocation;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;

    private Status customerStatus = Status.InQueue;

    void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        customerManager = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
        exitLocation = GameObject.Find("ExitLocation").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.Play("Idle_A");
    }

    void Update()
    {
        // if status is walking and agent.remainingdistance = 0, start working out (position customer in front of machine and play animation)
        if (customerStatus == Status.WalkingToMachine && !agent.pathPending && agent.remainingDistance == 0)
        {
            agent.ResetPath();
            transform.position = targetMachine.gameObject.transform.position;
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
            customerManager.RemoveCustomer(gameObject);
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

        targetMachine.DamageMachine(4);
        targetMachine.DirtyMachine(6);
        targetMachine.FreeMachine();
    }

    public void CustomerWalkToMachine(Machine machine)
    {
        targetMachine = machine;
        agent.SetDestination(machine.gameObject.transform.position + new Vector3(0, 0, -1));
        customerStatus = Status.WalkingToMachine;
        animator.Play("Walk");
    }

    public void WalkToLocation(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
