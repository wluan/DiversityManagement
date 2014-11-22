public class clsComboBoxItem
{
    public string Text { get; set; }
    public object Value { get; set; }

    public override string ToString()
    {
        return Text;
    }
    public string FirstName()
    {
        string[] temp = Text.Split(' ');
        return temp[0];
    }
    public string LastName()
    {
        string[] temp = Text.Split(' ');
        return temp[1];
    }
}