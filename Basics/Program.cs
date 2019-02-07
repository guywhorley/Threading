using System;
using System.Diagnostics;
using System.Threading;

namespace Basics
{
    class Program
    {
        // One the "main" thread.
        static void Main(string[] args)
        {
            Thread t = new Thread(DoMyThing);
            t.Start();

            while (t.IsAlive)
            {
                Thread.Sleep(5000);
                Console.WriteLine($"T is blocked == {(t.ThreadState & System.Threading.ThreadState.WaitSleepJoin) != 0}");
                Console.WriteLine($"State: {t.ThreadState}; name={t.Name}; still processing...");
            }

            // wait for worker
            t.Join();

            // with the combination of waits and blocked threads, we should be 
            // waiting for a total of 45 seconds.
            Console.WriteLine($"Worker thread {t.Name} has completed.");
            Console.WriteLine("Done with main thead.");

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        static void WaitLongTime()
        {
            Thread.Sleep(15000);
        }

        static void DoMyThing()
        {
            // start a new thread to wait on
            Thread wait = new Thread(WaitLongTime);
            wait.Start();
            // do not progress until wait thread has completed.
            wait.Join();

            // now we can sleep for 30 seconds.
            Thread.Sleep(30000);

            var st = new StackTrace();
            var sf = st.GetFrame(1);
            Console.WriteLine($"Finished sleeping on thread {sf.GetMethod().Name}");
        }
    }
}
