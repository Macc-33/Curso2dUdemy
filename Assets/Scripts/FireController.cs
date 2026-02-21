using System;
using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private float offDuration;
    [SerializeField] private FireButtomController _fireButtomController;
    [Space]
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private bool isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        if(_fireButtomController == null)
        {
           Debug.LogWarning("no fire buttom asigned" + gameObject.name);
        }
        SetFire(true);
    }
    public void SwitchOffFire()
    {
        if (!isActive) return;
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(offDuration);
        SetFire(true);
    }

    private void SetFire(bool active)
    {
        _animator.SetBool("Active", active);
        _collider.enabled = active;
        isActive = active;
    }
}

