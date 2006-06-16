using System.Reflection;
using System.Resources;

namespace Ferda.Guha
{
    public static class Resources
    {
        public const string defaultLocale = "en-US";

        public static ResourceManager GetResource(string[] localePrefs, string resourceIdentifier, Assembly assembly)
        {
            if (localePrefs != null && localePrefs.Length != 0)
                foreach (string var in localePrefs)
                {
                    try
                    {
                        return new ResourceManager(
                            resourceIdentifier + var,
                            assembly
                            );
                    }
                    catch
                    {
                    }
                }

            return new ResourceManager(
                resourceIdentifier + defaultLocale,
                assembly
                );
        }
    }
}