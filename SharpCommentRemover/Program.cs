using System;
using System.IO;
using System.Text.RegularExpressions;

class CommentRemover
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: SharpCommentRemover <repository-directory>");
            return;
        }

        string repoDirectory = args[0];
        if (!Directory.Exists(repoDirectory))
        {
            Console.WriteLine($"Directory {repoDirectory} does not exist.");
            return;
        }

        ProcessDirectory(repoDirectory);
        Console.WriteLine("Comments removed successfully from all .cs files.");
    }

    static void ProcessDirectory(string directory)
    {
        // Process all .cs files in the directory and subdirectories
        string[] files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            ProcessFile(file);
        }
    }

    static void ProcessFile(string filePath)
    {
        string content = File.ReadAllText(filePath);

        // Define regex patterns for comments
        string singleLineCommentPattern = @"(?<!:)\/\/.*?$";   // Single-line comments
        string xmlDocCommentPattern = @"^\s*///.*?$";          // XML documentation comments
        string multiLineCommentPattern = @"/\*.*?\*/";         // Multi-line comments

        // Combine patterns
        string combinedPattern = $"{singleLineCommentPattern}|{xmlDocCommentPattern}|{multiLineCommentPattern}";

        // Remove comments
        string noComments = Regex.Replace(content, combinedPattern, "", RegexOptions.Singleline | RegexOptions.Multiline);

        // Save the modified content back to the file
        File.WriteAllText(filePath, noComments);
    }
}
