using System.Collections;

public class CircularQueue<T>
{
    Node<T> current;

    public CircularQueue()
    {

    }

    public CircularQueue(T item)
    {
        current = new Node<T>(item);
        current.SetNext(current);
        current.SetPrev(current);
    }

    //Returns the item stored in this node, and iterates to the next node
    public T Forward()
    {
        T output = current.GetItem();
        current = current.GetNext();
        return output;
    }

    public T Current()
    {
        return current.GetItem();
    }

    public T Reverse()
    {
        T output = current.GetItem();
        current = current.GetPrev();
        return output;
    }

    public void EnQueue(T item)
    {
        if (current == null)
        {
            current = new Node<T>(item);
            current.SetNext(current);
            current.SetPrev(current);
            return;
        }
        else
        {
            Node<T> temp = new Node<T>(item);
            temp.SetNext(current);
            temp.SetPrev(current.GetPrev());
            current.GetPrev().SetNext(temp);
            current.SetPrev(temp);
            current = temp;
            return;
        }
    }
}
