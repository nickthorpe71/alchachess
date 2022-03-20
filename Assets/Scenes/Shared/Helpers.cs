namespace Logic
{

    public static class Helpers
    {
        public static string GetClassAsStr<T>(T obj) => obj == null ? "null" : obj.GetType().Name;
    }
}
