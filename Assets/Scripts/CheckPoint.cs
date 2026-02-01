using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isActive;
    private int idCheck;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       animator = GetComponent<Animator>();
      // GameManager.instance.hasCheckPointActive = false;
    }
    void Start()
    {
       idCheck = Animator.StringToHash("Check");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (isActive) return;
        if (collision.CompareTag("Player"))  ActiveCheckPoint(); 
        GameManager.instance.hasCheckPointActive = true;
        GameManager.instance.checkPointRespwnPosition = transform.position;
    }

    private void ActiveCheckPoint()
    {
        isActive = true;
        animator.SetTrigger(idCheck);
    }
}
