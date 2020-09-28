using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class Level1Starter : Starter
{
    public override void StarterSetup()
    {
        GlobalSystemStorage.Add<ThreadManager>();
        GlobalSystemStorage.Add<CorutineManager>();
        GlobalSystemStorage.Add<TestProc>();
        //GlobalSystemStorage.Add<ProcessingExample4>();

        //GlobalSystemStorage.Add<ProcessingExample1>();
        //GlobalSystemStorage.Add<ProcessingExample2>();
        //GlobalSystemStorage.Add<ProcessingExample3>();

        GlobalSystemStorage.Add<ColisionDamageProcessing>();
        GlobalSystemStorage.Add<DelayDestroyProcessing>();
        GlobalSystemStorage.Add<MainCameraProcessing>();
        GlobalSystemStorage.Add<ForwardMoveProcessing>(); 

        GlobalSystemStorage.Add<GunProcessing>();
        GlobalSystemStorage.Add<PlayerControlProcessing>();
        GlobalSystemStorage.Add<ShipPhysicsProcessing>();
    }
}