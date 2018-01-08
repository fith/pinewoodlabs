using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();
    public Dictionary<string, object> stats = new Dictionary<string, object>();
    public Mesh mesh;
    public float depth = 0.2f;
    public float volume = 0.0f;
    public float defaultMass = 49.0f;
    public float mass = 50.0f;
    private bool gatherStats = false;
    public float statsInterval = 1.0f;
    private float lastStatsTime;
    private float startTime;

    public Mesh UpdateMesh(List<Vector2> points)
    {
        this.points = points;
        this.mesh = ExtrudeShape.Generate(this.points, this.depth);
        this.volume = MeshVolume.Calculate(mesh);
        this.mass = (defaultMass + this.volume * 100.0f);
        return this.mesh;
    }

    void Update()
    {
        if (gatherStats && Time.time > (lastStatsTime + statsInterval))
        {
            lastStatsTime = Time.time;
            float time = lastStatsTime - startTime;
            float speed = this.GetComponent<Rigidbody>().velocity.magnitude;
            stats.Add(time.ToString(), speed);
        }
    }

    public void ResetStats()
    {
        stats = new Dictionary<string, object>();
    }

    public void GatherStats(bool gather)
    {
        if (gather) {
            this.gatherStats = true;
            this.startTime = Time.time;
            this.lastStatsTime = Time.time;
        } else {
            this.gatherStats = false;
        }

    }
}
