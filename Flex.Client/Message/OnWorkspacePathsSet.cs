// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Message.OnWorkspacePathsSet
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Message
{
  public class OnWorkspacePathsSet
  {
    public string HandInPath { get; }

    public string AssignmentFilesPath { get; }

    public OnWorkspacePathsSet(string assignmentFilesPath, string handInPath)
    {
      this.AssignmentFilesPath = assignmentFilesPath;
      this.HandInPath = handInPath;
    }
  }
}
