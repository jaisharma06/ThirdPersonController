using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInCircle : MonoBehaviour
{
    public float m_rotationSpeed = 1f;
    public float m_radius = 1f;

    private Vector2 _center;
    private float _angle;

    // Start is called before the first frame update
    void Start()
    {
        _center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _angle += m_rotationSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * m_radius;

        transform.position = _center + offset;
    }
}
