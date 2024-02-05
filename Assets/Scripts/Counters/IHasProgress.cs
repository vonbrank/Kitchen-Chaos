using System;

namespace Counters
{
    public interface IHasProgress
    {
        public event EventHandler<ProgressChangedEventArgs> OnProgressChanged;

        public class ProgressChangedEventArgs : EventArgs
        {
            public float progressNormalized;
        }
    }
}