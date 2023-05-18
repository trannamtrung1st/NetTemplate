namespace NetTemplate.Common.IO
{
    public static class DirectoryInfoExtensions
    {
        public static bool IsSubDirectoryOf(this DirectoryInfo dir, DirectoryInfo another)
        {
            while (dir.FullName.StartsWith(another.FullName) && dir.Parent != null)
            {
                dir = dir.Parent;

                if (dir.FullName == another.FullName)
                    return true;
            }

            return false;
        }
    }
}
