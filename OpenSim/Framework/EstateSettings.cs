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

using OpenMetaverse;
using System;
using System.Collections.Generic;

namespace OpenSim.Framework
{
    public class EstateSettings
    {
        // private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<UUID> l_EstateAccess = new List<UUID>();

        private List<EstateBan> l_EstateBans = new List<EstateBan>();

        private List<UUID> l_EstateGroups = new List<UUID>();

        // All those lists...
        //
        private List<UUID> l_EstateManagers = new List<UUID>();

        private string m_AbuseEmail = String.Empty;

        private bool m_AbuseEmailToEstateOwner = false;

        private bool m_AllowDirectTeleport = true;

        private bool m_AllowLandmark = true;

        private bool m_AllowParcelChanges = true;

        private bool m_AllowSetHome = true;

        private bool m_AllowVoice = true;

        private float m_BillableFactor = 0.0f;

        private bool m_BlockDwell = false;

        private bool m_DenyAnonymous = false;

        private bool m_DenyIdentified = false;

        private bool m_DenyMinors = false;

        private bool m_DenyTransacted = false;

        // Only the client uses these
        //
        private uint m_EstateID = 0;

        private string m_EstateName = "My Estate";

        private UUID m_EstateOwner = UUID.Zero;

        private bool m_EstateSkipScripts = false;

        private bool m_FixedSun = false;

        private uint m_ParentEstateID = 1;

        private int m_PricePerMeter = 1;

        private bool m_PublicAccess = true;

        private int m_RedirectGridX = 0;

        private int m_RedirectGridY = 0;

        private bool m_ResetHomeOnTeleport = false;

        private double m_SunPosition = 0.0;

        private bool m_TaxFree = false;

        // Used by the sim
        //
        private bool m_UseGlobalTime = true;

        public EstateSettings()
        {
        }

        public delegate void SaveDelegate(EstateSettings rs);

        public event SaveDelegate OnSave;
        public string AbuseEmail
        {
            get { return m_AbuseEmail; }
            set { m_AbuseEmail = value; }
        }

        public bool AbuseEmailToEstateOwner
        {
            get { return m_AbuseEmailToEstateOwner; }
            set { m_AbuseEmailToEstateOwner = value; }
        }

        public bool AllowDirectTeleport
        {
            get { return m_AllowDirectTeleport; }
            set { m_AllowDirectTeleport = value; }
        }

        public bool AllowLandmark
        {
            get { return m_AllowLandmark; }
            set { m_AllowLandmark = value; }
        }

        public bool AllowParcelChanges
        {
            get { return m_AllowParcelChanges; }
            set { m_AllowParcelChanges = value; }
        }

        public bool AllowSetHome
        {
            get { return m_AllowSetHome; }
            set { m_AllowSetHome = value; }
        }

        public bool AllowVoice
        {
            get { return m_AllowVoice; }
            set { m_AllowVoice = value; }
        }

        public float BillableFactor
        {
            get { return m_BillableFactor; }
            set { m_BillableFactor = value; }
        }

        public bool BlockDwell
        {
            get { return m_BlockDwell; }
            set { m_BlockDwell = value; }
        }

        public bool DenyAnonymous
        {
            get { return m_DenyAnonymous; }
            set { m_DenyAnonymous = value; }
        }

        public bool DenyIdentified
        {
            get { return m_DenyIdentified; }
            set { m_DenyIdentified = value; }
        }

        public bool DenyMinors
        {
            get { return m_DenyMinors; }
            set { m_DenyMinors = value; }
        }

        public bool DenyTransacted
        {
            get { return m_DenyTransacted; }
            set { m_DenyTransacted = value; }
        }

        public UUID[] EstateAccess
        {
            get { return l_EstateAccess.ToArray(); }
            set { l_EstateAccess = new List<UUID>(value); }
        }

        public EstateBan[] EstateBans
        {
            get { return l_EstateBans.ToArray(); }
            set { l_EstateBans = new List<EstateBan>(value); }
        }

        public UUID[] EstateGroups
        {
            get { return l_EstateGroups.ToArray(); }
            set { l_EstateGroups = new List<UUID>(value); }
        }

        public uint EstateID
        {
            get { return m_EstateID; }
            set { m_EstateID = value; }
        }
        public UUID[] EstateManagers
        {
            get { return l_EstateManagers.ToArray(); }
            set { l_EstateManagers = new List<UUID>(value); }
        }

        public string EstateName
        {
            get { return m_EstateName; }
            set { m_EstateName = value; }
        }
        public UUID EstateOwner
        {
            get { return m_EstateOwner; }
            set { m_EstateOwner = value; }
        }

        public bool EstateSkipScripts
        {
            get { return m_EstateSkipScripts; }
            set { m_EstateSkipScripts = value; }
        }

        public bool FixedSun
        {
            get { return m_FixedSun; }
            set { m_FixedSun = value; }
        }

