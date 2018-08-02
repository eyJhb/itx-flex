// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.LoggingFlexClientDecorator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itx.Flex.Client.Service
{
  public class LoggingFlexClientDecorator : IFlexClient, IGrabUploader
  {
    private readonly IFlexClient _service;
    private readonly ILoggerService _loggerService;

    public LoggingFlexClientDecorator(IFlexClient service, ILoggerService loggerService)
    {
      this._service = service;
      this._loggerService = loggerService;
    }

    private T Log<T>(Func<T> action)
    {
      try
      {
        return action();
      }
      catch (Exception ex)
      {
        Exception exception = ex;
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        for (; exception != null; exception = exception.InnerException)
        {
          stringBuilder1.AppendLine(exception.Message);
          stringBuilder2.AppendLine(exception.StackTrace);
        }
        this._loggerService.Log(LogType.Error, stringBuilder1.ToString(), stringBuilder2.ToString());
        throw;
      }
    }

    private void Log(Action action)
    {
      this.Log<object>((Func<object>) (() =>
      {
        action();
        return (object) null;
      }));
    }

    public void Authenticate(string boardingCode)
    {
      this.Log((Action) (() => this._service.Authenticate(boardingCode)));
    }

    public GlobalResponse GetGlobalSettings()
    {
      return this.Log<GlobalResponse>((Func<GlobalResponse>) (() => this._service.GetGlobalSettings()));
    }

    public HeartbeatResponse Heartbeat(HeartbeatRequest request)
    {
      return this.Log<HeartbeatResponse>((Func<HeartbeatResponse>) (() => this._service.Heartbeat(request)));
    }

    public AssignmentDecryptionResponse GetAssigmentDecryptionKeys(AssignmentDecryptionRequest request)
    {
      return this.Log<AssignmentDecryptionResponse>((Func<AssignmentDecryptionResponse>) (() => this._service.GetAssigmentDecryptionKeys(request)));
    }

    public CreateSessionResponse CreateGrabSession(CreateSessionRequest request)
    {
      return this.Log<CreateSessionResponse>((Func<CreateSessionResponse>) (() => this._service.CreateGrabSession(request)));
    }

    public GetMissingHashesResponse GetMissingGrabHashes(GetMissingHashesRequest request)
    {
      return this.Log<GetMissingHashesResponse>((Func<GetMissingHashesResponse>) (() => this._service.GetMissingGrabHashes(request)));
    }

    public void PostGrabs(PostGrabsRequest request)
    {
      this.Log((Action) (() => this._service.PostGrabs(request)));
    }

    public void PostGrabsFromPreviousSession(PostGrabsRequest request, IFlexEndpoints oldEndpoints, string oldJwtToken)
    {
      this.Log((Action) (() => this._service.PostGrabsFromPreviousSession(request, oldEndpoints, oldJwtToken)));
    }

    public GrabConfigurationResponse GetGrabConfiguration()
    {
      return this.Log<GrabConfigurationResponse>((Func<GrabConfigurationResponse>) (() => this._service.GetGrabConfiguration()));
    }

    public AssignmentResponse GetAssigments()
    {
      return this.Log<AssignmentResponse>((Func<AssignmentResponse>) (() => this._service.GetAssigments()));
    }

    public void SendBackupFile(BackupRequest request)
    {
      this.Log((Action) (() => this._service.SendBackupFile(request)));
    }

    public BackupCurrentMetadataResponse BackupSendCurrentMetadata(BackupCurrentMetadataRequest request)
    {
      return this.Log<BackupCurrentMetadataResponse>((Func<BackupCurrentMetadataResponse>) (() => this._service.BackupSendCurrentMetadata(request)));
    }

    public ExaminationResponse GetExamination()
    {
      return this.Log<ExaminationResponse>((Func<ExaminationResponse>) (() => this._service.GetExamination()));
    }

    public void SetBoardingPassPrefix(string s)
    {
      this.Log((Action) (() => this._service.SetBoardingPassPrefix(s)));
    }

    public void LogClientClosedByUserAfterConfirmation(DateTime eventOn)
    {
      this.Log((Action) (() => this._service.LogClientClosedByUserAfterConfirmation(eventOn)));
    }

    public void LogClientClosedByUserWithoutConfirmation(DateTime eventOn)
    {
      this.Log((Action) (() => this._service.LogClientClosedByUserWithoutConfirmation(eventOn)));
    }

    public QueueNumberResponse QueueNumber()
    {
      return this.Log<QueueNumberResponse>((Func<QueueNumberResponse>) (() => this._service.QueueNumber()));
    }

    public TagPendingHandinResponse TagHandin(TagPendingHandinRequest request)
    {
      return this.Log<TagPendingHandinResponse>((Func<TagPendingHandinResponse>) (() => this._service.TagHandin(request)));
    }

    public HandInBlankResponse HandinBlank()
    {
      return this.Log<HandInBlankResponse>((Func<HandInBlankResponse>) (() => this._service.HandinBlank()));
    }

    public void Handin(string mainDocumentFilepath, IEnumerable<string> attachmentFilepaths, string handinFieldsFilePath)
    {
      this.Log((Action) (() => this._service.Handin(mainDocumentFilepath, attachmentFilepaths, handinFieldsFilePath)));
    }

    public void SendCrashReport(LogDumpRequest logDumpRequest)
    {
      this._service.SendCrashReport(logDumpRequest);
    }

    public string JwtToken
    {
      get
      {
        return this._service.JwtToken;
      }
    }

    public string BoardingPass
    {
      get
      {
        return this._service.BoardingPass;
      }
    }
  }
}
