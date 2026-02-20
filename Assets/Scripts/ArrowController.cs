using System;
using System.Collections;
using UnityEngine;

public class ArrowController : TrampolineController
{
    [Header("Adicional Settings")]
    [SerializeField] private bool rotacionRight;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float respwnTime;
    private int direction = -1;

    [SerializeField] private float scaleUpSpeed;
    [SerializeField] private Vector3 targetScale;



    private void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }



    private void Update()
    {
        HandleScaleUp();

        HandleRotation();
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        direction = rotacionRight ? 1 : -1;
        transform.Rotate(0, 0, (rotationSpeed * direction) * Time.deltaTime);
    }

    public void DestroyMe()
    {
        GameObject arrowPrefab = GameManager.instance.arrowPrefab;
        Vector3 respwnPosition = transform.position;
        GameManager.instance.CreateObject(arrowPrefab, respwnPosition,respwnTime);
        Destroy(gameObject);
    }



}

