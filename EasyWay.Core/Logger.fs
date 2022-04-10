namespace EasyWay.Core.Utils

open System
open System.IO
open System.Reflection
open System.Text
open NLog
open NLog.Config
open NLog.Targets

type Logger private () =
    let assembly = Assembly.GetCallingAssembly()
    let name = assembly.GetName().Name
    static let mutable _instance = Logger()
    let mutable _logger = LogManager.GetLogger($"{name}.Logger")
    
    do
        let directory    = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        let logDirectory = Path.Combine(directory, "Logs/")
        let config       = LoggingConfiguration()
        
        if not <| Directory.Exists directory then raise <| DirectoryNotFoundException directory
        
        let allFileTarget = new FileTarget("allFileTarget")
        
        allFileTarget.FileName         <- Layouts.Layout.FromString $"{logDirectory}Debug.log"
        allFileTarget.Layout           <- Layouts.Layout.FromString "${longdate} ${uppercase:${level}} ${message}"
        allFileTarget.Encoding         <- Encoding.Unicode
        allFileTarget.ArchiveFileName  <- Layouts.Layout.FromString ($"{logDirectory}" + "Debug.${shortdate}.{#}.log")
        allFileTarget.ArchiveAboveSize <- 16_777_216L
        allFileTarget.ArchiveEvery     <- FileArchivePeriod.Day
        allFileTarget.ArchiveNumbering <- ArchiveNumberingMode.Rolling
        allFileTarget.MaxArchiveFiles  <- 20
        
        config.AddTarget(allFileTarget)
        config.AddRuleForAllLevels(allFileTarget)
        
        LogManager.Configuration <- config
        
        _logger <- LogManager.GetLogger($"{name}.Logger")
    
    member x.Logger
        with get() = _logger
        and set value = _logger <- value
    
    static member private Instance =
        if _instance.Logger.Name = Assembly.GetCallingAssembly().GetName().Name then _instance
        else
            _instance <- Logger()
            _instance
    
    static member private Log = Logger.Instance.Logger
    static member Debug(message: String)    = Logger.Log.Debug(message)
    static member Debug(message: Exception) = Logger.Log.Debug(message)
    static member Info(message: String)     = Logger.Log.Info(message)
    static member Info(message: Exception)  = Logger.Log.Info(message)

