// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.RetryWebClientServiceDecorator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Itx.Flex.Client.Service
{
  public class RetryWebClientServiceDecorator : IWebClientService
  {
    private readonly IWebClientService _webClientServiceImplementation;

    public RetryWebClientServiceDecorator(IWebClientService webClientServiceImplementation)
    {
      this._webClientServiceImplementation = webClientServiceImplementation;
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

    public byte[] DownloadData(string address)
    {
      List<Exception> source = new List<Exception>();
      for (int index = 0; index < this.RetryCount; ++index)
      {
        try
        {
          if (index > 0)
            Thread.Sleep(this.RetryInterval);
          return this._webClientServiceImplementation.DownloadData(address);
        }
        catch (Exception ex)
        {
          source.Add(ex);
        }
      }
      throw new NoConnectionException("Could not download file", source.First<Exception>());
    }

    public void DownloadFile(string url, string filepath)
    {
      List<Exception> source = new List<Exception>();
      for (int index = 0; index < this.RetryCount; ++index)
      {
        try
        {
          if (index > 0)
            Thread.Sleep(this.RetryInterval);
          this._webClientServiceImplementation.DownloadFile(url, filepath);
          return;
        }
        catch (Exception ex)
        {
          source.Add(ex);
        }
      }
      throw new NoConnectionException("Could not download file", source.First<Exception>());
    }
  }
}
