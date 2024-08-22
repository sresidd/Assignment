using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private MoveAlongSpline moveAlongSpline;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam, Vector3.up);

        tMP_Text.text = moveAlongSpline.direction switch
        {
            1 => "Moving CW",
            -1 => "Moving CCW",
            _ => "----"
        };
    }
}
