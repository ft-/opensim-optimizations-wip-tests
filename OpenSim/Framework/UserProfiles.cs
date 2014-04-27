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

namespace OpenSim.Framework
{
    public class UserAccountAuth
    {
        public string Password = string.Empty;
        public string UserId = UUID.Zero.ToString();
    }

    public class UserAccountProperties
    {
        public string EmailAddress = string.Empty;
        public string Firstname = string.Empty;
        public string LastName = string.Empty;
        public string Password = string.Empty;
        public string UserId = string.Empty;
    }

    public class UserAppData
    {
        public string DataKey = string.Empty;
        public string DataVal = string.Empty;
        public string TagId = string.Empty;
        public string UserId = UUID.Zero.ToString();
    }

    public class UserClassifiedAdd
    {
        public int Category = 0;
        public UUID ClassifiedId = UUID.Zero;
        public int CreationDate = 0;
        public UUID CreatorId = UUID.Zero;
        public string Description = string.Empty;
        public int ExpirationDate = 0;
        public byte Flags = 0;
        public string GlobalPos = "<0,0,0>";
        public string Name = string.Empty;
        public UUID ParcelId = UUID.Zero;
        public string ParcelName = string.Empty;
        public int ParentEstate = 0;
        public int Price = 0;
        public string SimName = string.Empty;
        public UUID SnapshotId = UUID.Zero;
    }

    public class UserPreferences
    {
        public string EMail = string.Empty;
        public bool IMViaEmail = false;
        public UUID UserId;
        public bool Visible = false;
    }

    public class UserProfileNotes
    {
        public string Notes;
        public UUID TargetId;
        public UUID UserId;
    }

    public class UserProfilePick
    {
        public UUID CreatorId = UUID.Zero;
        public string Desc = string.Empty;
        public bool Enabled = false;
        public string GlobalPos = "<0,0,0>";
        public string Name = string.Empty;
        public string OriginalName = string.Empty;
        public UUID ParcelId = UUID.Zero;
        public UUID PickId = UUID.Zero;
        public string SimName = string.Empty;
        public UUID SnapshotId = UUID.Zero;
        public int SortOrder = 0;
        public bool TopPick = false;
        public string User = string.Empty;
    }

    public class UserProfileProperties
    {
        public string AboutText = string.Empty;
        public UUID FirstLifeImageId = UUID.Zero;
        public string FirstLifeText = string.Empty;
        public UUID ImageId = UUID.Zero;
        public string Language = string.Empty;
        public UUID PartnerId = UUID.Zero;
        public bool PublishMature = false;
        public bool PublishProfile = false;
        public int SkillsMask = 0;
        public string SkillsText = string.Empty;
        public UUID UserId = UUID.Zero;
        public int WantToMask = 0;
        public string WantToText = string.Empty;
        public string WebUrl = string.Empty;
    }
}