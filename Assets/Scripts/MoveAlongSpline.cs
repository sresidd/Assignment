using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    private TrainInput trainInput;
    public TrainInput.TrainControllerActions trainControllerActions;
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float retardationMultiplier = 4f;
    [SerializeField] private float distancePercentage = 0f;
    private float velocity;
    private float splineLength;
    public float direction { get; private set; } = 1f; // 1 for clockwise, -1 for counterclockwise
    private bool isChangingDirection = false;
    private bool isDecelerating = false;
    private float targetDirection; // Stores the new direction after deceleration

    private void Awake()
    {
        trainInput = new TrainInput();
        trainControllerActions = trainInput.TrainController;

        Debug.Log($"{trainInput}, {trainControllerActions}");
    }

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    void FixedUpdate()
    {
        CheckInput();
        UpdateVelocity();

        distancePercentage += direction * velocity * Time.deltaTime / splineLength;

        // Keep distancePercentage within the range [0, 1]
        if (distancePercentage > 1f) distancePercentage -= 1f;
        if (distancePercentage < 0f) distancePercentage += 1f;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.01f);
        Vector3 directionVector = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(directionVector, transform.up);
    }

    private void CheckInput()
    {
        float newDirection = direction;
        bool isInputDetected = false;

        if (Input.GetKey(KeyCode.A))
        {
            newDirection = -1f;
            isInputDetected = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newDirection = 1f;
            isInputDetected = true;
        }

        // If direction is changing, set the target direction and start decelerating
        if (newDirection != direction && !isChangingDirection)
        {
            targetDirection = newDirection;
            isChangingDirection = true;
        }

        // If no input is detected, start decelerating
        if (!isInputDetected && velocity > 0)
        {
            isDecelerating = true;
        }
        else
        {
            isDecelerating = false;
        }
    }

    private void UpdateVelocity()
    {
        if (isChangingDirection)
        {
            // Apply deceleration when changing direction
            velocity -= acceleration * retardationMultiplier * Time.deltaTime;
            if (velocity <= 0)
            {
                // Once velocity reaches zero, switch to the new direction and start accelerating
                velocity = 0;
                direction = targetDirection;
                isChangingDirection = false;
            }
        }
        else if (isDecelerating)
        {
            // Decelerate when no input is detected
            velocity -= acceleration * retardationMultiplier * Time.deltaTime;
            if (velocity < 0)
            {
                velocity = 0;
            }
        }
        else
        {
            // Apply acceleration
            velocity += acceleration * Time.deltaTime;
            if (velocity >= maxVelocity)
            {
                velocity = maxVelocity;
            }
        }
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
