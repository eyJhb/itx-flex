// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Startup
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Castle.MicroKernel;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Service;
using Itx.Flex.Client.View;
using System.Globalization;

namespace Itx.Flex.Client
{
  internal class Startup
  {
    public static void Start()
    {
      DispatcherHelper.Initialize();
      WindsorContainer windsorContainer = new WindsorContainer();
      windsorContainer.Kernel.Resolver.AddSubResolver((ISubDependencyResolver) new CollectionResolver(windsorContainer.Kernel, false));
      windsorContainer.Install(FromAssembly.This());
      ILanguageService languageService = windsorContainer.Resolve<ILanguageService>();
      languageService.Initialize();
      languageService.ChangeLanguage(CultureInfo.CurrentUICulture.LCID);
      Messenger.OverrideDefault(windsorContainer.Resolve<IMessenger>());
      MainWindow mainWindow = windsorContainer.Resolve<MainWindow>();
      mainWindow.ShowDialog();
      windsorContainer.Release((object) mainWindow);
    }
  }
}
