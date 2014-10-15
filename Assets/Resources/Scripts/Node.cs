using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Node<T>
{
    private T item;
    private Node<T> next, prev;

    public Node()
    {

    }

    public Node(T item)
    {
        this.item = item;
    }

    //Getters and setters!
    public void SetItem(T item){   this.item = item;   }
    public T GetItem(){   return item;    }
    public void SetNext(Node<T> next){   this.next = next;   }
    public Node<T> GetNext(){   return next;    }
    public void SetPrev(Node<T> prev){   this.prev = prev;   }
    public Node<T> GetPrev(){   return prev;    }
}
