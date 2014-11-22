using System;
using System.Collections;
public class clsSortLFName : IComparer
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        string client1Name = client1.LastName + client1.FirstName;
        string client2Name = client2.LastName + client2.FirstName;
        return client1Name.CompareTo(client2Name);
    }
}

public class clsSortLName : IComparer
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        return client1.LastName.CompareTo(client2.LastName);
    }
}

public class clsSortFName : IComparer
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        return client1.FirstName.CompareTo(client2.FirstName);
    }
}

public class clsSortStartWeight : IComparer // decreasing starting weights
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        return client2.StartWeight.CompareTo(client1.StartWeight);
    }
}

public class clsSortGoalWeight : IComparer  // decreasing goal weights
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        return -(client1.GoalWeight.CompareTo(client2.GoalWeight));
    }
}

public class clsSortWeeks : IComparer
{
    int IComparer.Compare(object object1, object object2)
    {
        clsClients client1 = (clsClients)object1;
        clsClients client2 = (clsClients)object2;
        return client1.Weeks.CompareTo(client2.Weeks);
    }
}
