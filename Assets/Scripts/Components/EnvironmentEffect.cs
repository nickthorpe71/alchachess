using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    private float targetHeight = 0;
    private float direction = 1;
    private bool moving = false;

    public void Raise()
    {
        SetMovement(1, 0, -1f);
    }

    public void Lower()
    {
        SetMovement(-1, -1f, 0);
    }

    private void SetMovement(float dir, float targetH, float startH)
    {
        transform.position = new Vector3(transform.position.x, startH, transform.position.z);
        direction = dir;
        targetHeight = targetH;
        moving = true;
    }

    private void StopMovement()
    {
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        moving = false;
    }

    private void Update()
    {
        if ((direction == -1 && transform.position.y <= targetHeight) || (direction == 1 && transform.position.y >= targetHeight))
            StopMovement();

        if (moving)
            transform.position += new Vector3(0, direction * Time.deltaTime, 0);
    }
}
