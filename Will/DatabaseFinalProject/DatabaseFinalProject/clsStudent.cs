using System;

class clsStudent
{
    private int mStudentID;
    private string mLastName;
    private string mFirstName;
    private string mSchool;
    private string mCity;
    private string mState;
    private int mZip;
    

    public clsStudent()
    {
        mStudentID = 0;
        mFirstName = "";
        mLastName = "";
        mSchool = "";
        mCity = "";
        mState = "";
        mZip= 0;
        
    }
    public clsStudent(int studentID, string lastName, string firstName, string school, string city, string state, int zip)
    {
        mStudentID = studentID;
        mFirstName = firstName;
        mLastName = lastName;
        mSchool = school;
        mCity = city;
        mState = state;
        mZip = zip;
        
    }
    public int UnivID
    {
        get
        {
            return mStudentID;
        }
        set
        {
            mStudentID = value;
        }
    }
    public string FirstName
    {
        get
        {
            return mLastName;
        }
        set
        {
            mLastName = value;
        }
    }
    public string LastName
    {
        get
        {
            return mLastName;
        }
        set
        {
            mLastName = value;
        }
    }
    public string Street
    {
        get
        {
            return mSchool;
        }
        set
        {
            mSchool = value;
        }
    }
    public string City
    {
        get
        {
            return mCity;
        }
        set
        {
            mCity = value;
        }
    }
    public string State
    {
        get
        {
            return mState;
        }
        set
        {
            mState = value;
        }
    }

    public int Zip
    {
        get
        {
            return mZip;
        }
        set
        {
            mZip = value;
        }
    }

    }
    public string ShowStudent()
    {
        return mStudentID.ToString().PadRight(10) + "    "
            + (mFirstName + " " + mLastName).PadRight(17) + "    "
            + mSchool.PadRight(20) + " "
            + mCity.PadRight(17) + "  "
            + mState.PadRight(7) + " "
            + mZip.ToString().PadRight(7);

    }
}