using UnityEngine;

[ExecuteInEditMode]
public class LineController : MonoBehaviour
{
    public LineRenderer line;
    public Transform pt;

    void Start()
    {
        line.positionCount = 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, pt.position);
    }
}
