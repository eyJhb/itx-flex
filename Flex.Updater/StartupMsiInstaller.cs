// Decompiled with JetBrains decompiler
// Type: Flex.Updater.StartupMsiInstaller
// Assembly: Flex.Updater, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: A51821AE-A539-4ADD-B006-972CC1F7D242
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Updater.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Flex.Updater
{
  public class StartupMsiInstaller
  {
    public void RunUpdate(Action<string> statusCallback)
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      if (commandLineArgs.Length != 3)
        throw new Exception("Invalid arguments");
      if (commandLineArgs[1] != "update")
        throw new Exception("Invalid arguments");
      string str = commandLineArgs[2];
      if (!File.Exists(str))
        throw new Exception("File not found: " + str);
      statusCallback("Waiting for ITX Flex to close");
      while (true)
      {
        List<string> list = ((IEnumerable<Process>) Process.GetProcesses()).Select<Process, string>((Func<Process, string>) (p => p.ProcessName)).ToList<string>();
        Func<string, bool> func = (Func<string, bool>) (p => p == "Flex.Client");
        Func<string, bool> predicate;
        if (list.Count<string>(predicate) >= 1)
          Thread.Sleep(500);
        else
          break;
      }
      statusCallback("ITX Flex closed sucessfully");
      Thread.Sleep(500);
      statusCallback("Updating");
      this.RunNsis(str);
      if (!File.Exists(str))
        return;
      try
      {
        File.Delete(str);
      }
      catch
      {
      }
    }

    private int RunNsis(string exePath)
    {
      string str = "/S";
      Process process = new Process();
      process.StartInfo = new ProcessStartInfo()
      {
        FileName = exePath,
        Arguments = str
      };
      process.Start();
      process.WaitForExit();
      return process.ExitCode;
    }
  }
}
