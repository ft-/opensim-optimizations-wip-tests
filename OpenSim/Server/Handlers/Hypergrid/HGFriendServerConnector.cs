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

using Nini.Config;
using OpenSim.Framework.Servers.HttpServer;
using OpenSim.Server.Base;
using OpenSim.Server.Handlers.Base;
using OpenSim.Services.Interfaces;
using System;

namespace OpenSim.Server.Handlers.Hypergrid
{
    public class HGFriendsServerConnector : ServiceConnector
    {
        private string m_ConfigName = "HGFriendsService";
        private IHGFriendsService m_TheService;
        private IUserAgentService m_UserAgentService;

        // Called from Robust
        public HGFriendsServerConnector(IConfigSource config, IHttpServer server, string configName) :
            this(config, server, configName, null)
        {
        }

        // Called from standalone configurations
        public HGFriendsServerConnector(IConfigSource config, IHttpServer server, string configName, IFriendsSimConnector localConn)
            : base(config, server, configName)
        {
            if (configName != string.Empty)
                m_ConfigName = configName;

            Object[] args = new Object[] { config, m_ConfigName, localConn };

            IConfig serverConfig = config.Configs[m_ConfigName];
            if (serverConfig == null)
                throw new Exception(String.Format("No section {0} in config file", m_ConfigName));

            string theService = serverConfig.GetString("LocalServiceModule",
                    String.Empty);
            if (theService == String.Empty)
                throw new Exception("No LocalServiceModule in config file");
            m_TheService = ServerUtils.LoadPlugin<IHGFriendsService>(theService, args);

            theService = serverConfig.GetString("UserAgentService", string.Empty);
            if (theService == String.Empty)
                throw new Exception("No UserAgentService in " + m_ConfigName);
            m_UserAgentService = ServerUtils.LoadPlugin<IUserAgentService>(theService, new Object[] { config, localConn });

            server.AddStreamHandler(new HGFriendsServerPostHandler(m_TheService, m_UserAgentService, localConn));
        }
    }
}