using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace CPT
{
    public class FEITTimer
    {
        private Stopwatch mTimer;

        public List<long> mTracker;
        public List<string> mPointNote;

        public FEITTimer()
        {
            mTracker = new List<long>();
            mPointNote = new List<string>();

            mTimer = new Stopwatch();
        }

        public void Start()
        {
            mTimer.Start();
        }

        public void Stop()
        {
            mTimer.Stop();
        }

        public void Reset()
        {
            mTimer.Reset();
        }

        public void Dot(string note)
        {
            mTracker.Add(mTimer.ElapsedMilliseconds);
            mPointNote.Add(note);
        }

        public long GetElapsedTime()
        {
            return mTimer.ElapsedMilliseconds;
        }
    }
}
