namespace Save_Load
{
    public abstract class SaveLoad
    {
        public abstract void Save<T>(T data);

        public abstract bool Load<T>(out T data);
    }
}