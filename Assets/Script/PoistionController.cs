using UnityEngine;

[ExecuteInEditMode]
public class PoistionController : MonoBehaviour
{
    public Transform fromPostion;

    void LateUpdate()
    {
        transform.position = fromPostion.position;
    }
}
