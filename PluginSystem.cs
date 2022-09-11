using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI
{
    public interface IPlugin
    {
        /// <summary>
        /// This function is called when the plugin is first activated by the Plugin Loader. Tasks, such as loading configuration should be executed in this function
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public void onActivate();
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// Called upon unloading the assembly
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public void onDeactivate();
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This function is called every tick. A tick should ideally run every 2 seconds
        /// </summary>
        /// <param name="lastTick">The last time a tick was executed by the host program</param>
#pragma warning disable IDE1006 // Naming Styles
        public void onTick(TimeSpan lastTick);
#pragma warning restore IDE1006 // Naming Styles

        public string PluginName { get; }
        public VersionNumber PluginVersion { get; }
    }
    public class VersionNumber
    {
        public int Major;
        public int Minor;
        public int Build;
        public int Revision;

        public static bool operator >(VersionNumber a, VersionNumber b)
        {
            bool ret = true;
            bool f = false;
            if (a.Major < b.Major) ret=f;
            if (a.Minor < b.Minor) ret = f;
            if (a.Build < b.Build) ret = f;
            if (a.Revision < b.Revision) ret = f;


            return ret;
        }
        public static bool operator <(VersionNumber a, VersionNumber b)
        {

            bool ret = true;
            bool f = false;
            if (a.Major > b.Major) ret = f;
            if (a.Minor > b.Minor) ret = f;
            if (a.Build > b.Build) ret = f;
            if (a.Revision > b.Revision) ret = f;


            return ret;
        }

        public static VersionNumber operator +(VersionNumber a, VersionNumber b)
        {
            a.Major += b.Major;
            a.Minor += b.Minor;
            a.Build += b.Build;
            a.Revision += b.Revision;

            return a;
        }
        public static VersionNumber operator -(VersionNumber a, VersionNumber b)
        {
            a.Major -= b.Major;
            a.Minor -= b.Minor;
            a.Build -= b.Build;
            a.Revision -= b.Revision;

            return a;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }

    }
    public class PluginSystem
    {
        public Assembly LoadedASM = null;
        public Assembly LoadLibrary(string DLL)
        {
            LoadedASM = Assembly.LoadFrom(DLL);
            return LoadedASM;
        }

        public List<IPlugin> Activate(Assembly asm)
        {
            List<IPlugin> ret = new List<IPlugin>();
            foreach(Type A in asm.GetTypes())
            {
                Type check = A.GetInterface("IPlugin");
                if (check != null)
                {
                    IPlugin plugin = Activator.CreateInstance(A) as IPlugin;
                    plugin.onActivate();
                    ret.Add(plugin);
                }
            }

            return ret;
        }


        /// <summary>
        /// Scans memory for commands
        /// </summary>
        /// <typeparam name="T">Attribute that should be scanned for</typeparam>
        public static void ScanForCommands<T>()
        {

        }
    }

    public class CommandRegistry
    {
        public static CommandRegistry _reg = null;
        public static readonly object lckreg = new object();
        static CommandRegistry() { }

        public static CommandRegistry Master
        {
            get
            {
                if (_reg != null) return _reg;
                else
                {
                    lock (lckreg)
                    {
                        if (_reg == null) _reg = new CommandRegistry();
                        return _reg;
                    }
                }
            }
        }

        // We now want to have as universal of a command registry as possible.
        // The structure of this should be equivalent to a Hive
        // All functions should return a CommandResult instance which will provide a error code, and response message.
        // The CommandResult should also be able to convey if it is meant to be responded to privately, or in the same context where the command was issued.

        /// <summary>
        /// This provides the actual registry.
        /// </summary>
        private Hive Registry { get; set; } = new Hive();

#pragma warning disable IDE1006 // Naming Styles
        public void register(CommandBase baseApi)
#pragma warning restore IDE1006 // Naming Styles
        {
            Registry.register(baseApi);
        }

#pragma warning disable IDE1006 // Naming Styles
        public CommandBase getCommand(string sCmd)
#pragma warning restore IDE1006 // Naming Styles
        {
            return Registry.get(sCmd);
        }
    }

    /// <summary>
    /// The registry hive. Subject to change to a real registry format.
    /// </summary>
    public class Hive
    {
        // This functionality, is just a registry hive
        private Dictionary<string, CommandBase> commands = new Dictionary<string, CommandBase>();
#pragma warning disable IDE1006 // Naming Styles
        public void register(CommandBase api)
#pragma warning restore IDE1006 // Naming Styles
        {
            commands[api.Command] = api;
        }

#pragma warning disable IDE1006 // Naming Styles
        public CommandBase get(string sCmd)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (commands.ContainsKey(sCmd) == false) return null;
            return commands[sCmd];
        }

    }

    public class KVP<T>
    {
        public string Key;
        public T Value;
    }

    public class CommandArguments
    {
        public Dictionary<string, object> args = new Dictionary<string, object>();
        public int AuthorityLevel;
        
        public CommandArguments(params KVP<object>[] arguments)
        {
            foreach(KVP<object> arg in arguments)
            {
                args.Add(arg.Key, arg.Value);
            }
        }
    }

    [Flags]
    public enum ErrorCodes
    {
        AUTH_LEVEL=0x0001,
        SUCCESS=0x0002,
        EXPECTED_PARAMETERS_MISSING=0x0003
    }


    public class CommandResult
    {
        public enum ResultType
        {
            OK,
            ERROR
        }
        public enum ResponseType
        {
            SAFE,
            SENSITIVE
        }
        public ResultType Type;
        public string ResponseText;
        public ResponseType Resp_Type;
        public int ResponseCode;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)] // Allow alias commands
    public class CommandBase : Attribute
    {
        [Flags]
        public enum Destinations
        {
            ANY = 1,
            LOCAL = 2,
            GROUP = 4,
            IM = 8,
            DISCORD = 16
        }
        public string Command;
        public int MinLevel;
        public MethodInfo assignedMethod;
        public CommandHelp usage;
        public Destinations cmdType;

        public CommandBase(string cmd, int level, string hlp, Destinations dest)
        {
            Command = cmd;
            MinLevel = level;
            usage = new CommandHelp(cmd, level, hlp, dest);
            cmdType = dest;
        }
    }

    public class CommandHelp
    {
        public string Name;
        public int lvl;
        public string Text;
        public string sources;
        public CommandBase.Destinations Dests;

        public bool HasGroupFlag()
        {
            return ((Dests & CommandBase.Destinations.GROUP) == CommandBase.Destinations.GROUP);
        }
        public static readonly string NoArgs = "This command does not take any arguments";
        
        public string GetUsageAsString()
        {
            return $"_\nCommand [{Name}]\n{sources}\nLevel Required: {lvl}\nUsage: \n\n{Text}";
        }

        public CommandHelp(string cmdname, int lvls, string htext, CommandBase.Destinations dst)
        {
            Name = cmdname;
            lvl = lvls;
            List<string> rss = new List<string>();
            if ((Dests & CommandBase.Destinations.GROUP) == CommandBase.Destinations.GROUP) rss.Add("Group");
            if ((Dests & CommandBase.Destinations.LOCAL) == CommandBase.Destinations.LOCAL) rss.Add("Local");
            if ((Dests & CommandBase.Destinations.IM) == CommandBase.Destinations.IM) rss.Add("IM");
            if ((Dests & CommandBase.Destinations.DISCORD) == CommandBase.Destinations.DISCORD) rss.Add("Discord");
            if ((Dests & CommandBase.Destinations.ANY) == CommandBase.Destinations.ANY)
            {
                rss.Clear();
                rss.Add("Any");
            }

            Dests = dst;
            Text = htext;
            sources = $"Command can be used in: {String.Join(", ", rss)}";
        }
    }

}
