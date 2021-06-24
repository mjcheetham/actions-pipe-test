using System;
using System.Diagnostics;
using System.IO;

namespace actions_pipe_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = new byte[]
            {
                (byte)'P', (byte)'A', (byte)'C', (byte)'K',
                1, 2, 3, 4,
                5, 6, 7, 128,
            };

            Console.WriteLine("Data is:");
            for (int i = 0; i < data.Length; i++)
                Console.WriteLine(" [{0}]: {1}", i, data[i]);

            var psi = new ProcessStartInfo(args[0]);
            psi.WorkingDirectory = Environment.CurrentDirectory;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;

            using (var proc = new Process {StartInfo = psi})
            using (var ms = new MemoryStream(data))
            {
                Console.WriteLine("Piping data to: {0}", psi.FileName);
                proc.Start();
                Console.WriteLine("PID is: {0}", proc.Id);

                ms.CopyTo(proc.StandardInput.BaseStream);
                proc.StandardInput.Close();

                proc.BeginErrorReadLine();
                var output = proc.StandardOutput.ReadToEnd();

                proc.WaitForExit();
                Console.WriteLine("Exit code was: {0}", proc.ExitCode);
                Console.WriteLine("Output was:\n{0}", output);
            }
        }
    }
}
