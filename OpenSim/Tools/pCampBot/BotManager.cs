/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using log4net;
using log4net.Appender;
using log4net.Repository;
using Nini.Config;
using OpenMetaverse;
using OpenSim.Framework;
using OpenSim.Framework.Console;
using pCampBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace pCampBot
{
    /// <summary>
    /// Thread/Bot manager for the application
    /// </summary>
    public class BotManager
    {
        public const int DefaultLoginDelay = 5000;

        /// <summary>
        /// Created bots, whether active or inactive.
        /// </summary>
        protected List<Bot> m_bots;

        /// <summary>
        /// Command console
        /// </summary>
        protected CommandConsole m_console;

        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Behaviour switches for bots.
        /// </summary>
        private HashSet<string> m_defaultBehaviourSwitches = new HashSet<string>();

        /// <summary>
        /// First name for bots
        /// </summary>
        private string m_firstName;

        /// <summary>
        /// Postfix bot number at which bot sequence starts.
        /// </summary>
        private int m_fromBotNumber;

        /// <summary>
        /// Last name stem for bots
        /// </summary>
        private string m_lastNameStem;

        /// <summary>
        /// Login URI for bots.
        /// </summary>
        private string m_loginUri;

        /// <summary>
        /// Password for bots
        /// </summary>
        private string m_password;

        /// <summary>
        /// Start location for bots.
        /// </summary>
        private string m_startUri;

        /// <summary>
        /// Wear setting for bots.
        /// </summary>
        private string m_wearSetting;

        /// <summary>
        /// Constructor Creates MainConsole.Instance to take commands and provide the place to write data
        /// </summary>
        public BotManager()
        {
            InitBotSendAgentUpdates = true;
            InitBotRequestObjectTextures = true;

            LoginDelay = DefaultLoginDelay;

            Rng = new Random(Environment.TickCount);
            AssetsReceived = new Dictionary<UUID, bool>();
            RegionsKnown = new Dictionary<ulong, GridRegion>();

            m_console = CreateConsole();
            MainConsole.Instance = m_console;

            // Make log4net see the console
            //
            ILoggerRepository repository = LogManager.GetRepository();
            IAppender[] appenders = repository.GetAppenders();
            OpenSimAppender consoleAppender = null;

            foreach (IAppender appender in appenders)
            {
                if (appender.Name == "Console")
                {
                    consoleAppender = (OpenSimAppender)appender;
                    consoleAppender.Console = m_console;
                    break;
                }
            }

            m_console.Commands.AddCommand(
                "Bots", false, "shutdown", "shutdown", "Shutdown bots and exit", HandleShutdown);

            m_console.Commands.AddCommand(
                "Bots", false, "quit", "quit", "Shutdown bots and exit", HandleShutdown);

            m_console.Commands.AddCommand(
                "Bots", false, "connect", "connect [<n>]", "Connect bots",
                "If an <n> is given, then the first <n> disconnected bots by postfix number are connected.\n"
                    + "If no <n> is given, then all currently disconnected bots are connected.",
                HandleConnect);

            m_console.Commands.AddCommand(
                "Bots", false, "disconnect", "disconnect [<n>]", "Disconnect bots",
                "Disconnecting bots will interupt any bot connection process, including connection on startup.\n"
                    + "If an <n> is given, then the last <n> connected bots by postfix number are disconnected.\n"
                    + "If no <n> is given, then all currently connected bots are disconnected.",
                HandleDisconnect);

            m_console.Commands.AddCommand(
                "Bots", false, "add behaviour", "add behaviour <abbreviated-name> [<bot-number>]",
                "Add a behaviour to a bot",
                "If no bot number is specified then behaviour is added to all bots.\n"
                    + "Can be performed on connected or disconnected bots.",
                HandleAddBehaviour);

            m_console.Commands.AddCommand(
                "Bots", false, "remove behaviour", "remove behaviour <abbreviated-name> [<bot-number>]",
                "Remove a behaviour from a bot",
                "If no bot number is specified then behaviour is added to all bots.\n"
                    + "Can be performed on connected or disconnected bots.",
                HandleRemoveBehaviour);

            m_console.Commands.AddCommand(
                "Bots", false, "sit", "sit", "Sit all bots on the ground.",
                HandleSit);

            m_console.Commands.AddCommand(
                "Bots", false, "stand", "stand", "Stand all bots.",
                HandleStand);

            m_console.Commands.AddCommand(
                "Bots", false, "set bots", "set bots <key> <value>", "Set a setting for all bots.", HandleSetBots);

            m_console.Commands.AddCommand(
                "Bots", false, "show regions", "show regions", "Show regions known to bots", HandleShowRegions);

            m_console.Commands.AddCommand(
                "Bots", false, "show bots", "show bots", "Shows the status of all bots", HandleShowBotsStatus);

            m_console.Commands.AddCommand(
                "Bots", false, "show bot", "show bot <bot-number>",
                "Shows the detailed status and settings of a particular bot.", HandleShowBotStatus);

            m_bots = new List<Bot>();
        }

        /// <summary>
        /// Track the assets we have and have not received so we don't endlessly repeat requests.
        /// </summary>
        public Dictionary<UUID, bool> AssetsReceived { get; private set; }

        /// <summary>
        /// Is pCampbot in the process of connecting bots?
        /// </summary>
        public bool ConnectingBots { get; private set; }

        /// <summary>
        /// Is pCampbot in the process of disconnecting bots?
        /// </summary>
        public bool DisconnectingBots { get; private set; }

        /// <summary>
        /// Controls whether bots request textures for the object information they receive
        /// </summary>
        public bool InitBotRequestObjectTextures { get; set; }

        /// <summary>
        /// Controls whether bots start out sending agent updates on connection.
        /// </summary>
        public bool InitBotSendAgentUpdates { get; set; }

        /// <summary>
        /// Delay between logins of multiple bots.
        /// </summary>
        /// <remarks>TODO: This value needs to be configurable by a command line argument.</remarks>
        public int LoginDelay { get; set; }

        /// <summary>
        /// The regions that we know about.
        /// </summary>
        public Dictionary<ulong, GridRegion> RegionsKnown { get; private set; }

        /// <summary>
        /// Random number generator.
        /// </summary>
        public Random Rng { get; private set; }

        public void ConnectBots(int botcount)
        {
            ConnectingBots = true;

            Thread connectBotThread = new Thread(o => ConnectBotsInternal(botcount));

            connectBotThread.Name = "Bots connection thread";
            connectBotThread.Start();
        }

        /// <summary>
        /// This creates a bot but does not start it.
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="behaviours">Behaviours for this bot to perform.</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="password">Password</param>
        /// <param name="loginUri">Login URI</param>
        /// <param name="startLocation">Location to start the bot.  Can be "last", "home" or a specific sim name.</param>
        /// <param name="wearSetting"></param>
        public void CreateBot(
             BotManager bm, List<IBehaviour> behaviours,
             string firstName, string lastName, string password, string loginUri, string startLocation, string wearSetting)
        {
            MainConsole.Instance.OutputFormat(
                "[BOT MANAGER]: Creating bot {0} {1}, behaviours are {2}",
                firstName, lastName, string.Join(",", behaviours.ConvertAll<string>(b => b.Name).ToArray()));

            Bot pb = new Bot(bm, behaviours, firstName, lastName, password, startLocation, loginUri);
            pb.wear = wearSetting;
            pb.Client.Settings.SEND_AGENT_UPDATES = InitBotSendAgentUpdates;
            pb.RequestObjectTextures = InitBotRequestObjectTextures;

            pb.OnConnected += handlebotEvent;
            pb.OnDisconnected += handlebotEvent;

            m_bots.Add(pb);
        }

        /// <summary>
        /// Startup number of bots specified in the starting arguments
        /// </summary>
        /// <param name="botcount">How many bots to start up</param>
        /// <param name="cs">The configuration for the bots to use</param>
        public void CreateBots(int botcount, IConfig startupConfig)
        {
            m_firstName = startupConfig.GetString("firstname");
            m_lastNameStem = startupConfig.GetString("lastname");
            m_password = startupConfig.GetString("password");
            m_loginUri = startupConfig.GetString("loginuri");
            m_fromBotNumber = startupConfig.GetInt("from", 0);
            m_wearSetting = startupConfig.GetString("wear", "no");

            m_startUri = ParseInputStartLocationToUri(startupConfig.GetString("start", "last"));

            Array.ForEach<string>(
                startupConfig.GetString("behaviours", "p").Split(new char[] { ',' }), b => m_defaultBehaviourSwitches.Add(b));

            for (int i = 0; i < botcount; i++)
            {
                lock (m_bots)
                {
                    string lastName = string.Format("{0}_{1}", m_lastNameStem, i + m_fromBotNumber);

                    CreateBot(
                        this,
                        CreateBehavioursFromAbbreviatedNames(m_defaultBehaviourSwitches),
                        m_firstName, lastName, m_password, m_loginUri, m_startUri, m_wearSetting);
                }
            }
        }

        internal void Grid_GridRegion(object o, GridRegionEventArgs args)
        {
            lock (RegionsKnown)
            {
                GridRegion newRegion = args.Region;

                if (RegionsKnown.ContainsKey(newRegion.RegionHandle))
                {
                    return;
                }
                else
                {
                    m_log.DebugFormat(
                        "[BOT MANAGER]: Adding {0} {1} to known regions", newRegion.Name, newRegion.RegionHandle);
                    RegionsKnown[newRegion.RegionHandle] = newRegion;
                }
            }
        }

        /// <summary>
        /// Standard CreateConsole routine
        /// </summary>
        /// <returns></returns>
        protected CommandConsole CreateConsole()
        {
            return new LocalConsole("pCampbot");
        }

        private void ConnectBotsInternal(int botCount)
        {
            MainConsole.Instance.OutputFormat(
                "[BOT MANAGER]: Starting {0} bots connecting to {1}, location {2}, named {3} {4}_<n>",
                botCount,
                m_loginUri,
                m_startUri,
                m_firstName,
                m_lastNameStem);

            MainConsole.Instance.OutputFormat("[BOT MANAGER]: Delay between logins is {0}ms", LoginDelay);
            MainConsole.Instance.OutputFormat("[BOT MANAGER]: BotsSendAgentUpdates is {0}", InitBotSendAgentUpdates);
            MainConsole.Instance.OutputFormat("[BOT MANAGER]: InitBotRequestObjectTextures is {0}", InitBotRequestObjectTextures);

            int connectedBots = 0;

            for (int i = 0; i < m_bots.Count; i++)
            {
                lock (m_bots)
                {
                    if (DisconnectingBots)
                    {
                        MainConsole.Instance.Output(
                            "[BOT MANAGER]: Aborting bot connection due to user-initiated disconnection");
                        break;
                    }

                    if (m_bots[i].ConnectionState == ConnectionState.Disconnected)
                    {
                        m_bots[i].Connect();
                        connectedBots++;

                        if (connectedBots >= botCount)
                            break;

                        // Stagger logins
                        Thread.Sleep(LoginDelay);
                    }
                }
            }

            ConnectingBots = false;
        }

        private List<IBehaviour> CreateBehavioursFromAbbreviatedNames(HashSet<string> abbreviatedNames)
        {
            // We must give each bot its own list of instantiated behaviours since they store state.
            List<IBehaviour> behaviours = new List<IBehaviour>();

            // Hard-coded for now
            foreach (string abName in abbreviatedNames)
            {
                IBehaviour newBehaviour = null;

                if (abName == "c")
                    newBehaviour = new CrossBehaviour();

                if (abName == "g")
                    newBehaviour = new GrabbingBehaviour();

                if (abName == "n")
                    newBehaviour = new NoneBehaviour();

                if (abName == "p")
                    newBehaviour = new PhysicsBehaviour();

                if (abName == "t")
                    newBehaviour = new TeleportBehaviour();

                if (abName == "tw")
                    newBehaviour = new TwitchyBehaviour();

                if (abName == "ph2")
                    newBehaviour = new PhysicsBehaviour2();

                if (newBehaviour != null)
                {
                    behaviours.Add(newBehaviour);
                }
                else
                {
                    MainConsole.Instance.OutputFormat("No behaviour with abbreviated name {0} found", abName);
                }
            }

            return behaviours;
        }

        private string GenerateBotNameFromNumber(int botNumber)
        {
            return string.Format("{0} {1}_{2}", m_firstName, m_lastNameStem, botNumber);
        }

        /// <summary>
        /// Get a specific bot from its number.
        /// </summary>
        /// <returns>null if no bot was found</returns>
        /// <param name='botNumber'></param>
        private Bot GetBotFromNumber(int botNumber)
        {
            string name = GenerateBotNameFromNumber(botNumber);

            Bot bot;

            lock (m_bots)
                bot = m_bots.Find(b => b.Name == name);

            return bot;
        }

        private void HandleAddBehaviour(string module, string[] cmd)
        {
            if (cmd.Length < 3 || cmd.Length > 4)
            {
                MainConsole.Instance.OutputFormat("Usage: add behaviour <abbreviated-behaviour> [<bot-number>]");
                return;
            }

            string rawBehaviours = cmd[2];

            List<Bot> botsToEffect = new List<Bot>();

            if (cmd.Length == 3)
            {
                lock (m_bots)
                    botsToEffect.AddRange(m_bots);
            }
            else
            {
                int botNumber;
                if (!ConsoleUtil.TryParseConsoleNaturalInt(MainConsole.Instance, cmd[3], out botNumber))
                    return;

                Bot bot = GetBotFromNumber(botNumber);

                if (bot == null)
                {
                    MainConsole.Instance.OutputFormat("Error: No bot found with number {0}", botNumber);
                    return;
                }

                botsToEffect.Add(bot);
            }

            HashSet<string> rawAbbreviatedSwitchesToAdd = new HashSet<string>();
            Array.ForEach<string>(rawBehaviours.Split(new char[] { ',' }), b => rawAbbreviatedSwitchesToAdd.Add(b));

            foreach (Bot bot in botsToEffect)
            {
                List<IBehaviour> behavioursAdded = new List<IBehaviour>();

                foreach (IBehaviour behaviour in CreateBehavioursFromAbbreviatedNames(rawAbbreviatedSwitchesToAdd))
                {
                    if (bot.AddBehaviour(behaviour))
                        behavioursAdded.Add(behaviour);
                }

                MainConsole.Instance.OutputFormat(
                    "Added behaviours {0} to bot {1}",
                    string.Join(", ", behavioursAdded.ConvertAll<string>(b => b.Name).ToArray()), bot.Name);
            }
        }

        /// <summary>
        /// High level connnected/disconnected events so we can keep track of our threads by proxy
        /// </summary>
        /// <param name="callbot"></param>
        /// <param name="eventt"></param>
        private void handlebotEvent(Bot callbot, EventType eventt)
        {
            switch (eventt)
            {
                case EventType.CONNECTED:
                    {
                        m_log.Info("[" + callbot.FirstName + " " + callbot.LastName + "]: Connected");
                        break;
                    }

                case EventType.DISCONNECTED:
                    {
                        m_log.Info("[" + callbot.FirstName + " " + callbot.LastName + "]: Disconnected");
                        break;
                    }
            }
        }

        private void HandleConnect(string module, string[] cmd)
        {
            if (ConnectingBots)
            {
                MainConsole.Instance.Output("Still connecting bots.  Please wait for previous process to complete.");
                return;
            }

            lock (m_bots)
            {
                int botsToConnect;
                int disconnectedBots = m_bots.Count(b => b.ConnectionState == ConnectionState.Disconnected);

                if (cmd.Length == 1)
                {
                    botsToConnect = disconnectedBots;
                }
                else
                {
                    if (!ConsoleUtil.TryParseConsoleNaturalInt(MainConsole.Instance, cmd[1], out botsToConnect))
                        return;

                    botsToConnect = Math.Min(botsToConnect, disconnectedBots);
                }

                MainConsole.Instance.OutputFormat("Connecting {0} bots", botsToConnect);

                ConnectBots(botsToConnect);
            }
        }

        private void HandleDisconnect(string module, string[] cmd)
        {
            lock (m_bots)
            {
                int botsToDisconnect;
                int connectedBots = m_bots.Count(b => b.ConnectionState == ConnectionState.Connected);

                if (cmd.Length == 1)
                {
                    botsToDisconnect = connectedBots;
                }
                else
                {
                    if (!ConsoleUtil.TryParseConsoleNaturalInt(MainConsole.Instance, cmd[1], out botsToDisconnect))
                        return;

                    botsToDisconnect = Math.Min(botsToDisconnect, connectedBots);
                }

                DisconnectingBots = true;

                MainConsole.Instance.OutputFormat("Disconnecting {0} bots", botsToDisconnect);

                int disconnectedBots = 0;

                for (int i = m_bots.Count - 1; i >= 0; i--)
                {
                    if (disconnectedBots >= botsToDisconnect)
                        break;

                    Bot thisBot = m_bots[i];

                    if (thisBot.ConnectionState == ConnectionState.Connected)
                    {
                        Util.FireAndForget(o => thisBot.Disconnect());
                        disconnectedBots++;
                    }
                }

                DisconnectingBots = false;
            }
        }

        private void HandleRemoveBehaviour(string module, string[] cmd)
        {
            if (cmd.Length < 3 || cmd.Length > 4)
            {
                MainConsole.Instance.OutputFormat("Usage: remove behaviour <abbreviated-behaviour> [<bot-number>]");
                return;
            }

            string rawBehaviours = cmd[2];

            List<Bot> botsToEffect = new List<Bot>();

            if (cmd.Length == 3)
            {
                lock (m_bots)
                    botsToEffect.AddRange(m_bots);
            }
            else
            {
                int botNumber;
                if (!ConsoleUtil.TryParseConsoleNaturalInt(MainConsole.Instance, cmd[3], out botNumber))
                    return;

                Bot bot = GetBotFromNumber(botNumber);

                if (bot == null)
                {
                    MainConsole.Instance.OutputFormat("Error: No bot found with number {0}", botNumber);
                    return;
                }

                botsToEffect.Add(bot);
            }

            HashSet<string> abbreviatedBehavioursToRemove = new HashSet<string>();
            Array.ForEach<string>(rawBehaviours.Split(new char[] { ',' }), b => abbreviatedBehavioursToRemove.Add(b));

            foreach (Bot bot in botsToEffect)
            {
                List<IBehaviour> behavioursRemoved = new List<IBehaviour>();

                foreach (string b in abbreviatedBehavioursToRemove)
                {
                    IBehaviour behaviour;

                    if (bot.TryGetBehaviour(b, out behaviour))
                    {
                        bot.RemoveBehaviour(b);
                        behavioursRemoved.Add(behaviour);
                    }
                }

                MainConsole.Instance.OutputFormat(
                    "Removed behaviours {0} to bot {1}",
                    string.Join(", ", behavioursRemoved.ConvertAll<string>(b => b.Name).ToArray()), bot.Name);
            }
        }

        private void HandleSetBots(string module, string[] cmd)
        {
            string key = cmd[2];
            string rawValue = cmd[3];

            if (key == "SEND_AGENT_UPDATES")
            {
                bool newSendAgentUpdatesSetting;

                if (!ConsoleUtil.TryParseConsoleBool(MainConsole.Instance, rawValue, out newSendAgentUpdatesSetting))
                    return;

                MainConsole.Instance.OutputFormat(
                    "Setting SEND_AGENT_UPDATES to {0} for all bots", newSendAgentUpdatesSetting);

                lock (m_bots)
                    m_bots.ForEach(b => b.Client.Settings.SEND_AGENT_UPDATES = newSendAgentUpdatesSetting);
            }
            else
            {
                MainConsole.Instance.Output("Error: Only setting currently available is SEND_AGENT_UPDATES");
            }
        }

        private void HandleShowBotsStatus(string module, string[] cmd)
        {
            ConsoleDisplayTable cdt = new ConsoleDisplayTable();
            cdt.AddColumn("Name", 24);
            cdt.AddColumn("Region", 24);
            cdt.AddColumn("Status", 13);
            cdt.AddColumn("Conns", 5);
            cdt.AddColumn("Behaviours", 20);

            Dictionary<ConnectionState, int> totals = new Dictionary<ConnectionState, int>();
            foreach (object o in Enum.GetValues(typeof(ConnectionState)))
                totals[(ConnectionState)o] = 0;

            lock (m_bots)
            {
                foreach (Bot bot in m_bots)
                {
                    Simulator currentSim = bot.Client.Network.CurrentSim;
                    totals[bot.ConnectionState]++;

                    cdt.AddRow(
                        bot.Name,
                        currentSim != null ? currentSim.Name : "(none)",
                        bot.ConnectionState,
                        bot.SimulatorsCount,
                        string.Join(",", bot.Behaviours.Keys.ToArray()));
                }
            }

            MainConsole.Instance.Output(cdt.ToString());

            ConsoleDisplayList cdl = new ConsoleDisplayList();

            foreach (KeyValuePair<ConnectionState, int> kvp in totals)
                cdl.AddRow(kvp.Key, kvp.Value);

            MainConsole.Instance.Output(cdl.ToString());
        }

        private void HandleShowBotStatus(string module, string[] cmd)
        {
            if (cmd.Length != 3)
            {
                MainConsole.Instance.Output("Usage: show bot <n>");
                return;
            }

            int botNumber;

            if (!ConsoleUtil.TryParseConsoleInt(MainConsole.Instance, cmd[2], out botNumber))
                return;

            Bot bot = GetBotFromNumber(botNumber);

            if (bot == null)
            {
                MainConsole.Instance.OutputFormat("Error: No bot found with number {0}", botNumber);
                return;
            }

            ConsoleDisplayList cdl = new ConsoleDisplayList();
            cdl.AddRow("Name", bot.Name);
            cdl.AddRow("Status", bot.ConnectionState);

            Simulator currentSim = bot.Client.Network.CurrentSim;
            cdl.AddRow("Region", currentSim != null ? currentSim.Name : "(none)");

            List<Simulator> connectedSimulators = bot.Simulators;
            List<string> simulatorNames = connectedSimulators.ConvertAll<string>(cs => cs.Name);
            cdl.AddRow("Connections", string.Join(", ", simulatorNames.ToArray()));

            MainConsole.Instance.Output(cdl.ToString());

            MainConsole.Instance.Output("Settings");

            ConsoleDisplayList statusCdl = new ConsoleDisplayList();

            statusCdl.AddRow(
                "Behaviours",
                string.Join(", ", bot.Behaviours.Values.ToList().ConvertAll<string>(b => b.Name).ToArray()));

            GridClient botClient = bot.Client;
            statusCdl.AddRow("SEND_AGENT_UPDATES", botClient.Settings.SEND_AGENT_UPDATES);

            MainConsole.Instance.Output(statusCdl.ToString());
        }

        private void HandleShowRegions(string module, string[] cmd)
        {
            string outputFormat = "{0,-30}  {1, -20}  {2, -5}  {3, -5}";
            MainConsole.Instance.OutputFormat(outputFormat, "Name", "Handle", "X", "Y");

            lock (RegionsKnown)
            {
                foreach (GridRegion region in RegionsKnown.Values)
                {
                    MainConsole.Instance.OutputFormat(
                        outputFormat, region.Name, region.RegionHandle, region.X, region.Y);
                }
            }
        }

        private void HandleShutdown(string module, string[] cmd)
        {
            lock (m_bots)
            {
                int connectedBots = m_bots.Count(b => b.ConnectionState == ConnectionState.Connected);

                if (connectedBots > 0)
                {
                    MainConsole.Instance.OutputFormat("Please disconnect {0} connected bots first", connectedBots);
                    return;
                }
            }

            MainConsole.Instance.Output("Shutting down");

            Environment.Exit(0);
        }

        private void HandleSit(string module, string[] cmd)
        {
            lock (m_bots)
            {
                m_bots.ForEach(b => b.SitOnGround());
            }
        }

        private void HandleStand(string module, string[] cmd)
        {
            lock (m_bots)
            {
                m_bots.ForEach(b => b.Stand());
            }
        }

        /// <summary>
        /// Parses the command line start location to a start string/uri that the login mechanism will recognize.
        /// </summary>
        /// <returns>
        /// The input start location to URI.
        /// </returns>
        /// <param name='startLocation'>
        /// Start location.
        /// </param>
        private string ParseInputStartLocationToUri(string startLocation)
        {
            if (startLocation == "home" || startLocation == "last")
                return startLocation;

            string regionName;

            // Just a region name or only one (!) extra component.  Like a viewer, we will stick 128/128/0 on the end
            Vector3 startPos = new Vector3(128, 128, 0);

            string[] startLocationComponents = startLocation.Split('/');

            regionName = startLocationComponents[0];

            if (startLocationComponents.Length >= 2)
            {
                float.TryParse(startLocationComponents[1], out startPos.X);

                if (startLocationComponents.Length >= 3)
                {
                    float.TryParse(startLocationComponents[2], out startPos.Y);

                    if (startLocationComponents.Length >= 4)
                        float.TryParse(startLocationComponents[3], out startPos.Z);
                }
            }

            return string.Format("uri:{0}&{1}&{2}&{3}", regionName, startPos.X, startPos.Y, startPos.Z);
        }
    }
}