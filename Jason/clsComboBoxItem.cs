// This class facilitates assignment of items to a combo box such that both the
// display value and a related value are stored for each item.

public class clsComboBoxItem
{
    public string Text { get; set; }
    public object Value { get; set; }

    public override string ToString()
    {
        return Text;
    }
}