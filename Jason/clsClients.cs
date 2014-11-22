using System;

public class clsClients
{
    // List of the class data members.

    private int mID;
    private string mFName;
    private string mLName;
    private int mAge;
    private double mHeight;
    private double mStartWeight;
    private double mGoalWeight;
    private int mWeeks;
    private double mStartBMI;
    private double mGoalBMI;

    // ----------- Class constructors ----------------

    public clsClients()
    {
        mFName = "";
        mLName = "";
        mAge = 0;
        mHeight = 0.0;
        mStartWeight = 0.0;
        mGoalWeight = 0.0;
        mWeeks = 0;
    }

    public clsClients(string myFName, string myLName, int myAge, double myHeight, double myStartWeight,
        double myGoalWeight, int myWeeks)
    {
        mFName = myFName;
        mLName = myLName;
        mAge = myAge;
        mHeight = myHeight;
        mStartWeight = myStartWeight;
        mGoalWeight = myGoalWeight;
        mWeeks = myWeeks;
    }

    public clsClients(int clientID, string myFName, string myLName, int myAge, double myHeight, double myStartWeight,
        double myGoalWeight, int myWeeks)
    {
        mID = clientID;
        mFName = myFName;
        mLName = myLName;
        mAge = myAge;
        mHeight = myHeight;
        mStartWeight = myStartWeight;
        mGoalWeight = myGoalWeight;
        mWeeks = myWeeks;
    }

    // ----------- Accessor methods for data members ----------------

    // Purpose: Reads or writes the mName data member.
    public int ID
    {
        get
        {
            return mID;
        }
        set
        {
            mID = value;
        }
    }

    public string FirstName
    {
        get
        {
            return mFName;
        }

        set
        {
            mFName = value;
        }
    }

    public string LastName
    {
        get
        {
            return mLName;
        }

        set
        {
            mLName = value;
        }
    }

    public int Age
    {
        get
        {
            return mAge;
        }

        set
        {
            mAge = value;
        }
    }

    public double Height
    {
        get
        {
            return mHeight;
        }

        set
        {
            mHeight = value;
        }
    }

    public double StartWeight
    {
        get
        {
            return mStartWeight;
        }

        set
        {
            mStartWeight = value;
        }
    }

    public double GoalWeight
    {
        get
        {
            return mGoalWeight;
        }

        set
        {
            mGoalWeight = value;
        }
    }

    public int Weeks
    {
        get
        {
            return mWeeks;
        }

        set
        {
            mWeeks = value;
        }
    }

    public double StartBMI
    {
        get
        {
            return mStartBMI;
        }

        set
        {
            mStartBMI = value;
        }
    }

    public double GoalBMI
    {
        get
        {
            return mGoalBMI;
        }

        set
        {
            mGoalBMI = value;
        }
    }


    // ----------- End Accessor methods for data members ------------

    // Purpose: Construct a line of output for this client.
    public string ShowClient()
    {
        string outputLine;
        string clientName = mFName + " " + mLName;
        mStartBMI = calcBMI(mHeight, mStartWeight);
        mGoalBMI = calcBMI(mHeight, mGoalWeight);

        // Construct the output line.
        outputLine = clientName.PadRight(16) + "  " +
            mAge.ToString().PadLeft(2) + "    " +
            mHeight.ToString("##.0").PadLeft(4) + "      " +
            mStartWeight.ToString("###.0").PadLeft(5) + "      " +
            mStartBMI.ToString("##.#0").PadLeft(5) + "     " +
            mGoalWeight.ToString("###.0").PadLeft(5) + "     " +
            mGoalBMI.ToString("##.#0").PadLeft(5) + "     " +
            mWeeks.ToString("#0").PadLeft(2);
        return outputLine;
    }

    // The calcBMI helper method computes and returns the BMI value, given height in inches and weight in 
    // pounds.
    public double calcBMI(double hgt, double wgt)
    {
        const int BMI_Factor = 703;
        return (wgt / (hgt * hgt)) * BMI_Factor;
    }


}