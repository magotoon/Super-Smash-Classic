using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharAttacks : MonoBehaviour {

    public abstract void UpTilt();

    public abstract void DownTilt();

    public abstract void SideTilt();

    public abstract void UpSmash();

    public abstract void DownSmash();

    public abstract void SideSmash();

    public abstract void UpAir();

    public abstract void DownAir();

    public abstract void ForwardAir();

    public abstract void BackAir();

    public abstract void NeutralAir();

    public abstract void Jab();

    public abstract void UpSpecial();

    public abstract void DownSpecial();

    public abstract void SideSpecial();

    public abstract void Special();
}
