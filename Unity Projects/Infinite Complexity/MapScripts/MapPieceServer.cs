using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class mapPieceServer 
{
    public string ID = "0-0";
    public string type = "Wasteland";
    public int resourceCount = 0;
    public string owner = "None";
    public int rH, rM, rL, tH, tM, tL, lH, lM, lL, bH, bM, bL; // Number of Heavy, Medium, Light, mechs at the Right, Top, Left, Bottom. (value is STRENGTH)
    public int rP = 50, tP = 50, lP = 50, bP = 50; // Progress for all 4 sides.

    //public char bT; // f==factory, t==town, m==mine (buildingType)
    //   public FactoryServer f;
    // public TownServer t;
    // public MineServer m;

    /*
    public void setID(string _id) { this.ID = _id; }
    public string getID() { return ID; }
    public void setType(string _type) { this.type = _type; }
    public string getType() { return type; }
    public void setResourceCount(int _resourceCount) { this.resourceCount = _resourceCount; }
    public int getResourceCount() { return resourceCount; }
    public void setOwner(string _owner) { this.owner = _owner; }
    public string getOwner() { return owner; }
    */

    public mapPieceServer(string t, int r, string o, string i)
    {
        type = t;
        resourceCount = r;
        owner = o;
        ID = i;
    }
    public mapPieceServer(string t)
    {
        type = t;
    }
    public mapPieceServer(string t, int r)
    {
        type = t;
        resourceCount = r;
    }
    public mapPieceServer(string[] data)
    {
        type = data[0];
        resourceCount = Int32.Parse(data[1]);
        owner = data[2];
        ID = data[3];
        try
        {
            string[] strengths = data[4].Split('-');
            rH = Int32.Parse(strengths[0]);
            rM = Int32.Parse(strengths[1]);
            rL = Int32.Parse(strengths[2]);
            tH = Int32.Parse(strengths[3]);
            tM = Int32.Parse(strengths[4]);
            tL = Int32.Parse(strengths[5]);
            lH = Int32.Parse(strengths[6]);
            lM = Int32.Parse(strengths[7]);
            lL = Int32.Parse(strengths[8]);
            bH = Int32.Parse(strengths[9]);
            bM = Int32.Parse(strengths[10]);
            bL = Int32.Parse(strengths[11]);
        }
        catch (Exception e) { Debug.Log(e.StackTrace); }
        try
        {
            string[] progress = data[5].Split('-');
            rP = Int32.Parse(progress[0]);
            tP = Int32.Parse(progress[1]);
            lP = Int32.Parse(progress[2]);
            bP = Int32.Parse(progress[3]);
        }
        catch (Exception e) { Debug.Log(e.StackTrace); }

    }
    //   public mapPieceServer() { }


    public void create(string[] data)
    {
        type = data[0];
        resourceCount = Int32.Parse(data[1]);
        owner = data[2];
        ID = data[3];
    }

  /*  public void AddFactory(int resources, string newOwner)
    {
        resourceCount = resources;
        owner = newOwner;
    }*/

    public int totalStrength() { return rH + rM + rL + tH + tM + tL + lH + lM + lL + bH + bM + bL; }
    public int rightStrength() { return rH + rM + rL; }
    public int topStrength() { return tH + tM + tL; }
    public int leftStrength() { return lH + lM + lL; }
    public int bottomStrength() { return bH + bM + bL; }


    public string DataInfo()
    {
        return type + " " + resourceCount + " " + owner + " " + ID + " "
            + rH.ToString() + "-" + rM.ToString() + "-" + rL.ToString() + "-"
            + tH.ToString() + "-" + tM.ToString() + "-" + tL.ToString() + "-"
            + lH.ToString() + "-" + lM.ToString() + "-" + lL.ToString() + "-"
            + bH.ToString() + "-" + bM.ToString() + "-" + bL.ToString() + " "
            + rP.ToString() + "-" + tP.ToString() + "-" + lP.ToString() + "-" + bP.ToString();
    }

}