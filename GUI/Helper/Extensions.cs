namespace Filewatcher.GUI
{
    internal static class Extensions
    {
        internal static string Alt(this string str1, string str2)
        {
            return !string.IsNullOrEmpty(str1) ? str1 : str2;
        }
    }
}
