using UnityEngine;

[ExecuteInEditMode]
public class Poistion : MonoBehaviour
{
    public Transform fromPostion;

    void LateUpdate()
    {
        transform.position = fromPostion.position;

    }
}
