// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.LanguageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Itx.Flex.Client.Service
{
  public class LanguageService : ILanguageService
  {
    private readonly IFileService fileService;
    private readonly IConfigurationService configurationService;
    private readonly IMessenger messenger;

    public LanguageService(IFileService fileService, IConfigurationService configurationService, IMessenger messenger)
    {
      this.fileService = fileService;
      this.configurationService = configurationService;
      this.messenger = messenger;
    }

    public void Initialize()
    {
      IEnumerable<string> source1 = this.fileService.ReadLinesFromFile(this.configurationService.DaDkLanguageFilePath);
      IEnumerable<string> source2 = this.fileService.ReadLinesFromFile(this.configurationService.EnGbLanguageFilePath);
      this.DaDkLanguageDictionary = this.ConvertToDictionary(source1.Select<string, string>(new Func<string, string>(Regex.Unescape)));
      this.EnGbLanguageDictionary = this.ConvertToDictionary(source2.Select<string, string>(new Func<string, string>(Regex.Unescape)));
    }

    public string SelectString(string textDaDk, string textEnGb)
    {
      switch (this.Language)
      {
        case Language.EnGb:
          return textEnGb;
        default:
          return textDaDk;
      }
    }

    private string ArgumentPattern
    {
      get
      {
        return "%(\\d+)\\$@";
      }
    }

    private Dictionary<string, string> ConvertToDictionary(IEnumerable<string> languageStrings)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (string str1 in languageStrings.Where<string>((Func<string, bool>) (ls =>
      {
        if (!string.IsNullOrEmpty(ls))
          return !ls.StartsWith("/*");
        return false;
      })))
      {
        string[] strArray = str1.Split('=');
        string str2 = strArray[0].Trim();
        string str3 = str2.Substring(1, str2.Length - 2);
        string str4 = strArray[1].TrimEnd(';').Trim();
        string str5 = Regex.Replace(str4.Substring(1, str4.Length - 2), this.ArgumentPattern, (MatchEvaluator) (m => "{" + (object) (int.Parse(m.Groups[1].Value) - 1) + "}"));
        dictionary[str3.ToLowerInvariant()] = str5;
      }
      return dictionary;
    }

    private Dictionary<string, string> EnGbLanguageDictionary { get; set; }

    private Dictionary<string, string> DaDkLanguageDictionary { get; set; }

    private Language Language { get; set; }

    public void ChangeLanguage(Language changeToLanguage)
    {
      if (this.Language == changeToLanguage)
        return;
      this.Language = changeToLanguage;
      this.messenger.Send<OnLanguageChanged>(new OnLanguageChanged());
    }

    public void ChangeLanguage(int lcid)
    {
      if (lcid == 1030)
        this.ChangeLanguage(Language.DaDk);
      else
        this.ChangeLanguage(Language.EnGb);
    }

    public string GetString(string key)
    {
      if (key == null)
        return "";
      key = key.ToLowerInvariant();
      switch (this.Language)
      {
        case Language.EnGb:
          return this.GetStringWithFallback(key, (IDictionary<string, string>) this.EnGbLanguageDictionary, (IDictionary<string, string>) this.DaDkLanguageDictionary);
        default:
          return this.GetStringWithFallback(key, (IDictionary<string, string>) this.DaDkLanguageDictionary, (IDictionary<string, string>) this.EnGbLanguageDictionary);
      }
    }

    private string GetStringWithFallback(string key, IDictionary<string, string> primaryLookup, IDictionary<string, string> secondaryLookup)
    {
      string str;
      if (!primaryLookup.TryGetValue(key, out str) && !secondaryLookup.TryGetValue(key, out str))
        return "";
      return str;
    }
  }
}
