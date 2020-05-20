using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPiece : MonoBehaviour {

    private string ID = "0-0";
    private string type = "Wasteland";
    private int resourceCount = 0;
    private string owner = "None";
    private int rH, rM, rL, tH, tM, tL, lH, lM, lL, bH, bM, bL; // Number of Heavy, Medium, Light, mechs at the Right, Top, Left, Bottom. (value is STRENGTH)
    private int rP= 50, tP = 50, lP = 50, bP = 50; // Progress for all 4 sides.

    private char buildingType; // f==factory, t==town, m==mine
    private Factory factory;
    //private Town town;
    //private Mine mine;

    public void setID(string _id){this.ID = _id;}
    public string getID(){return ID;}
    public void setType(string _type) { this.type = _type; }
    public string getType() { return type; }
    public void setResourceCount(int _resourceCount) { this.resourceCount = _resourceCount; }
    public int getResourceCount() { return resourceCount; }
    public void setOwner(string _owner) { this.owner = _owner; }
    public string getOwner() { return owner; }

    public int getRightProgress() { return rP; }
    public int getTopProgress() { return tP; }
    public int getLeftProgress() { return lP; }
    public int getBottomProgress() { return bP; }
    public int totalStrength() { return rH + rM + rL + tH + tM + tL + lH + lM + lL + bH + bM + bL; }
    public int rightStrength() { return rH + rM + rL; }
    public int topStrength() { return tH + tM + tL; }
    public int leftStrength() { return lH + lM + lL; }
    public int bottomStrength() { return bH + bM + bL; }

    public int getrH() { return rH; }
    public void setrH(int _rH) { rH = _rH; }
    public int getrM() { return rM; }
    public void setrM(int _rM) { rM = _rM; }
    public int getrL() { return rL; }
    public void setrL(int _rL) { rL = _rL; }
    public int gettH() { return tH; }
    public void settH(int _tH) { tH = _tH; }
    public int gettM() { return tM; }
    public void settM(int _tM) { tM = _tM; }
    public int gettL() { return tL; }
    public void settL(int _tL) { tL = _tL; }
    public int getlH() { return lH; }
    public void setlH(int _lH) { lH = _lH; }
    public int getlM() { return lM; }
    public void setlM(int _lM) { lM = _lM; }
    public int getlL() { return lL; }
    public void setlL(int _lL) { lL = _lL; }
    public int getbH() { return bH; }
    public void setbH(int _bH) { bH = _bH; }
    public int getbM() { return bM; }
    public void setbM(int _bM) { bM = _bM; }
    public int getbL() { return bL; }
    public void setbL(int _bL) { bL = _bL; }


    public void create(string[] data)
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

    public void AddFactory(Factory fact, string newOwner)
    {
        factory = fact;
        resourceCount = fact.GetCreditGain();
        Debug.Log("Gained resources: " + resourceCount);
        owner = newOwner;
        buildingType = 'f';

        // Testing only
        tP = (int)UnityEngine.Random.Range(0,100);
        bP = (int)UnityEngine.Random.Range(0, 100);
        lP = (int)UnityEngine.Random.Range(0, 100);
        rP = (int)UnityEngine.Random.Range(0, 100);

        tH = (int)UnityEngine.Random.Range(0, 50000);
        bH = (int)UnityEngine.Random.Range(0, 50000);
        lH = (int)UnityEngine.Random.Range(0, 50000);
        rH = (int)UnityEngine.Random.Range(0, 50000);
        // End of testing
    }

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
