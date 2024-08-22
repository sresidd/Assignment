using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    private TrainInput trainInput;
    private TrainInput.TrainControllerActions trainControllerActions;

    [SerializeField] private SplineContainer spline;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float retardationMultiplier = 4f;
    [SerializeField] private float distancePercentage = 0f;
    public float Direction { get; private set; } = 1f; 

    private float velocity;
    private float splineLength;
    private bool isChangingDirection;
    private bool isDecelerating;
    private float targetDirection;

    private void Awake()
    {
        trainInput = new TrainInput();
        trainControllerActions = trainInput.TrainController;
    }

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    private void FixedUpdate()
    {
        CheckInput();
        UpdateVelocity();
        Move();
    }

    private void Move()
    {
        distancePercentage = Mathf.Repeat(distancePercentage + Direction * velocity * Time.deltaTime / splineLength, 1f);
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);

        transform.position = currentPosition;

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.01f);
        Vector3 directionVector = (nextPosition - currentPosition).normalized;
        transform.rotation = Quaternion.LookRotation(directionVector, transform.up);
    }

    private void CheckInput()
    {
        bool isInputDetected = false;

        if (Input.GetKey(KeyCode.A))
        {
            SetDirection(-1f);
            isInputDetected = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetDirection(1f);
            isInputDetected = true;
        }

        isDecelerating = !isInputDetected && velocity > 0;
    }

    private void SetDirection(float newDirection)
    {
        if (newDirection != Direction && !isChangingDirection)
        {
            targetDirection = newDirection;
            isChangingDirection = true;
        }
    }

    private void UpdateVelocity()
    {
        if (isChangingDirection)
        {
            Decelerate();
            if (velocity <= 0)
            {
                velocity = 0;
                Direction = targetDirection;
                isChangingDirection = false;
            }
        }
        else if (isDecelerating)
        {
            Decelerate();
        }
        else
        {
            Accelerate();
        }
    }

    private void Decelerate()
    {
        velocity = Mathf.Max(0, velocity - acceleration * retardationMultiplier * Time.deltaTime);
    }

    private void Accelerate()
    {
        velocity = Mathf.Min(maxVelocity, velocity + acceleration * Time.deltaTime);
    }

    private void OnEnable()
    {
        trainInput.Enable();
        trainControllerActions.Enable();
    }

    private void OnDisable()
    {
        trainInput.Disable();
        trainControllerActions.Disable();
    }
}
