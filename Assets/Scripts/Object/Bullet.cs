using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;

    public float LifeTime = 3f;

    private void OnEnable() {
        Invoke(nameof(DestroySelf), LifeTime);
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * (Speed * Time.deltaTime));
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}