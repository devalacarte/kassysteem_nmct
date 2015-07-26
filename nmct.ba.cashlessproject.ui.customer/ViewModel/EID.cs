using be.belgium.eid;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace nmct.ba.cashlessproject.ui.customer.ViewModel
{
    public class EIDReader
    {
        public BEID_ReaderSet ReaderSet;
        public System.Collections.Hashtable MyReadersSet;
        public Timer TimerEIDChanged = new Timer(500);
        public EIDReader()
        {
            ReaderSet = BEID_ReaderSet.instance();
            MyReadersSet = new System.Collections.Hashtable();
            TimerEIDChanged = new Timer(500);
            TimerEIDChanged.Elapsed += TimerEIDChangedTicked;
            TimerEIDChanged.Enabled = true ;
            AttachEvents();
        }

        class ReaderRef
        {
            public BEID_ReaderContext reader;
            public uint eventHandle;
            public IntPtr ptr;
            public uint cardId;
        }

        public void AttachEvents()
        {
            try
            {
                BEID_ReaderContext reader;
                ReaderRef readerRef;
                uint i;

                BEID_SetEventDelegate MyCallback = new BEID_SetEventDelegate(CallBack);

                string readerName;
                BEID_ReaderSet ReaderSet = BEID_ReaderSet.instance();
                for (i = 0; i < ReaderSet.readerCount(); i++)
                {
                    reader = ReaderSet.getReaderByNum(i);
                    readerName = ReaderSet.getReaderName(i);

                    readerRef = new ReaderRef();

                    readerRef.reader = reader;
                    readerRef.ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName);
                    readerRef.cardId = 0;
                    MyReadersSet.Add(readerName, readerRef);
                    readerRef.eventHandle = reader.SetEventCallback(MyCallback, readerRef.ptr);
                    
                }

            }
            catch (BEID_Exception)
            {
                Console.WriteLine("Crash BEID_Exception!");
            }
            catch (Exception)
            {
                Console.WriteLine("Crash System.Exception!");
            }

        }

        public void DetachEvents()
        {
            try
            {
                BEID_ReaderContext reader;

                foreach (ReaderRef readerRef in MyReadersSet.Values)
                {
                    reader = readerRef.reader;
                    reader.StopEventCallback(readerRef.eventHandle);
                }
                MyReadersSet.Clear();
            }
            catch (BEID_Exception)
            {
                Console.WriteLine("Crash BEID_Exception!");
            }
            catch (Exception)
            {
                Console.WriteLine("Crash System.Exception!");
            }

        }

        public const string CARDINSERTED = "CARDINSERTED";
        public const string CARDREMOVED = "CARDREMOVED";
        public void CallBack(int lRe, uint lState, System.IntPtr p)
        {
            try
            {
                string action = "";
                string readerName;
                ReaderRef readerRef;
                bool bChange;
                bool cardinserted = false;
                bool cardremoved = false;
                readerName = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(p);
                readerRef = (ReaderRef)MyReadersSet[readerName];

                bChange = false;

                if (readerRef.reader.isCardPresent())
                {
                    if (readerRef.reader.isCardChanged(ref readerRef.cardId))
                    {
                        action = "inserted in";
                        bChange = true;
                        cardinserted = true;
                    }
                }
                else
                {
                    if (readerRef.cardId != 0)
                    {
                        action = "removed from";
                        bChange = true;
                        cardremoved = true;
                    }
                }

                if (bChange)
                    Console.WriteLine("A card has been " + action + " the reader : " + readerName);
                if(cardinserted)
                    Messenger.Default.Send(new NotificationMessage(CARDINSERTED));
                if(cardremoved)
                    Messenger.Default.Send(new NotificationMessage(CARDREMOVED));

            }
            catch (BEID_Exception)
            {
                Console.WriteLine("Crash BEID_Exception!");
            }
            catch (Exception)
            {
                Console.WriteLine("Crash System.Exception!");
            }
        }
        
        
        public void TimerEIDChangedTicked(object sender, EventArgs e)
        {
            try
            {
                uint count;
                if (BEID_ReaderSet.instance().isReadersChanged())
                {
                    DetachEvents();
                    count = BEID_ReaderSet.instance().readerCount(true); //Force the read of reader list
                    AttachEvents();

                    Console.WriteLine("Readers has been plugged/unplugged\r\nNumber of readers : " + count);
                }
            }
            catch (BEID_Exception)
            {
                Console.WriteLine("Crash BEID_Exception!");
            }
            catch (Exception)
            {
                Console.WriteLine("Crash System.Exception!");
            }
        }
    }
}
