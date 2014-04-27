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
    public struct GroupActiveProposals
    {
        public string EndDateTime;
        public string Majority;
        public string ProposalText;
        public string Quorum;
        public string StartDateTime;
        public string TerseDateID;
        public string VoteID;
        public string VoteInitiator;
    }

    public struct GroupMembersData
    {
        public bool AcceptNotices;
        public UUID AgentID;
        public ulong AgentPowers;
        public int Contribution;
        public bool IsOwner;
        public bool ListInProfile;
        public string OnlineStatus;
        public string Title;
    }

    public struct GroupNoticeData
    {
        public byte AssetType;
        public string FromName;
        public bool HasAttachment;
        public UUID NoticeID;
        public string Subject;
        public uint Timestamp;
    }

    public struct GroupProfileData
    {
        public bool AllowPublish;
        public string Charter;
        public UUID FounderID;
        public UUID GroupID;
        public int GroupMembershipCount;
        public int GroupRolesCount;
        public UUID InsigniaID;
        public bool MaturePublish;
        public int MembershipFee;
        public string MemberTitle;
        public int Money;
        public string Name;
        public bool OpenEnrollment;
        public UUID OwnerRole;
        public ulong PowersMask;
        public bool ShowInList;
    }

    public struct GroupRoleMembersData
    {
        public UUID MemberID;
        public UUID RoleID;
    }

    public struct GroupRolesData
    {
        public string Description;
        public int Members;
        public string Name;
        public ulong Powers;
        public UUID RoleID;
        public string Title;
    }

    public struct GroupTitlesData
    {
        public string Name;
        public bool Selected;
        public UUID UUID;
    }

    public struct GroupVoteHistory
    {
        public string EndDateTime;
        public string Majority;
        public string ProposalText;
        public string Quorum;
        public string StartDateTime;
        public string TerseDateID;
        public string VoteID;
        public string VoteInitiator;
        public string VoteResult;
        public string VoteType;
    }

    public class GroupMembershipData
    {
        // Per user data
        public bool AcceptNotices = true;

        public bool Active = false;

        public UUID ActiveRole = UUID.Zero;

        public bool AllowPublish = true;

        public string Charter;

        public int Contribution = 0;

        public UUID FounderID = UUID.Zero;

        // Group base data
        public UUID GroupID;

        public string GroupName;
        public UUID GroupPicture = UUID.Zero;
        public ulong GroupPowers = 0;
        public string GroupTitle;
        public bool ListInProfile = false;
        public bool MaturePublish = true;
        public int MembershipFee = 0;
        public bool OpenEnrollment = true;
        public bool ShowInList = true;
    }

    public class GroupRecord
    {
        public bool AllowPublish = true;
        public string Charter;
        public UUID FounderID = UUID.Zero;
        public UUID GroupID;
        public string GroupName;
        public UUID GroupPicture = UUID.Zero;
        public bool MaturePublish = true;
        public int MembershipFee = 0;
        public bool OpenEnrollment = true;
        public UUID OwnerRoleID = UUID.Zero;
        public bool ShowInList = false;
    }
}