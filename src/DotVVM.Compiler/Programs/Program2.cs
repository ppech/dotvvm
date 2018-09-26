using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using DotVVM.Compiler.Compilation;
using DotVVM.Compiler.DTOs;
using DotVVM.Compiler.Initialization;
using DotVVM.Compiler.Output;
using DotVVM.Compiler.Resolving;
using Newtonsoft.Json;

namespace DotVVM.Compiler.Programs
{
    public class Program2
    {
        public static IOutputLogger Logger { get; private set; }

        internal static CompilerOptions Options { get; private set; }
        internal static HashSet<string> assemblySearchPaths { get; private set; } = new HashSet<string>();
        private static Stopwatch stopwatcher;
        private static string GetEnvironmentWebAssemblyPath()
        {
            return Environment.GetEnvironmentVariable("webAssemblyPath");
        }
        public static void ContinueMain(string[] args)
        {
            Logger = new AggregatedOutputLogger(new ConsoleOutputLogger(stopwatcher));

            WriteTargetFramework();

            GetEnvironmentAssemblySearchPaths();
#if NETCOREAPP2_0
            AssemblyResolver.ResolverNetstandard(GetEnvironmentWebAssemblyPath());
#else
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.ResolveAssembly;
#endif
            if (args.Length == 0)
            {
                while (true)
                {
                    var optionsJson = ReadJsonFromStdin();
                    Options = GetCompilerOptions(optionsJson);
                    ProcessCompilationRequest(Options);
                }
            }
            if (args[0] == "-?")
            {
                WriteHelp();
                Exit(0);
            }
            if (args[0] == "--debugger")
            {
                WaitForDbg();
                args = args.Skip(1).ToArray();
            }
            if (args[0] == "--debugger-break")
            {
                WaitForDbg(true);
                args = args.Skip(1).ToArray();
            }
            if (args[0] == "--logfile")
            {
                var filePath = args[1];
                Logger = new AggregatedOutputLogger(new ConsoleOutputLogger(stopwatcher), new FileOutputLogger(filePath, stopwatcher));
                args = args.Skip(2).ToArray();
            }

            if (args[0] == "--json")
            {
                var opt = string.Join(" ", args.Skip(1));

                Options = GetCompilerOptions(opt);
                if (!ProcessCompilationRequest(Options))
                {
                    Exit(1);
                }
            }
            else
            {
                var file = string.Join(" ", args);
                if (!DoCompileFromFile(file))
                {
                    Exit(1);
                }
            }

        }

        private static void WriteTargetFramework()
        {
#if NET461
            Logger.WriteInfo("Target framework: .NET Framework 4.6");
#elif NETCOREAPP2_0
            Logger.WriteInfo("Target framework: .NET Standard 2.0");
#endif
        }

        private static void WriteHelp()
        {
            Console.Write(@"
DotVVM Compiler
    --json      - Determines options for compiler
    --debugger  - Waits as long as compiler is not attached


JSON structure:
        string[] DothtmlFiles           (null = build all, [] = build none)
        string WebSiteAssembly          
        bool OutputResolvedDothtmlMap   
        string BindingsAssemblyName 
        string BindingClassName 
        string OutputPath 

        string AssemblyName             
        string WebSitePath              
        bool FullCompile                (default: true)
        bool CheckBindingErrors         
        bool SerializeConfig            
        string ConfigOutputPath

");

        }

        private static void Exit(int exitCode)
        {

            Environment.Exit(exitCode);
        }

        private static void GetEnvironmentAssemblySearchPaths()
        {
            foreach (var path in Environment.GetEnvironmentVariable("assemblySearchPath")?.Split(',') ?? new string[0])
            {
                Logger.WriteInfo($"Adding path from environment variable: {path}");
                assemblySearchPaths.Add(path);
            }
            Logger.WriteInfo($"Adding path from current working directory: {Environment.CurrentDirectory}");
            assemblySearchPaths.Add(Environment.CurrentDirectory);
            //Env current directory may not be directory where compiler.exe or .dll is located
            Logger.WriteInfo($"Adding path from compiler assembly startup path: {AppDomain.CurrentDomain.BaseDirectory}");
            assemblySearchPaths.Add(AppDomain.CurrentDomain.BaseDirectory);
        }
       

        private static void WaitForDbg(bool _break = false)
        {
            Logger.WriteInfo("Process ID: " + Process.GetCurrentProcess().Id);
            while (!Debugger.IsAttached) Thread.Sleep(10);
            if (_break)
            {
                Debugger.Break();
            }
        }


        private static string ReadJsonFromStdin()
        {
            var optionsJson = ReadFromStdin();
            if (string.IsNullOrWhiteSpace(optionsJson))
            {
                Environment.Exit(0);
            }
            return optionsJson;
        }

        private static bool DoCompileFromFile(string file)
        {
            var optionsJson = File.ReadAllText(file);
            var compilerOptions = GetCompilerOptions(optionsJson);
            return ProcessCompilationRequest(compilerOptions);
        }

        private static bool ProcessCompilationRequest(CompilerOptions options)
        {
            InitStopwacher();

            //Don't touch anything until the paths are filled we have to touch Json though
            try
            {
                CompilationResult result;
                if (options.FullCompile)
                {
                    // compile views
                    Logger.WriteInfo("Starting full compilation...");
                    result = Compile(options);
                }
                else if (options.CheckBindingErrors)
                {
                    // check errors views
                    Logger.WriteInfo("Starting error validation...");
                    result = Compile(options);
                }
                else
                {
                    // only export configuration
                    Logger.WriteInfo("Starting export configuration only ...");
                    result = ExportConfiguration(options);
                }

                ConfigurationSerialization.PreInit();

                var serializedResult = JsonConvert.SerializeObject(result, Formatting.Indented,
                    new JsonSerializerSettings {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                Logger.WriteResult(serializedResult);
                WriteConfigurationOutput(serializedResult);

                Console.WriteLine();
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return false;
            }
        }

        private static void WriteConfigurationOutput(string serializedResult)
        {
            var path = Options.ConfigOutputPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            var file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            Logger.WriteInfo($"Saving configuration to '{file.FullName}'");
            File.WriteAllText(file.FullName, serializedResult);
        }

        private static CompilationResult Compile(CompilerOptions options)
        {
            var compiler = new ViewStaticCompiler();
            compiler.Options = options;
            return compiler.Execute();
        }
        private static CompilationResult ExportConfiguration(CompilerOptions options)
        {
            var assembly = Assembly.LoadFile(options.WebSiteAssembly);
            var config = ConfigurationInitializer.InitDotVVM(assembly, options.WebSitePath, null, collection => { });
            return new CompilationResult() {
                Configuration = config
            };
        }

        private static void InitStopwacher()
        {
            stopwatcher = Stopwatch.StartNew();
        }

        private static CompilerOptions GetCompilerOptions(string optionsJson)
        {
            var options = new CompilerOptions();
            try
            {
                options = JsonConvert.DeserializeObject<CompilerOptions>(optionsJson);
                if (!string.IsNullOrEmpty(options.WebSiteAssembly))
                {
                    assemblySearchPaths.Add(Path.GetDirectoryName(options.WebSiteAssembly));
                }

                Logger.WriteInfo("Using the following assembly search paths: ");
                foreach (var path in assemblySearchPaths)
                {
                    Logger.WriteInfo(path);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

            return options;
        }

        private static string ReadFromStdin()
        {
            var sb = new StringBuilder();
            string line;
            do
            {
                line = Console.ReadLine();
                sb.Append(line);
            } while (!string.IsNullOrEmpty(line));
            return sb.ToString();
        }
    }
}
