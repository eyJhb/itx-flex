// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.RetryConnectionFlexClientDecorator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Itx.Flex.Client.Service
{
  public class RetryConnectionFlexClientDecorator : IFlexClient, IGrabUploader
  {
    private readonly IFlexClient _service;

    public RetryConnectionFlexClientDecorator(IFlexClient service)
    {
      this._service = service;
    }

    private int RetryCount
    {
      get
      {
        return 3;
      }
    }

    private TimeSpan RetryInterval
    {
      get
      {
        return TimeSpan.FromSeconds(1.0);
      }
    }

    private T Retry<T>(Func<T> action, TimeSpan retryInterval, int retryCount)
    {
      List<Exception> source = new List<Exception>();
      for (int index = 0; index < retryCount; ++index)
      {
        try
        {
          if (index > 0)
            Thread.Sleep(retryInterval);
          return action();
        }
        catch (WebException ex)
        {
          if (ex.Status == WebExceptionStatus.Timeout || ex.Status == WebExceptionStatus.ConnectFailure)
            source.Add((Exception) ex);
          else
            throw;
        }
      }
      throw new NoConnectionException("Could not connect to backend", source.First<Exception>());
    }

    private void Retry(Action action, TimeSpan retryInterval, int retryCount)
    {
      this.Retry<object>((Func<object>) (() =>
      {
        action();
        return (object) null;
      }), retryInterval, retryCount);
    }

    public void Authenticate(string boardingCode)
    {
      this.Retry((Action) (() => this._service.Authenticate(boardingCode)), this.RetryInterval, this.RetryCount);
    }

    public GlobalResponse GetGlobalSettings()
    {
      return this.Retry<GlobalResponse>((Func<GlobalResponse>) (() => this._service.GetGlobalSettings()), this.RetryInterval, this.RetryCount);
    }

    public HeartbeatResponse Heartbeat(HeartbeatRequest request)
    {
      return this.Retry<HeartbeatResponse>((Func<HeartbeatResponse>) (() => this._service.Heartbeat(request)), this.RetryInterval, this.RetryCount);
    }

    public AssignmentDecryptionResponse GetAssigmentDecryptionKeys(AssignmentDecryptionRequest request)
    {
      return this.Retry<AssignmentDecryptionResponse>((Func<AssignmentDecryptionResponse>) (() => this._service.GetAssigmentDecryptionKeys(request)), this.RetryInterval, this.RetryCount);
    }

    public CreateSessionResponse CreateGrabSession(CreateSessionRequest request)
    {
      return this.Retry<CreateSessionResponse>((Func<CreateSessionResponse>) (() => this._service.CreateGrabSession(request)), this.RetryInterval, this.RetryCount);
    }

    public GetMissingHashesResponse GetMissingGrabHashes(GetMissingHashesRequest request)
    {
      return this.Retry<GetMissingHashesResponse>((Func<GetMissingHashesResponse>) (() => this._service.GetMissingGrabHashes(request)), this.RetryInterval, this.RetryCount);
    }

    public void PostGrabs(PostGrabsRequest request)
    {
      this.Retry((Action) (() => this._service.PostGrabs(request)), this.RetryInterval, this.RetryCount);
    }

    public void PostGrabsFromPreviousSession(PostGrabsRequest request, IFlexEndpoints oldEndpoints, string oldJwtToken)
    {
      this.Retry((Action) (() => this._service.PostGrabsFromPreviousSession(request, oldEndpoints, oldJwtToken)), this.RetryInterval, this.RetryCount);
    }

    public GrabConfigurationResponse GetGrabConfiguration()
    {
      return this.Retry<GrabConfigurationResponse>((Func<GrabConfigurationResponse>) (() => this._service.GetGrabConfiguration()), this.RetryInterval, this.RetryCount);
    }

    public AssignmentResponse GetAssigments()
    {
      return this.Retry<AssignmentResponse>((Func<AssignmentResponse>) (() => this._service.GetAssigments()), this.RetryInterval, this.RetryCount);
    }

    public void SendBackupFile(BackupRequest request)
    {
      this.Retry((Action) (() => this._service.SendBackupFile(request)), this.RetryInterval, this.RetryCount);
    }

    public BackupCurrentMetadataResponse BackupSendCurrentMetadata(BackupCurrentMetadataRequest request)
    {
      return this.Retry<BackupCurrentMetadataResponse>((Func<BackupCurrentMetadataResponse>) (() => this._service.BackupSendCurrentMetadata(request)), this.RetryInterval, this.RetryCount);
    }

    public ExaminationResponse GetExamination()
    {
      return this.Retry<ExaminationResponse>((Func<ExaminationResponse>) (() => this._service.GetExamination()), this.RetryInterval, this.RetryCount);
    }

    public void SetBoardingPassPrefix(string boardingPass)
    {
      this.Retry((Action) (() => this._service.SetBoardingPassPrefix(boardingPass)), this.RetryInterval, this.RetryCount);
    }

    public void LogClientClosedByUserAfterConfirmation(DateTime eventOn)
    {
      this.Retry((Action) (() => this._service.LogClientClosedByUserAfterConfirmation(eventOn)), this.RetryInterval, this.RetryCount);
    }

    public void LogClientClosedByUserWithoutConfirmation(DateTime eventOn)
    {
      this.Retry((Action) (() => this._service.LogClientClosedByUserWithoutConfirmation(eventOn)), this.RetryInterval, this.RetryCount);
    }

    public void SendCrashReport(LogDumpRequest logDumpRequest)
    {
      this.Retry((Action) (() => this._service.SendCrashReport(logDumpRequest)), this.RetryInterval, this.RetryCount);
    }

    public QueueNumberResponse QueueNumber()
    {
      return this.Retry<QueueNumberResponse>((Func<QueueNumberResponse>) (() => this._service.QueueNumber()), this.RetryInterval, this.RetryCount);
    }

    public TagPendingHandinResponse TagHandin(TagPendingHandinRequest request)
    {
      return this.Retry<TagPendingHandinResponse>((Func<TagPendingHandinResponse>) (() => this._service.TagHandin(request)), this.RetryInterval, this.RetryCount);
    }

    public HandInBlankResponse HandinBlank()
    {
      return this.Retry<HandInBlankResponse>((Func<HandInBlankResponse>) (() => this._service.HandinBlank()), this.RetryInterval, this.RetryCount);
    }

    public void Handin(string mainDocumentFilepath, IEnumerable<string> attachmentFilepaths, string handinFieldsFilePath)
    {
      this.Retry((Action) (() => this._service.Handin(mainDocumentFilepath, attachmentFilepaths, handinFieldsFilePath)), this.RetryInterval, this.RetryCount);
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
