// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HeartbeatService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using System;
using System.Timers;

namespace Itx.Flex.Client.Service
{
  public class HeartbeatService : IHeartbeatService
  {
    private readonly IFlexClient _flexClient;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMessenger _messenger;
    private readonly ITimerService _heartbeatIntervalTimer;

    public HeartbeatService(IFlexClient flexClient, IDateTimeService dateTimeService, IMessenger messenger, ITimerService heartbeatIntervalTimer)
    {
      this._flexClient = flexClient;
      this._dateTimeService = dateTimeService;
      this._messenger = messenger;
      this._heartbeatIntervalTimer = heartbeatIntervalTimer;
      this._heartbeatIntervalTimer.Interval = 5000.0;
    }

    public void StartHeartbeats()
    {
      this.StopHeartbeats();
      this._heartbeatIntervalTimer.AutoReset = false;
      this._heartbeatIntervalTimer.Elapsed += new ElapsedEventHandler(this.HeartbeatIntervalTimerOnElapsed);
      this._heartbeatIntervalTimer.Start();
      this.RestartHeartbeats();
    }

    public void StopHeartbeats()
    {
      this._heartbeatIntervalTimer.Stop();
      this._heartbeatIntervalTimer.Elapsed -= new ElapsedEventHandler(this.HeartbeatIntervalTimerOnElapsed);
    }

    private void RestartHeartbeats()
    {
      HeartbeatResponse heartbeatResponse = this.GetHeartbeatResponse();
      if (heartbeatResponse != null)
      {
        this.PropagateHeartbeatInformation(heartbeatResponse);
        this._heartbeatIntervalTimer.Stop();
        if (heartbeatResponse.NextHeartbeatInSeconds <= 0)
          return;
        this._heartbeatIntervalTimer.Interval = (double) (heartbeatResponse.NextHeartbeatInSeconds * 1000);
        this._heartbeatIntervalTimer.Start();
      }
      else
        this._heartbeatIntervalTimer.Start();
    }

    private HeartbeatResponse GetHeartbeatResponse()
    {
      try
      {
        return this._flexClient.Heartbeat(new HeartbeatRequest() { LocalTime = this._dateTimeService.LocalTime });
      }
      catch (Exception ex)
      {
      }
      return (HeartbeatResponse) null;
    }

    private void HeartbeatIntervalTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
      this.RestartHeartbeats();
    }

    private void PropagateHeartbeatInformation(HeartbeatResponse heartbeatResponse)
    {
      this._dateTimeService.UpdateServerTime(heartbeatResponse.ServerTime);
      this._messenger.Send<OnTimeLeftUntilExamStarts>(new OnTimeLeftUntilExamStarts(heartbeatResponse.ExaminationStartInSeconds));
      this._messenger.Send<OnTimeLeftUntilExamEnds>(new OnTimeLeftUntilExamEnds(heartbeatResponse.ExaminationEndInSeconds));
      this._messenger.Send<OnNewGrabSettings>(new OnNewGrabSettings(heartbeatResponse.EnableGrabs, heartbeatResponse.NoGrabsAfterSeconds, heartbeatResponse.ExaminationStartInSeconds));
      this._messenger.Send<OnNewBackupSettings>(new OnNewBackupSettings(heartbeatResponse.EnableBackup, heartbeatResponse.NoBackupAfterSeconds, heartbeatResponse.BackupIntervalInSeconds));
      this._messenger.Send<OnEnableHandInReceived>(new OnEnableHandInReceived(heartbeatResponse.EnableHandin));
      this._messenger.Send<OnHandInStatusHandInTypeUpdated>(new OnHandInStatusHandInTypeUpdated(heartbeatResponse.HandinType.ToHandInType(), heartbeatResponse.HandinStatus.ToHandInStatus()));
      this._messenger.Send<OnAllowHandInReceived>(new OnAllowHandInReceived(heartbeatResponse.AllowHandin));
    }
  }
}
