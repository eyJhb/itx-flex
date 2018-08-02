// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInUploadService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Itx.Flex.Client.Service
{
  public class HandInUploadService : IHandInUploadService
  {
    private readonly IFlexClient _flexClient;
    private readonly ITimerService _queueRetryTimerService;
    private readonly IMessenger _messenger;
    private readonly ILoggerService _loggerService;
    private const int QueueProgressPart = 50;
    private const int ProgressMaximum = 100;
    private List<SubmitHandInFileModel> _submitHandInFiles;

    public HandInUploadService(IFlexClient flexClient, ITimerService queueRetryTimerService, IMessenger messenger, ILoggerService loggerService)
    {
      this._flexClient = flexClient;
      this._queueRetryTimerService = queueRetryTimerService;
      this._messenger = messenger;
      this._loggerService = loggerService;
      this._queueRetryTimerService.AutoReset = false;
    }

    public void Upload(IEnumerable<SubmitHandInFileModel> submitHandInFiles)
    {
      this._submitHandInFiles = submitHandInFiles.ToList<SubmitHandInFileModel>();
      this._queueRetryTimerService.Elapsed += new ElapsedEventHandler(this.QueueRetryTimerServiceOnElapsed);
      this.SendFiles(this._submitHandInFiles);
    }

    private void QueueRetryTimerServiceOnElapsed(object o, ElapsedEventArgs elapsedEventArgs)
    {
      this.SendFiles(this._submitHandInFiles);
    }

    public void StopUpload()
    {
      this._queueRetryTimerService.Stop();
      this._queueRetryTimerService.Elapsed -= new ElapsedEventHandler(this.QueueRetryTimerServiceOnElapsed);
      this._submitHandInFiles = (List<SubmitHandInFileModel>) null;
    }

    private void SendFiles(List<SubmitHandInFileModel> submitHandInFiles)
    {
      QueueNumberResponse queueNumberResponse = this._flexClient.QueueNumber();
      if (queueNumberResponse.UploadAllowed)
      {
        try
        {
          this._messenger.Send<OnHandInUploadProgressUpdated>(new OnHandInUploadProgressUpdated(OnHandInUploadProgressUpdated.UploadStep.Uploading, this.CalculateProgress(queueNumberResponse.QueueProgress, 0)));
          this._flexClient.Handin(submitHandInFiles.First<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.MainDocument)).Path, submitHandInFiles.Where<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.Attachment)).Select<SubmitHandInFileModel, string>((Func<SubmitHandInFileModel, string>) (f => f.Path)), submitHandInFiles.FirstOrDefault<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.HandInFields))?.Path);
          this._messenger.Send<OnHandInUploadProgressUpdated>(new OnHandInUploadProgressUpdated(OnHandInUploadProgressUpdated.UploadStep.Done, this.CalculateProgress(queueNumberResponse.QueueProgress, 100)));
        }
        catch (Exception ex)
        {
          this._loggerService.Log("Could not upload hand in files: " + string.Join(",", submitHandInFiles.Select<SubmitHandInFileModel, string>((Func<SubmitHandInFileModel, string>) (p => p.Path)).ToArray<string>()), ex);
          this._messenger.Send<OnHandInUploadProgressUpdated>(new OnHandInUploadProgressUpdated(OnHandInUploadProgressUpdated.UploadStep.Error, 0.0));
        }
      }
      else
      {
        this._messenger.Send<OnHandInUploadProgressUpdated>(new OnHandInUploadProgressUpdated(OnHandInUploadProgressUpdated.UploadStep.InQueue, this.CalculateProgress(queueNumberResponse.QueueProgress, 0)));
        this.ResetQueueTimer(queueNumberResponse.NextCheckInSeconds);
      }
    }

    private double CalculateProgress(int queueProgress, int fileUploadProgress)
    {
      double num1 = 0.5;
      double num2 = (double) queueProgress * num1;
      double num3 = 0.5;
      double num4 = (double) fileUploadProgress * num3;
      return num2 + num4;
    }

    private void ResetQueueTimer(int nextCheckInSeconds)
    {
      this._queueRetryTimerService.Interval = (double) (nextCheckInSeconds * 1000);
      this._queueRetryTimerService.Start();
    }
  }
}
