using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Basic_04
{
    public class ListWithChangedEvent : ArrayList
    {
        public event EventHandler Changed;
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed( this, e );
        }

        public override int Add( object value )
        {
            int i = base.Add( value );
            OnChanged( EventArgs.Empty );
            return i;
        }
        public override void Clear()
        {
            base.Clear();
            OnChanged( EventArgs.Empty );
        }
        public override object this[int index]
        {
            set
            {
                base[index] = value;
                OnChanged( EventArgs.Empty );
            }
        }
    }
}

namespace TestEvents
{
    using Event_Basic_04;
    class EventListener
    {
        private ListWithChangedEvent List;

        public EventListener(ListWithChangedEvent list)
        {
            List = list;
            List.Changed += new EventHandler( ListChanged );           
        }
        // This will be called whenever the list changes
        private void ListChanged(object sender, EventArgs e)
        {
            Console.WriteLine( "This is called when the event fires." );
        }
        public void Detach()
        {
            // Detach the event and delete the list
            List.Changed -= new EventHandler( ListChanged );
            List = null;
        }
    }
    class Test
    {
        public static void Main()
        {
            // Create a new list
            ListWithChangedEvent list = new ListWithChangedEvent();

            // Create a class that listens to the list's change event
            EventListener listener = new EventListener( list );

            // Add and remove items from the list
            list.Add( "item 1" );
            list.Clear();
            listener.Detach();
        }
    }
}