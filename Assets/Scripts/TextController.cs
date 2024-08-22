using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    // [SerializeField] private Transform cam;
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private MoveAlongSpline moveAlongSpline;
    [SerializeField] private Vector3 rotationSpeed;

    private void LateUpdate()
    {
        Vector3 rotationThisFrame = rotationSpeed * Time.deltaTime;
        
        transform.Rotate(rotationThisFrame);

        tMP_Text.text = moveAlongSpline.Direction switch
        {
            1 => "Moving CW",
            -1 => "Moving CCW",
            _ => "----"
        };
    }
}
