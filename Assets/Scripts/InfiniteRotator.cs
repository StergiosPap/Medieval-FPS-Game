using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotator : MonoBehaviour
{
    public Transform transformToRotate;
    [Range(1f,100f)]
    public float RotationSpeed = 0.1f;
    public bool RotateOnStart = true;
    private bool isRotating = false;
    private bool isInside = false;
    public GameObject switchPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (RotateOnStart)
        {
            StartRotating();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            isRotating = !isRotating;
        }


        if (isRotating)
        {
            float angle = RotationSpeed * Time.deltaTime;
            transformToRotate.Rotate(0f, 0f, angle);
        }
    }

    public void StartRotating()
    {
        isRotating = true;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            isInside = true;
            switchPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        isInside = false;
        switchPanel.SetActive(false);
    }
}
