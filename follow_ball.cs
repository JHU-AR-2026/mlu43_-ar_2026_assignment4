using UnityEngine;

public class follow_ball : MonoBehaviour
{
    public Transform ballTransform;
    private Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - ballTransform.position;


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ballTransform.position + offset;


    }
}
