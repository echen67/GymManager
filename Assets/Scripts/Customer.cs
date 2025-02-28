using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private enum Status { InQueue, WalkingToMachine, WorkingOut, Finished }
    
    private Machine targetMachine;
    private RatingManager ratingManager;
    private MoneyManager moneyManager;
    private CustomerManager customerManager;
    private Transform exitLocation;
    private AudioSource audioSource;
    private Transform mainCam;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas satisfactionCanvas;
    [SerializeField] private Image satisfactionUI;

    private Status customerStatus = Status.InQueue;
    private float currentSatisfaction = 100;
    private float maxSatifaction = 100;
    private float targetSatisfaction = 100;
    private float uiSpeed = 2f;
    private float satisfactionSpeed = 10f;

    void UpdateSatisfaction()
    {
        targetSatisfaction = currentSatisfaction / maxSatifaction;
    }
    void DecreaseSatisfaction(float decreaseAmount)
    {
        currentSatisfaction -= decreaseAmount;
        currentSatisfaction = Mathf.Max(currentSatisfaction, 0);
        UpdateSatisfaction();
    }
    void IncreaseSatisfaction(float increaseAmount)
    {
        currentSatisfaction += increaseAmount;
        currentSatisfaction = Mathf.Min(currentSatisfaction, maxSatifaction);
        UpdateSatisfaction();
    }

    void Start()
    {
        mainCam = GameObject.Find("Main Camera").transform;
        ratingManager = GameObject.Find("RatingManager").GetComponent<RatingManager>();
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
        // Keep customer satisfaction bar updated
        satisfactionUI.fillAmount = Mathf.MoveTowards(satisfactionUI.fillAmount, targetSatisfaction, uiSpeed * Time.deltaTime);
        satisfactionCanvas.transform.LookAt(mainCam, Vector3.up);

        // If customer is in queue, decrease satisfaction
        if (customerStatus == Status.InQueue)
        {
            DecreaseSatisfaction(satisfactionSpeed * Time.deltaTime);
        }

        // Once their satisfaction is depleted, the customer leaves and decreases gym rating
        if (customerStatus == Status.InQueue && currentSatisfaction == 0)
        {
            customerManager.RemoveCustomerFromQueue(gameObject);
            animator.Play("Walk");
            agent.SetDestination(exitLocation.position);
            customerManager.ShiftCustomersUpQueue();
        }
        if (customerStatus == Status.InQueue && currentSatisfaction == 0 && !agent.pathPending && agent.remainingDistance == 0)
        {
            ratingManager.DecreaseRating(10);
            Destroy(gameObject);
        }

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
            ratingManager.IncreaseRating(10);
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
        IncreaseSatisfaction(satisfactionSpeed * 2);
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