        public uint ParentEstateID
        {
            get { return m_ParentEstateID; }
            set { m_ParentEstateID = value; }
        }
        public int PricePerMeter
        {
            get { return m_PricePerMeter; }
            set { m_PricePerMeter = value; }
        }
        public bool PublicAccess
        {
            get { return m_PublicAccess; }
            set { m_PublicAccess = value; }
        }

        public int RedirectGridX
        {
            get { return m_RedirectGridX; }
            set { m_RedirectGridX = value; }
        }
        public int RedirectGridY
        {
            get { return m_RedirectGridY; }
            set { m_RedirectGridY = value; }
        }
        public bool ResetHomeOnTeleport
        {
            get { return m_ResetHomeOnTeleport; }
            set { m_ResetHomeOnTeleport = value; }
        }

        public double SunPosition
        {
            get { return m_SunPosition; }
            set { m_SunPosition = value; }
        }

        public bool TaxFree
        {
            get { return m_TaxFree; }
            set { m_TaxFree = value; }
        }

        public bool UseGlobalTime
        {
            get { return m_UseGlobalTime; }
            set { m_UseGlobalTime = value; }
        }
        public void AddBan(EstateBan ban)
        {
            if (ban == null)
                return;
            if (!IsBanned(ban.BannedUserID))
                l_EstateBans.Add(ban);
        }

        public void AddEstateGroup(UUID avatarID)
        {
            if (avatarID == UUID.Zero)
                return;
            if (!l_EstateGroups.Contains(avatarID))
                l_EstateGroups.Add(avatarID);
        }

        public void AddEstateManager(UUID avatarID)
        {
            if (avatarID == UUID.Zero)
                return;
            if (!l_EstateManagers.Contains(avatarID))
                l_EstateManagers.Add(avatarID);
        }

        public void AddEstateUser(UUID avatarID)
        {
            if (avatarID == UUID.Zero)
                return;
            if (!l_EstateAccess.Contains(avatarID))
                l_EstateAccess.Add(avatarID);
        }

        public void ClearBans()
        {
            l_EstateBans.Clear();
        }

        public bool GroupAccess(UUID groupID)
        {
            return l_EstateGroups.Contains(groupID);
        }

        public bool HasAccess(UUID user)
        {
            if (IsEstateManagerOrOwner(user))
                return true;

            return l_EstateAccess.Contains(user);
        }

        public bool IsBanned(UUID avatarID)
        {
            foreach (EstateBan ban in l_EstateBans)
                if (ban.BannedUserID == avatarID)
                    return true;
            return false;
        }

        public bool IsEstateManagerOrOwner(UUID avatarID)
        {
            if (IsEstateOwner(avatarID))
                return true;

            return l_EstateManagers.Contains(avatarID);
        }

        public bool IsEstateOwner(UUID avatarID)
        {
            if (avatarID == m_EstateOwner)
                return true;

            return false;
        }

        public void RemoveBan(UUID avatarID)
        {
            foreach (EstateBan ban in new List<EstateBan>(l_EstateBans))
                if (ban.BannedUserID == avatarID)
                    l_EstateBans.Remove(ban);
        }

        public void RemoveEstateGroup(UUID avatarID)
        {
            if (l_EstateGroups.Contains(avatarID))
                l_EstateGroups.Remove(avatarID);
        }

        public void RemoveEstateManager(UUID avatarID)
        {
            if (l_EstateManagers.Contains(avatarID))
                l_EstateManagers.Remove(avatarID);
        }

        public void RemoveEstateUser(UUID avatarID)
        {
            if (l_EstateAccess.Contains(avatarID))
                l_EstateAccess.Remove(avatarID);
        }

        public void Save()
        {
            if (OnSave != null)
                OnSave(this);
        }
        public void SetFromFlags(ulong regionFlags)
        {
            ResetHomeOnTeleport = ((regionFlags & (ulong)OpenMetaverse.RegionFlags.ResetHomeOnTeleport) == (ulong)OpenMetaverse.RegionFlags.ResetHomeOnTeleport);
            BlockDwell = ((regionFlags & (ulong)OpenMetaverse.RegionFlags.BlockDwell) == (ulong)OpenMetaverse.RegionFlags.BlockDwell);
            AllowLandmark = ((regionFlags & (ulong)OpenMetaverse.RegionFlags.AllowLandmark) == (ulong)OpenMetaverse.RegionFlags.AllowLandmark);
            AllowParcelChanges = ((regionFlags & (ulong)OpenMetaverse.RegionFlags.AllowParcelChanges) == (ulong)OpenMetaverse.RegionFlags.AllowParcelChanges);
            AllowSetHome = ((regionFlags & (ulong)OpenMetaverse.RegionFlags.AllowSetHome) == (ulong)OpenMetaverse.RegionFlags.AllowSetHome);
        }
    }
}