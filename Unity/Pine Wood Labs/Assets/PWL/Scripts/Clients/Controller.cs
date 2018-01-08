using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using SimpleState;

public class Controller : MonoBehaviour
{
    public SocketIOClient socket;
    protected StateMachine sm;

    public Camera startCamera;
    public Camera tableCamera;
    public Racer racer;
    public GameObject raceCar;
    public GameObject tableCar;
    public List<RaceTrack> raceTracks;
    public List<Racer> raceCars;

    // Use this for initialization
    void Start()
    {
        this.racer.mesh = this.tableCar.GetComponent<MeshFilter>().mesh;
        this.UpdateRaceCar();
        this.InitSockets();
        this.InitStateMachine();
    }

    void InitSockets()
    {
        socket.On("updateCar", OnUpdateCar);
        socket.On("startRace", OnStartRace);
    }

    public void OnStartRace(Socket socket, Packet packet, params object[] args)
    {
        sm.changeState("race");
    }

    void AttemptFinishRace()
    {
        sm.changeState("finish");
    }

    void InitStateMachine()
    {
        sm = new StateMachine();

        State table = new State();
        table.name = "table";
        table.next_states = new List<string>() { "table", "race" }; // Include start to allow it to even start. Need to find a better way to handle that.
        table.enter = delegate (string previous) { this.ShowTable(); }; // Automatically advance out of start state.

        State race = new State();
        race.name = "race";
        race.next_states = new List<string>() { "finish" };
        race.enter = delegate (string previous) { this.StartRace(); };
        race.update = delegate (float dt) { this.SendStats(); };
        race.exit = delegate (string next) { this.FinishRace(); };

        State finish = new State();
        finish.name = "finish";
        finish.next_states = new List<string>() { "table", "race" };

        sm.states.Add(table);
        sm.states.Add(race);
        sm.states.Add(finish);
        sm.startState(table);
    }

    void SendStats()
    {
        socket.Emit("sendStats", this.racer.stats);
    }

    void FinishRace()
    {
        racer.GatherStats(false);
        socket.Emit("endRace");
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update(Time.deltaTime);
    }

    void StartRace()
    {
        this.UpdateRaceCar();
        racer.ResetStats();
        racer.GatherStats(true);

        for (int i = 0; i < raceTracks.Count; i++)
        {
            raceCars[i].transform.position = raceTracks[i].startingBlock.position;
            raceCars[i].transform.rotation = raceTracks[i].startingBlock.rotation;
            Rigidbody rb = raceCars[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        this.SwitchCamera(startCamera);
    }

    void UpdateRaceCar() {
        raceCar.GetComponent<MeshFilter>().mesh = this.racer.mesh;
        raceCar.GetComponent<MeshCollider>().sharedMesh = this.racer.mesh;
        raceCar.GetComponent<Rigidbody>().mass = this.racer.mass;
    }

    void ShowTable()
    {
        this.SwitchCamera(tableCamera);
    }

    void SwitchCamera(Camera c)
    {
        // reset cameras
        foreach (Camera camera in Camera.allCameras)
        {
            camera.gameObject.SetActive(false);
        }
        c.gameObject.SetActive(true);
    }

    public void OnUpdateCar(Socket socket, Packet packet, params object[] args)
    {
        // Should be its own decoder.
        List<Vector2> points = new List<Vector2>();
        List<object> list = args[0] as List<object>;
        foreach (Dictionary<string, object> dict in list)
        {
            float x = Convert.ToSingle(dict["x"]);
            float y = Convert.ToSingle(dict["y"]);
            points.Add(new Vector2(x, y));
        }

        this.racer.UpdateMesh(points);

        MeshFilter mf = tableCar.GetComponent<MeshFilter>();
        mf.mesh = this.racer.mesh;

        sm.changeState("table");
    }
}
