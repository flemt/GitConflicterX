using System.Diagnostics;


namespace GitConflicterX
{
    internal static class Tool
    {
        public static void ExecuteGitCommands()
        {
            string currentBranch = GetCurrentBranch();

            ExecuteGitCommand("checkout master");
            ExecuteGitCommand("pull");
            ExecuteGitCommand($"checkout {currentBranch}");
            ExecuteGitCommand("merge master");
            ExecuteGitCommand("mergetool");
        }

        private static void ExecuteGitCommand(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = "git",
                WorkingDirectory = Environment.CurrentDirectory,
                Arguments = arguments,
            };
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }

        private static string GetCurrentBranch()
        {
            ProcessStartInfo processStartInfo = null;

            Process branchProcess = Process.Start(processStartInfo);
            branchProcess.WaitForExit(TimeSpan.FromMilliseconds(1_500).Milliseconds);
            if (!branchProcess.HasExited || branchProcess.ExitCode == 128)
            {
                return string.Empty;
            }
            using (System.IO.StreamReader streamReader = branchProcess.StandardOutput)
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith('*'))
                    {
                        //we've got our branch name
                        return line.Replace("* ", "");
                    }
                }
            }
            return string.Empty;
        }
    }
}