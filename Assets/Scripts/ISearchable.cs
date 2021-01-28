internal interface ISearchable
{
    T Find<T>(T[] typeArray, string name);
}