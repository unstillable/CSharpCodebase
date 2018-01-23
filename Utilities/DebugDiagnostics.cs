using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;


//used for debugging in visual studio
namespace Utilities
{
    public static class DebugDiagnostics
    {
        /// <summary>
        /// Prints a link to a file and line number to debug output window
        /// </summary>
        /// <param name="associatedErrorMessage">Whatever you want to find after the linked file and line in the output window</param>
        /// <param name="path">Attributed with CallerFilePath, always use default</param>
        /// <param name="line">Attributed with CallerLineNumber, always use default</param>
        public static void LinkPathInDebug(string associatedErrorMessage,
            [CallerFilePath] string path = "",
            [CallerLineNumber] int line = 0)
        {
            _tryParseRelative(ref path);
            Debug.WriteLine($"{path}({line}) : {associatedErrorMessage}");
        }
        /// <summary>
        /// truncates path to the relative path substring to the codebase
        /// </summary>
        /// <param name="path">File path in </param>
        /// <returns>If the path shares partial relative path with entry assembly dll</returns>
        private static void _tryParseRelative(ref string path)
        {
            var loc = System.Reflection.Assembly.GetEntryAssembly()  //using anywhere .AsParallel() will slow this down (too few elements)
                .CodeBase.Substring(8) //kill file:///
                .Select(a => a == '/' ? '\\' : a) //path and codebase use '//'vs '\'
                .Zip(path, (a, b) => a == b) //Zip to list of true false
                .TakeWhile(x => x) //take until the first false
                .Count(); //count members
            if (loc > 0)
            {
                path = path.Substring(loc);
            }
        }
    }
}
