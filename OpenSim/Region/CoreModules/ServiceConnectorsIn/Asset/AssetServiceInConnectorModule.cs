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
using Mono.Addins;
using Nini.Config;
using OpenSim.Framework.Servers;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Server.Base;
using OpenSim.Server.Handlers.Base;
using System;
using System.Reflection;

namespace OpenSim.Region.CoreModules.ServiceConnectorsIn.Asset
{
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule", Id = "AssetServiceInConnectorModule")]
    public class AssetServiceInConnectorModule : ISharedRegionModule
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool m_Enabled = false;

        private IConfigSource m_Config;
        private bool m_Registered = false;

        #region Region Module interface

        public string Name
        {
            get { return "RegionAssetService"; }
        }

        public Type ReplaceableInterface
        {
            get { return null; }
        }

        public void AddRegion(Scene scene)
        {
            if (!m_Enabled)
                return;

            if (!m_Registered)
            {
                m_Registered = true;

                m_log.Info("[HGAssetService]: Starting...");

                Object[] args = new Object[] { m_Config, MainServer.Instance, "HGAssetService" };

                ServerUtils.LoadPlugin<IServiceConnector>("OpenSim.Server.Handlers.dll:AssetServiceConnector", args);
            }
        }

        public void Close()
        {
        }

        public void Initialise(IConfigSource config)
        {
            m_Config = config;
            IConfig moduleConfig = config.Configs["Modules"];
            if (moduleConfig != null)
            {
                m_Enabled = moduleConfig.GetBoolean("AssetServiceInConnector", false);
                if (m_Enabled)
                {
                    m_log.Info("[ASSET IN CONNECTOR]: AssetServiceInConnector enabled");
                }
            }
        }

        public void PostInitialise()
        {
        }

        public void RegionLoaded(Scene scene)
        {
        }

        public void RemoveRegion(Scene scene)
        {
        }

        #endregion Region Module interface
    }
}