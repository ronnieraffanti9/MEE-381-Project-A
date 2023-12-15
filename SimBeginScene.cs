using Godot;
using System;
public partial class SimBeginScene : Node3D
{
    MeshInstance3D anchor;
    MeshInstance3D ball;
    SpringModel spring;
    Label keLabel;
    PendSim pend;
    double xA, yA, zA; // coords of anchor
    float length0; // natural length of pendulum
    float length; // length of pendulum
    double angle; // pendulum angle
    double angleInit; // initial pendulum angle
    double time;
    Vector3 endA; // end point of anchor
    Vector3 endB; // end point for pendulum bob
    // Called when the node enters the scene tree for the first time.

    UIPanelDisplay dataDisplay;
    public override void _Ready()
    {
        GD.Print("Hello MEE 381 in Godot!");
        xA = 0.0; yA = 1.2; zA = 0.0;
        anchor = GetNode<MeshInstance3D>("Anchor");
        ball = GetNode<MeshInstance3D>("Ball1");
        spring = GetNode<SpringModel>("SpringModel");
        endA = new Vector3((float)xA, (float)yA, (float)zA);
        anchor.Position = endA;
        keLabel = GetNode<Label>("KElabel");
        pend = new PendSim();
        length0 = length = 0.9f;
        spring.GenMesh(0.05f, 0.015f, length, 6.0f, 62);
        angleInit = Mathf.DegToRad(60.0);
        endB.X = endA.X + pend.xCoord;
        endB.Y = endA.Y + pend.yCoord;
        endB.Z = endA.Z + pend.zCoord;
        PlacePendulum(endB);
        //PlacePendulum((float)angle);
        time = 0.0;

        // set up the data display
		dataDisplay = GetNode<UIPanelDisplay>("Control/MarginContainer/DataDisplay");
		dataDisplay.SetNDisplay(4);
		dataDisplay.SetLabel(0,"Pendulum");
		dataDisplay.SetValue(0,"");
		dataDisplay.SetLabel(1,"Kin. Energy");
		dataDisplay.SetValue(1,"---");
		dataDisplay.SetLabel(2,"Pot. Energy");
		dataDisplay.SetValue(2,"---");
		dataDisplay.SetLabel(3,"Tot. Energy");
		dataDisplay.SetValue(3,"---");
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        dataDisplay.SetValue(1,(float)pend.kE);//Kinetic Energy
		dataDisplay.SetValue(2,(float)pend.pE);//Potental Energy
		dataDisplay.SetValue(3,(float)pend.totalE);//Total Energy

        endB.X = endA.X + pend.xCoord;
        endB.Y = endA.Y + pend.yCoord;
        endB.Z = endA.Z + pend.zCoord;
        PlacePendulum(endB);
        time += delta;
        GD.Print("Pot. Energy: " + Math.Round(pend.pE,2) + " Kin. Energy: " + Math.Round(pend.kE,2) + " Tot. energy: " + Math.Round(pend.totalE,2));
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        pend.Step(time, delta);
    }
    private void PlacePendulum(Vector3 endBB)
    {
        ball.Position = endBB;
        spring.PlaceEndPoints(endA, endB);
    }
}