using System;

namespace Save_Load
{
    [Serializable]
    public abstract class AbstractSaveData<T>
    {
        public abstract void Insert(T handler);
    }
}