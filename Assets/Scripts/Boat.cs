﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Boat : MonoBehaviour
{
    public float Speed;
    public Transform Door;
    public Boat Prefab;

    private Rigidbody _body;
    private bool _stopped;
    private float _time = 8;
    private float oriY;

    private bool _ready;
    public bool Ready
    {
        get { return _ready; }
        set {
            _ready = value;
        }
    }

    public void Start()
    {
        Prefab = ((GameObject)Resources.Load("Boat")).GetComponent<Boat>();
        oriY = transform.position.y;
        _body = GetComponent<Rigidbody>();
        _stopped = false;
    }

    public void Update()
    {
        if (!Ready) return;
        if (!_stopped)
        {
            //_body.AddForce(0, 0, Speed - _body.velocity.z, ForceMode.VelocityChange);
            transform.position += Vector3.forward*Speed*Time.deltaTime;
        }
        else
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beach") && !_stopped) { 
            Destroy(Door.gameObject);
            ((Boat)Instantiate(Prefab, new Vector3(transform.position.x,oriY,transform.position.z) - Vector3.forward*256, Quaternion.Euler(0,0,90))).Ready = true;
            Timer.instance.StartTimer();
            var soldiers = new List<Transform>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                var t = transform.GetChild(i);
                if (t.CompareTag("Soldier")) soldiers.Add(t);
            }
            foreach (var soldier in soldiers)
            {
                if(soldier.GetComponent<Soldier>()!=null)soldier.GetComponent<Soldier>().Ready = true;
                soldier.parent = transform.parent;
            }
            _stopped = true;
        }
    }
}
