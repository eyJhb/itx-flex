// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Installer.Installers
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Flex.WebserviceClient;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using Grabber.Core;
using Grabber.Core.Providers;
using Itx.Flex.Client.AutoUpdate;
using Itx.Flex.Client.Service;
using Itx.Flex.Client.View;
using Itx.Flex.Client.ViewModel;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace Itx.Flex.Client.Installer
{
  public class Installers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      FlexEndpoints flexEndpoints = new FlexEndpoints(ConfigurationService.GlobalEndpoint);
      container.Register((IRegistration) Component.For<IFlexEndpoints>().Instance((IFlexEndpoints) flexEndpoints).LifestyleSingleton());
      container.Register((IRegistration) Component.For<IFlexClient>().ImplementedBy<LoggingFlexClientDecorator>().LifestyleSingleton(), (IRegistration) Component.For<IFlexClient>().ImplementedBy<RetryConnectionFlexClientDecorator>().LifestyleSingleton(), (IRegistration) Component.For<IFlexClient>().ImplementedBy<FlexClient>().LifestyleSingleton(), (IRegistration) Component.For<IEndpointResolver>().ImplementedBy<EndpointResolver>().LifestyleSingleton());
      container.Register((IRegistration) Component.For<IUpdater>().LifestyleTransient().ImplementedBy<UpdaterProvider>());
      container.Register((IRegistration) Component.For<IIsCurrentVersionProvider>().LifestyleTransient().ImplementedBy<IsCurrentVersionProvider>());
      container.Register((IRegistration) Component.For<IApplicationExitProvider>().LifestyleTransient().ImplementedBy<ApplicationExitProvider>());
      container.Register((IRegistration) Component.For<IGrabberSession>().LifestyleSingleton().ImplementedBy<GrabberSession>());
      container.Register((IRegistration) Component.For<ICurrentVersionProvider>().LifestyleTransient().ImplementedBy<CurrentVersionProvider>());
      container.Register((IRegistration) Component.For<IHashProvider>().LifestyleTransient().ImplementedBy<Sha256HashProvider>());
      container.Register((IRegistration) Component.For<IFileDecrypter>().LifestyleTransient().ImplementedBy<FileDecrypter>());
      container.Register((IRegistration) Component.For<IMessenger>().ImplementedBy<Messenger>().LifestyleSingleton(), (IRegistration) Component.For<WebClient>().LifestyleTransient(), (IRegistration) Component.For<ILanguageService>().ImplementedBy<LanguageService>().LifestyleSingleton(), (IRegistration) Component.For<IHeartbeatService>().ImplementedBy<HeartbeatService>().LifestyleSingleton(), (IRegistration) Component.For<System.Timers.Timer>().LifestyleTransient(), (IRegistration) Component.For<IConfigurationService>().ImplementedBy<ConfigurationService>().LifestyleSingleton(), (IRegistration) Component.For<IBackupService>().ImplementedBy<BackupService>().LifestyleSingleton(), (IRegistration) Component.For<ICaptureWindow>().ImplementedBy<CaptureWindow>().LifestyleSingleton(), (IRegistration) Component.For<IWebClientService>().ImplementedBy<RetryWebClientServiceDecorator>().LifestyleTransient(), (IRegistration) Component.For<IDateTimeService, IGrabTimeService>().ImplementedBy<DateTimeService>().LifestyleSingleton(), (IRegistration) Component.For<Stopwatch>().LifestyleTransient(), (IRegistration) Component.For<ILastSaveViewModel>().ImplementedBy<LastSaveViewModel>().LifestyleSingleton(), (IRegistration) Component.For<IExamTimeLeftViewModel>().ImplementedBy<ExamTimeLeftViewModel>().LifestyleSingleton());
      Assembly assembly = Assembly.GetAssembly(typeof (IMainWindowViewModel));
      container.Register((IRegistration) Classes.FromAssembly(assembly).Where((Predicate<Type>) (type =>
      {
        if (!type.Name.EndsWith("Service") && !(type.Name == "MainWindow") && !type.Name.EndsWith("ViewModel"))
          return type.Name.EndsWith("Validator");
        return true;
      })).WithService.AllInterfaces().WithServiceSelf().Configure((Action<ComponentRegistration>) (c => c.LifestyleTransient())));
    }
  }
}
