// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.WorkspaceStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;

namespace Itx.Flex.Client.Service
{
  public class WorkspaceStorageService : IWorkspaceStorageService
  {
    private readonly IRegistryService registryService;
    private const string WorkspaceKey = "Workspace";
    private const string AssignmentFilesKey = "AssignmentFiles";
    private const string HandInKey = "HandIn";

    public WorkspaceStorageService(IRegistryService registryService)
    {
      this.registryService = registryService;
    }

    public bool HasExisting()
    {
      if (!string.IsNullOrEmpty(this.registryService.GetValue("Workspace")) && !string.IsNullOrEmpty(this.registryService.GetValue("AssignmentFiles")))
        return !string.IsNullOrEmpty(this.registryService.GetValue("HandIn"));
      return false;
    }

    public WorkspacePaths GetExisting()
    {
      string workspacePath = this.registryService.GetValue("Workspace");
      string str1 = this.registryService.GetValue("AssignmentFiles");
      string str2 = this.registryService.GetValue("HandIn");
      string assignmentFilesPath = str1;
      string handInPath = str2;
      return new WorkspacePaths(workspacePath, assignmentFilesPath, handInPath);
    }

    public void Store(string workspacePath, string assignmentFilesPath, string handInPath)
    {
      this.registryService.SetValue("Workspace", workspacePath);
      this.registryService.SetValue("AssignmentFiles", assignmentFilesPath);
      this.registryService.SetValue("HandIn", handInPath);
    }

    public void Clear()
    {
      this.registryService.ClearValue("Workspace");
      this.registryService.ClearValue("AssignmentFiles");
      this.registryService.ClearValue("HandIn");
    }
  }
}
