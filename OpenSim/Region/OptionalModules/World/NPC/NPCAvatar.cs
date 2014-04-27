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
using OpenMetaverse.Packets;
using OpenSim.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using System;
using System.Collections.Generic;
using System.Net;

namespace OpenSim.Region.OptionalModules.World.NPC
{
    public class NPCAvatar : IClientAPI, INPC
    {
        private readonly string m_firstname;

        private readonly string m_lastname;

        private readonly UUID m_ownerID;

        private readonly Scene m_scene;

        private readonly Vector3 m_startPos;

        private readonly UUID m_uuid;

        public NPCAvatar(
            string firstname, string lastname, Vector3 position, UUID ownerID, bool senseAsAgent, Scene scene)
        {
            m_firstname = firstname;
            m_lastname = lastname;
            m_startPos = position;
            m_uuid = UUID.Random();
            m_scene = scene;
            m_ownerID = ownerID;
            SenseAsAgent = senseAsAgent;
        }

        public NPCAvatar(
            string firstname, string lastname, UUID agentID, Vector3 position, UUID ownerID, bool senseAsAgent, Scene scene)
        {
            m_firstname = firstname;
            m_lastname = lastname;
            m_startPos = position;
            m_uuid = agentID;
            m_scene = scene;
            m_ownerID = ownerID;
            SenseAsAgent = senseAsAgent;
        }

        public delegate void ChatToNPC(
            string message, byte type, Vector3 fromPos, string fromName,
            UUID fromAgentID, UUID ownerID, byte source, byte audible);

        /// <summary>
        /// Fired when the NPC receives a chat message.
        /// </summary>
        public event ChatToNPC OnChatToNPC;

        /// <summary>
        /// Fired when the NPC receives an instant message.
        /// </summary>
        public event Action<GridInstantMessage> OnInstantMessageToNPC;

        public UUID OwnerID
        {
            get { return m_ownerID; }
        }

        public Vector3 Position
        {
            get { return m_scene.Entities[m_uuid].AbsolutePosition; }
            set { m_scene.Entities[m_uuid].AbsolutePosition = value; }
        }

        public IScene Scene
        {
            get { return m_scene; }
        }

        public ISceneAgent SceneAgent { get; set; }

        public bool SendLogoutPacketWhenClosing
        {
            set { }
        }

        public bool SenseAsAgent { get; set; }
        public void ActivateGesture(UUID assetId, UUID gestureId)
        {
        }

        public void Broadcast(string message)
        {
            SendOnChatFromClient(0, message, ChatTypeEnum.Broadcast);
        }

        public void DeactivateGesture(UUID assetId, UUID gestureId)
        {
        }

        public string GetClientOption(string option)
        {
            return string.Empty;
        }

        public void GiveMoney(UUID target, int amount)
        {
            OnMoneyTransferRequest(m_uuid, target, amount, 1, "Payment");
        }

        public void InstantMessage(UUID target, string message)
        {
            OnInstantMessage(this, new GridInstantMessage(m_scene,
                    m_uuid, m_firstname + " " + m_lastname,
                    target, 0, false, message,
                    UUID.Zero, false, Position, new byte[0], true));
        }

        public void Say(string message)
        {
            SendOnChatFromClient(0, message, ChatTypeEnum.Say);
        }

        public void Say(int channel, string message)
        {
            SendOnChatFromClient(channel, message, ChatTypeEnum.Say);
        }

        public void SendAcceptCallingCard(UUID transactionID)
        {
        }

        public void SendAdminResponse(UUID Token, uint AdminLevel)
        {
        }

        public void SendAgentOffline(UUID[] agentIDs)
        {
        }

        public void SendAgentOnline(UUID[] agentIDs)
        {
        }

        public void SendAgentTerseUpdate(ISceneEntity presence)
        {
        }

        public void SendAvatarGroupsReply(UUID avatarID, GroupMembershipData[] data)
        {
        }

        public void SendAvatarInterestsReply(UUID avatarID, uint wantMask, string wantText, uint skillsMask, string skillsText, string languages)
        {
        }

        public void SendChangeUserRights(UUID agentID, UUID friendID, int rights)
        {
        }

        public void SendClearFollowCamProperties(UUID objectID)
        {
        }

        public void SendDeclineCallingCard(UUID transactionID)
        {
        }

        public void SendDirClassifiedReply(UUID queryID, DirClassifiedReplyData[] data)
        {
        }

        public void SendDirEventsReply(UUID queryID, DirEventsReplyData[] data)
        {
        }

        public void SendDirGroupsReply(UUID queryID, DirGroupsReplyData[] data)
        {
        }

        public void SendDirLandReply(UUID queryID, DirLandReplyData[] data)
        {
        }

        public void SendDirPeopleReply(UUID queryID, DirPeopleReplyData[] data)
        {
        }

        public void SendDirPlacesReply(UUID queryID, DirPlacesReplyData[] data)
        {
        }

        public void SendDirPopularReply(UUID queryID, DirPopularReplyData[] data)
        {
        }

        public void SendEjectGroupMemberReply(UUID agentID, UUID groupID, bool success)
        {
        }

        public void SendEventInfoReply(EventData info)
        {
        }

        public void SendGroupAccountingDetails(IClientAPI sender, UUID groupID, UUID transactionID, UUID sessionID, int amt)
        {
        }

        public void SendGroupAccountingSummary(IClientAPI sender, UUID groupID, uint moneyAmt, int totalTier, int usedTier)
        {
        }

        public void SendGroupActiveProposals(UUID groupID, UUID transactionID, GroupActiveProposals[] Proposals)
        {
        }

        public void SendGroupMembership(GroupMembershipData[] GroupMembership)
        {
        }

        public void SendGroupTransactionsSummaryDetails(IClientAPI sender, UUID groupID, UUID transactionID, UUID sessionID, int amt)
        {
        }

        public void SendGroupVoteHistory(UUID groupID, UUID transactionID, GroupVoteHistory[] Votes)
        {
        }

        public void SendJoinGroupReply(UUID groupID, bool success)
        {
        }

        public void SendLeaveGroupReply(UUID groupID, bool success)
        {
        }

        public void SendMapItemReply(mapItemReply[] replies, uint mapitemtype, uint flags)
        {
        }

        public void SendOfferCallingCard(UUID destID, UUID transactionID)
        {
        }

        public void SendParcelInfo(RegionInfo info, LandData land, UUID parcelID, uint x, uint y)
        {
        }

        public void SendParcelMediaCommand(uint flags, ParcelMediaCommandEnum command, float time)
        {
        }

        public void SendParcelMediaUpdate(string mediaUrl, UUID mediaTextureID,
                                   byte autoScale, string mediaType, string mediaDesc, int mediaWidth, int mediaHeight,
                                   byte mediaLoop)
        {
        }

        public void SendPartPhysicsProprieties(ISceneEntity entity)
        {
        }

        public void SendPlacesReply(UUID queryID, UUID transactionID, PlacesReplyData[] data)
        {
        }

        public void SendRebakeAvatarTextures(UUID textureID)
        {
        }

        public void SendRegionHandle(UUID regoinID, ulong handle)
        {
        }

        public void SendScriptTeleportRequest(string objName, string simName, Vector3 pos, Vector3 lookAt)
        {
        }

        public void SendSetFollowCamProperties(UUID objectID, SortedDictionary<int, float> parameters)
        {
        }

        public void SendSitResponse(UUID TargetID, Vector3 OffsetPos, Quaternion SitOrientation, bool autopilot,
                                        Vector3 CameraAtOffset, Vector3 CameraEyeOffset, bool ForceMouseLook)
        {
        }

        public void SendTerminateFriend(UUID exFriendID)
        {
        }

        public void SendTextBoxRequest(string message, int chatChannel, string objectname, UUID ownerID, string ownerFirstName, string ownerLastName, UUID objectId)
        {
        }

        public void SetClientOption(string option, string value)
        {
        }

        public void Shout(int channel, string message)
        {
            SendOnChatFromClient(channel, message, ChatTypeEnum.Shout);
        }

        public bool Touch(UUID target)
        {
            SceneObjectPart part = m_scene.GetSceneObjectPart(target);
            if (part == null)
                return false;
            bool objectTouchable = hasTouchEvents(part); // Only touch an object that is scripted to respond
            if (!objectTouchable && !part.IsRoot)
                objectTouchable = hasTouchEvents(part.ParentGroup.RootPart);
            if (!objectTouchable)
                return false;
            // Set up the surface args as if the touch is from a client that does not support this
            SurfaceTouchEventArgs surfaceArgs = new SurfaceTouchEventArgs();
            surfaceArgs.FaceIndex = -1; // TOUCH_INVALID_FACE
            surfaceArgs.Binormal = Vector3.Zero; // TOUCH_INVALID_VECTOR
            surfaceArgs.Normal = Vector3.Zero; // TOUCH_INVALID_VECTOR
            surfaceArgs.STCoord = new Vector3(-1.0f, -1.0f, 0.0f); // TOUCH_INVALID_TEXCOORD
            surfaceArgs.UVCoord = surfaceArgs.STCoord; // TOUCH_INVALID_TEXCOORD
            List<SurfaceTouchEventArgs> touchArgs = new List<SurfaceTouchEventArgs>();
            touchArgs.Add(surfaceArgs);
            Vector3 offset = part.OffsetPosition * -1.0f;
            if (OnGrabObject == null)
                return false;
            OnGrabObject(part.LocalId, offset, this, touchArgs);
            if (OnGrabUpdate != null)
                OnGrabUpdate(part.UUID, offset, part.ParentGroup.RootPart.GroupPosition, this, touchArgs);
            if (OnDeGrabObject != null)
                OnDeGrabObject(part.LocalId, this, touchArgs);
            return true;
        }

        public void Whisper(int channel, string message)
        {
            SendOnChatFromClient(channel, message, ChatTypeEnum.Whisper);
        }
        private bool hasTouchEvents(SceneObjectPart part)
        {
            if ((part.ScriptEvents & scriptEvents.touch) != 0 ||
                (part.ScriptEvents & scriptEvents.touch_start) != 0 ||
                (part.ScriptEvents & scriptEvents.touch_end) != 0)
                return true;
            return false;
        }
        #region Internal Functions

        private void SendOnChatFromClient(int channel, string message, ChatTypeEnum chatType)
        {
            if (channel == 0)
            {
                message = message.Trim();
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }
            }
            OSChatMessage chatFromClient = new OSChatMessage();
            chatFromClient.Channel = channel;
            chatFromClient.From = Name;
            chatFromClient.Message = message;
            chatFromClient.Position = StartPos;
            chatFromClient.Scene = m_scene;
            chatFromClient.Sender = this;
            chatFromClient.SenderUUID = AgentId;
            chatFromClient.Type = chatType;

            OnChatFromClient(this, chatFromClient);
        }

        #endregion Internal Functions

        #region Event Definitions IGNORE

        // disable warning: public events constituting public API
#pragma warning disable 67

        public event AbortXfer OnAbortXfer;

        public event AcceptCallingCard OnAcceptCallingCard;

        public event ActivateGesture OnActivateGesture;

        public event AddNewPrim OnAddPrim;

        public event UpdateAgent OnAgentCameraUpdate;

        public event FetchInventory OnAgentDataUpdateRequest;

        public event AgentRequestSit OnAgentRequestSit;

        public event AgentSit OnAgentSit;

        public event UpdateAgent OnAgentUpdate;

        public event FriendActionDelegate OnApproveFriendRequest;

        public event UDPAssetUploadRequest OnAssetUploadRequest;

        public event Action<Vector3, bool, bool> OnAutoPilotGo;

        public event AvatarInterestUpdate OnAvatarInterestUpdate;

        public event AvatarNotesUpdate OnAvatarNotesUpdate;

        public event AvatarNowWearing OnAvatarNowWearing;

        public event AvatarPickerRequest OnAvatarPickerRequest;

        public event BakeTerrain OnBakeTerrain;

        public event BuyObjectInventory OnBuyObjectInventory;

        public event CachedTextureRequest OnCachedTextureRequest;

        public event ChatMessage OnChatFromClient;

        public event StatusChange OnChildAgentStatus;

        public event ClassifiedDelete OnClassifiedDelete;

        public event ClassifiedDelete OnClassifiedGodDelete;

        public event ClassifiedInfoRequest OnClassifiedInfoRequest;

        public event ClassifiedInfoUpdate OnClassifiedInfoUpdate;

        public event CommitEstateTerrainTextureRequest OnCommitEstateTerrainTextureRequest;

        public event Action<IClientAPI, bool> OnCompleteMovementToRegion;

        public event ConfirmXfer OnConfirmXfer;

        public event Action<IClientAPI> OnConnectionClosed;

        public event CopyInventoryItem OnCopyInventoryItem;

        public event CreateInventoryFolder OnCreateNewInventoryFolder;

        public event CreateNewInventoryItem OnCreateNewInventoryItem;

        public event DeactivateGesture OnDeactivateGesture;

        public event DeclineCallingCard OnDeclineCallingCard;

        public event DeGrabObject OnDeGrabObject;

        public event DelinkObjects OnDelinkObjects;

        public event FriendActionDelegate OnDenyFriendRequest;

        public event DeRezObject OnDeRezObject;

        public event UUIDNameRequest OnDetachAttachmentIntoInv;

        public event DetailedEstateDataRequest OnDetailedEstateDataRequest;

        public event DirClassifiedQuery OnDirClassifiedQuery;

        public event DirFindQuery OnDirFindQuery;

        public event DirLandQuery OnDirLandQuery;

        public event DirPlacesQuery OnDirPlacesQuery;

        public event DirPopularQuery OnDirPopularQuery;

        public event DisconnectUser OnDisconnectUser;

        public event EconomyDataRequest OnEconomyDataRequest;

        public event EstateBlueBoxMessageRequest OnEstateBlueBoxMessageRequest;

        public event EstateChangeCovenantRequest OnEstateChangeCovenantRequest;

        public event EstateChangeInfo OnEstateChangeInfo;

        public event EstateCovenantRequest OnEstateCovenantRequest;

        public event EstateDebugRegionRequest OnEstateDebugRegionRequest;

        public event EstateManageTelehub OnEstateManageTelehub;

        public event EstateRestartSimRequest OnEstateRestartSimRequest;

        public event EstateTeleportAllUsersHomeRequest OnEstateTeleportAllUsersHomeRequest;

        public event EstateTeleportOneUserHomeRequest OnEstateTeleportOneUserHomeRequest;

        public event EventGodDelete OnEventGodDelete;

        public event EventInfoRequest OnEventInfoRequest;

        public event EventNotificationAddRequest OnEventNotificationAddRequest;

        public event EventNotificationRemoveRequest OnEventNotificationRemoveRequest;

        public event FetchInventory OnFetchInventory;

        public event FetchInventoryDescendents OnFetchInventoryDescendents;

        public event FindAgentUpdate OnFindAgent;

        public event ForceReleaseControls OnForceReleaseControls;

        public event GenericMessage OnGenericMessage;

        public event GetScriptRunning OnGetScriptRunning;

        public event GodKickUser OnGodKickUser;

        public event GodlikeMessage onGodlikeMessage;

        public event GodUpdateRegionInfoUpdate OnGodUpdateRegionInfoUpdate;

        public event GrabObject OnGrabObject;

        public event MoveObject OnGrabUpdate;

        public event GrantUserFriendRights OnGrantUserRights;

        public event GroupAccountDetailsRequest OnGroupAccountDetailsRequest;

        public event GroupAccountSummaryRequest OnGroupAccountSummaryRequest;

        public event GroupAccountTransactionsRequest OnGroupAccountTransactionsRequest;

        public event GroupActiveProposalsRequest OnGroupActiveProposalsRequest;

        public event GroupVoteHistoryRequest OnGroupVoteHistoryRequest;

        public event ImprovedInstantMessage OnInstantMessage;

        public event GodLandStatRequest OnLandStatRequest;

        public event LandUndo OnLandUndo;

        public event LinkInventoryItem OnLinkInventoryItem;

        public event LinkObjects OnLinkObjects;

        public event Action<IClientAPI> OnLogout;

        public event MapItemRequest OnMapItemRequest;

        public event RequestMapName OnMapNameRequest;

        public event ModifyTerrain OnModifyTerrain;

        public event MoneyBalanceRequest OnMoneyBalanceRequest;

        public event MoneyTransferRequest OnMoneyTransferRequest;

        public event MoveInventoryFolder OnMoveInventoryFolder;

        public event MoveInventoryItem OnMoveInventoryItem;

        public event MoveTaskInventory OnMoveTaskItem;

        public event MuteListRequest OnMuteListRequest;

        public event UUIDNameRequest OnNameFromUUIDRequest;

        public event NetworkStats OnNetworkStatsUpdate;

        public event ObjectAttach OnObjectAttach;

        public event ObjectBuy OnObjectBuy;

        public event GenericCall7 OnObjectClickAction;

        public event GenericCall7 OnObjectDescription;

        public event ObjectDeselect OnObjectDeselect;

        public event ObjectDeselect OnObjectDetach;

        public event ObjectDrop OnObjectDrop;

        public event ObjectDuplicate OnObjectDuplicate;

        public event ObjectDuplicateOnRay OnObjectDuplicateOnRay;

        public event RequestObjectPropertiesFamily OnObjectGroupRequest;

        public event ObjectIncludeInSearch OnObjectIncludeInSearch;

        public event GenericCall7 OnObjectMaterial;

        public event GenericCall7 OnObjectName;

        public event ObjectOwner OnObjectOwner;

        public event ObjectPermissions OnObjectPermissions;
        public event ObjectRequest OnObjectRequest;

        public event ObjectSaleInfo OnObjectSaleInfo;

        public event ObjectSelect OnObjectSelect;

        public event OfferCallingCard OnOfferCallingCard;

        public event ParcelAbandonRequest OnParcelAbandonRequest;

        public event ParcelAccessListRequest OnParcelAccessListRequest;

        public event ParcelAccessListUpdateRequest OnParcelAccessListUpdateRequest;

        public event ParcelBuy OnParcelBuy;
        public event ParcelBuyPass OnParcelBuyPass;

        public event ParcelDeedToGroup OnParcelDeedToGroup;

        public event ParcelDivideRequest OnParcelDivideRequest;

        public event ParcelDwellRequest OnParcelDwellRequest;

        public event EjectUserUpdate OnParcelEjectUser;

        public event FreezeUserUpdate OnParcelFreezeUser;

        public event ParcelGodForceOwner OnParcelGodForceOwner;

        public event ParcelGodMark OnParcelGodMark;

        public event ParcelInfoRequest OnParcelInfoRequest;

        public event ParcelJoinRequest OnParcelJoinRequest;

        public event ParcelObjectOwnerRequest OnParcelObjectOwnerRequest;

        public event ParcelPropertiesRequest OnParcelPropertiesRequest;

        public event ParcelPropertiesUpdateRequest OnParcelPropertiesUpdateRequest;

        public event ParcelReclaim OnParcelReclaim;

        public event ParcelReturnObjectsRequest OnParcelReturnObjectsRequest;

        public event ParcelSelectObjects OnParcelSelectObjects;

        public event ParcelSetOtherCleanTime OnParcelSetOtherCleanTime;

        public event PickDelete OnPickDelete;

        public event PickGodDelete OnPickGodDelete;

        public event PickInfoUpdate OnPickInfoUpdate;

        public event PlacesQuery OnPlacesQuery;

        public event UpdateAgent OnPreAgentUpdate;

        public event PurgeInventoryDescendents OnPurgeInventoryDescendents;

        public event AgentSit OnRedo;

        public event RegionHandleRequest OnRegionHandleRequest;

        public event Action<IClientAPI> OnRegionHandShakeReply;

        public event RegionInfoRequest OnRegionInfoRequest;

        public event Action<UUID> OnRemoveAvatar;

        public event RemoveInventoryFolder OnRemoveInventoryFolder;

        public event RemoveInventoryItem OnRemoveInventoryItem;

        public event MuteListEntryRemove OnRemoveMuteListEntry;

        public event RemoveTaskInventory OnRemoveTaskItem;

        public event RequestAsset OnRequestAsset;

        public event RequestAvatarProperties OnRequestAvatarProperties;

        public event Action<IClientAPI> OnRequestAvatarsData;

        public event RequestGodlikePowers OnRequestGodlikePowers;

        public event RequestMapBlocks OnRequestMapBlocks;

        public event RequestObjectPropertiesFamily OnRequestObjectPropertiesFamily;

        public event RequestPayPrice OnRequestPayPrice;

        public event RequestTaskInventory OnRequestTaskInventory;

        public event RequestTerrain OnRequestTerrain;

        public event TextureRequest OnRequestTexture;

        public event GenericCall1 OnRequestWearables;

        public event RequestXfer OnRequestXfer;

        public event RetrieveInstantMessages OnRetrieveInstantMessages;

        public event RezMultipleAttachmentsFromInv OnRezMultipleAttachmentsFromInv;

        public event RezObject OnRezObject;
        public event RezScript OnRezScript;

        public event RezSingleAttachmentFromInv OnRezSingleAttachmentFromInv;

        public event SaveStateHandler OnSaveState;

        public event ScriptAnswer OnScriptAnswer;

        public event ScriptReset OnScriptReset;

        public event SendPostcard OnSendPostcard;

        public event SetAlwaysRun OnSetAlwaysRun;

        public event SetAppearance OnSetAppearance;
        public event SetEstateFlagsRequest OnSetEstateFlagsRequest;

        public event SetEstateTerrainBaseTexture OnSetEstateTerrainBaseTexture;

        public event SetEstateTerrainDetailTexture OnSetEstateTerrainDetailTexture;

        public event SetEstateTerrainTextureHeights OnSetEstateTerrainTextureHeights;

        public event SetRegionTerrainSettings OnSetRegionTerrainSettings;

        public event SetScriptRunning OnSetScriptRunning;

        public event TeleportLocationRequest OnSetStartLocationRequest;

        public event SimulatorBlueBoxMessageRequest OnSimulatorBlueBoxMessageRequest;

        public event SimWideDeletesDelegate OnSimWideDeletes;

        public event SoundTrigger OnSoundTrigger;

        public event SpinStart OnSpinStart;

        public event SpinStop OnSpinStop;

        public event SpinObject OnSpinUpdate;

        public event StartAnim OnStartAnim;

        public event StartLure OnStartLure;

        public event StopAnim OnStopAnim;
        public event GenericCall2 OnStopMovement;

        public event TeleportCancel OnTeleportCancel;

        public event UUIDNameRequest OnTeleportHomeRequest;

        public event TeleportLandmarkRequest OnTeleportLandmarkRequest;

        public event TeleportLocationRequest OnTeleportLocationRequest;
        public event TeleportLureRequest OnTeleportLureRequest;

        public event FriendshipTermination OnTerminateFriendship;

        public event TrackAgentUpdate OnTrackAgent;

        public event TerrainUnacked OnUnackedTerrain;

        public event AgentSit OnUndo;

        public event UpdateAvatarProperties OnUpdateAvatarProperties;

        public event UpdateEstateAccessDeltaRequest OnUpdateEstateAccessDeltaRequest;

        public event ObjectExtraParams OnUpdateExtraParams;

        public event UpdateInventoryFolder OnUpdateInventoryFolder;

        public event UpdateInventoryItem OnUpdateInventoryItem;

        public event MuteListEntryUpdate OnUpdateMuteListEntry;

        public event UpdatePrimFlags OnUpdatePrimFlags;

        public event UpdatePrimGroupRotation OnUpdatePrimGroupMouseRotation;

        public event UpdateVector OnUpdatePrimGroupPosition;

        public event UpdatePrimRotation OnUpdatePrimGroupRotation;

        public event UpdateVector OnUpdatePrimGroupScale;

        public event UpdateVector OnUpdatePrimScale;

        public event UpdateShape OnUpdatePrimShape;

        public event UpdateVector OnUpdatePrimSinglePosition;

        public event UpdatePrimSingleRotation OnUpdatePrimSingleRotation;

        public event UpdatePrimSingleRotationPosition OnUpdatePrimSingleRotationPosition;

        public event UpdatePrimTexture OnUpdatePrimTexture;

        public event UpdateTaskInventory OnUpdateTaskInventory;

        public event UpdateUserInfo OnUpdateUserInfo;

        public event RequestTerrain OnUploadTerrain;

        public event UserInfoRequest OnUserInfoRequest;

        public event NewUserReport OnUserReport;

        public event UUIDNameRequest OnUUIDGroupNameRequest;

        public event ViewerEffectEventHandler OnViewerEffect;
        public event XferReceive OnXferReceive;
#pragma warning restore 67

        #endregion Event Definitions IGNORE
        #region Overrriden Methods IGNORE

        private uint m_circuitCode;

        private IPEndPoint m_remoteEndPoint;

        public UUID ActiveGroupId
        {
            get { return UUID.Zero; }
        }

        public string ActiveGroupName
        {
            get { return String.Empty; }
        }

        public ulong ActiveGroupPowers
        {
            get { return 0; }
        }

        public virtual UUID AgentId
        {
            get { return m_uuid; }
        }

        public uint CircuitCode
        {
            get { return m_circuitCode; }
            set
            {
                m_circuitCode = value;
                m_remoteEndPoint = new IPEndPoint(IPAddress.Loopback, (ushort)m_circuitCode);
            }
        }

        public int DebugPacketLevel { get; set; }

        public virtual string FirstName
        {
            get { return m_firstname; }
        }

        public bool IsActive
        {
            get { return true; }
            set { }
        }

        public bool IsLoggingOut
        {
            get { return false; }
            set { }
        }

        public virtual string LastName
        {
            get { return m_lastname; }
        }

        public virtual String Name
        {
            get { return FirstName + " " + LastName; }
        }

        public virtual int NextAnimationSequenceNumber
        {
            get { return 1; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return m_remoteEndPoint; }
        }

        public UUID SecureSessionId
        {
            get { return UUID.Zero; }
        }

        public UUID SessionId
        {
            get { return UUID.Zero; }
        }

        public virtual Vector3 StartPos
        {
            get { return m_startPos; }
            set { }
        }
        public void Close()
        {
            Close(false);
        }

        public void Close(bool force)
        {
            // Remove ourselves from the scene
            m_scene.RemoveClient(AgentId, false);
        }

        public virtual void CrossRegion(ulong newRegionHandle, Vector3 pos, Vector3 lookAt,
                                        IPEndPoint newRegionExternalEndPoint, string capsURL)
        {
        }

        public void FlushPrimUpdates()
        {
        }

        public ClientInfo GetClientInfo()
        {
            return null;
        }

        public ulong GetGroupPowers(UUID groupID)
        {
            return 0;
        }

        public byte[] GetThrottlesPacked(float multiplier)
        {
            return new byte[0];
        }

        public virtual void InformClientOfNeighbour(ulong neighbourHandle, IPEndPoint neighbourExternalEndPoint)
        {
        }

        public void InPacket(object NewPack)
        {
        }

        public bool IsGroupMember(UUID groupID)
        {
            return false;
        }
        public virtual void Kick(string message)
        {
        }

        public virtual void MoveAgentIntoRegion(RegionInfo regInfo, Vector3 pos, Vector3 look)
        {
        }

        public void ProcessInPacket(Packet NewPack)
        {
        }

        public void ReprioritizeUpdates()
        {
        }

        public virtual AgentCircuitData RequestClientInfo()
        {
            return new AgentCircuitData();
        }

        public virtual void SendAbortXferPacket(ulong xferID)
        {
        }

        public void SendAgentAlertMessage(string message, bool modal)
        {
        }

        public virtual void SendAgentDataUpdate(UUID agentid, UUID activegroupid, string firstname, string lastname, ulong grouppowers, string groupname, string grouptitle)
        {
        }

        public void SendAlertMessage(string message)
        {
        }

        public virtual void SendAnimations(UUID[] animations, int[] seqs, UUID sourceAgentId, UUID[] objectIDs)
        {
        }

        public virtual void SendAppearance(UUID agentID, byte[] visualParams, byte[] textureEntry)
        {
        }

        public void SendAsset(AssetRequestToClient req)
        {
        }

        public void SendAssetUploadCompleteMessage(sbyte AssetType, bool Success, UUID AssetFullID)
        {
        }

        public void SendAttachedSoundGainChange(UUID objectID, float gain)
        {
        }

        public void SendAvatarDataImmediate(ISceneEntity avatar)
        {
        }

        public virtual void SendAvatarPickerReply(AvatarPickerReplyAgentDataArgs AgentData, List<AvatarPickerReplyDataArgs> Data)
        {
        }

        public void SendAvatarProperties(UUID avatarID, string aboutText, string bornOn, Byte[] charterMember,
                                         string flAbout, uint flags, UUID flImageID, UUID imageID, string profileURL,
                                         UUID partnerID)
        {
        }

        public void SendBannedUserList(UUID invoice, EstateBan[] banlist, uint estateID)
        {
        }

        public void SendBlueBoxMessage(UUID FromAvatarID, String FromAvatarName, String Message)
        {
        }

        public virtual void SendBulkUpdateInventory(InventoryNodeBase node)
        {
        }

        public void SendCachedTextureResponse(ISceneEntity avatar, int serial, List<CachedTextureResponseArg> cachedTextures)
        {
        }

        public void SendCameraConstraint(Vector4 ConstraintPlane)
        {
        }

        public virtual void SendChatMessage(
            string message, byte type, Vector3 fromPos, string fromName,
            UUID fromAgentID, UUID ownerID, byte source, byte audible)
        {
            ChatToNPC ctn = OnChatToNPC;

            if (ctn != null)
                ctn(message, type, fromPos, fromName, fromAgentID, ownerID, source, audible);
        }

        public virtual void SendCloudData(float[] cloudCover)
        {
        }

        public virtual void SendCoarseLocationUpdate(List<UUID> users, List<Vector3> CoarseLocations)
        {
        }

        public void SendConfirmXfer(ulong xferID, uint PacketID)
        {
        }

        public void SendDetailedEstateData(UUID invoice, string estateName, uint estateID, uint parentEstate, uint estateFlags, uint sunPosition, UUID covenant, uint covenantChanged, string abuseEmail, UUID estateOwner)
        {
        }

        public virtual void SendDialog(string objectname, UUID objectID, UUID ownerID, string ownerFirstName, string ownerLastName, string msg, UUID textureID, int ch, string[] buttonlabels)
        {
        }

        public virtual void SendEconomyData(float EnergyEfficiency, int ObjectCapacity, int ObjectCount, int PriceEnergyUnit,
                                            int PriceGroupCreate, int PriceObjectClaim, float PriceObjectRent, float PriceObjectScaleFactor,
                                            int PriceParcelClaim, float PriceParcelClaimFactor, int PriceParcelRent, int PricePublicObjectDecay,
                                            int PricePublicObjectDelete, int PriceRentLight, int PriceUpload, int TeleportMinPrice, float TeleportPriceExponent)
        {
        }

        public void SendEntityUpdate(ISceneEntity entity, PrimUpdateFlags updateFlags)
        {
        }

        public void SendEstateCovenantInformation(UUID covenant)
        {
        }

        public void SendEstateList(UUID invoice, int code, UUID[] Data, uint estateID)
        {
        }

        public void SendForceClientSelectObjects(List<uint> objectIDs)
        {
        }

        public void SendGenericMessage(string method, UUID invoice, List<string> message)
        {
        }

        public void SendGenericMessage(string method, UUID invoice, List<byte[]> message)
        {
        }

        public void SendGroupNameReply(UUID groupLLUID, string GroupName)
        {
        }

        public void SendHealth(float health)
        {
        }

        public void SendImageFirstPart(ushort numParts, UUID ImageUUID, uint ImageSize, byte[] ImageData, byte imageCodec)
        {
        }

        public void SendImageNextPart(ushort partNumber, UUID imageUuid, byte[] imageData)
        {
        }

        public void SendImageNotFound(UUID imageid)
        {
        }

        public void SendInitiateDownload(string simFileName, string clientFileName)
        {
        }

        public void SendInstantMessage(GridInstantMessage im)
        {
            Action<GridInstantMessage> oimtn = OnInstantMessageToNPC;

            if (oimtn != null)
                oimtn(im);
        }

        public virtual void SendInventoryFolderDetails(UUID ownerID, UUID folderID,
                                                       List<InventoryItemBase> items,
                                                       List<InventoryFolderBase> folders,
                                                       int version,
                                                       bool fetchFolders,
                                                       bool fetchItems)
        {
        }

        public virtual void SendInventoryItemCreateUpdate(InventoryItemBase Item, uint callbackID)
        {
        }

        public virtual void SendInventoryItemDetails(UUID ownerID, InventoryItemBase item)
        {
        }

        public virtual void SendKillObject(List<uint> localID)
        {
        }

        public void SendLandAccessListData(List<LandAccessEntry> accessList, uint accessFlag, int localLandID)
        {
        }

        public void SendLandObjectOwners(LandData land, List<UUID> groups, Dictionary<UUID, int> ownersAndCount)
        {
        }

        public void SendLandParcelOverlay(byte[] data, int sequence_id)
        {
        }

        public void SendLandProperties(int sequence_id, bool snap_selection, int request_result, ILandObject lo, float simObjectBonusFactor, int parcelObjectCapacity, int simObjectCapacity, uint regionFlags)
        {
        }

        public void SendLandStatReply(uint reportType, uint requestFlags, uint resultCount, LandStatReportItem[] lsrpia)
        {
        }

        public virtual void SendLayerData(float[] map)
        {
        }

        public virtual void SendLayerData(int px, int py, float[] map)
        {
        }

        public virtual void SendLayerData(int px, int py, float[] map, bool track)
        {
        }

        public void SendLoadURL(string objectname, UUID objectID, UUID ownerID, bool groupOwned, string message,
                                string url)
        {
        }

        public virtual void SendLocalTeleport(Vector3 position, Vector3 lookAt, uint flags)
        {
        }

        public void SendLogoutPacket()
        {
        }

        public virtual void SendMapBlock(List<MapBlockData> mapBlocks, uint flag)
        {
        }

        public virtual void SendMoneyBalance(UUID transaction, bool success, byte[] description, int balance, int transactionType, UUID sourceID, bool sourceIsGroup, UUID destID, bool destIsGroup, int amount, string item)
        {
        }

        public virtual void SendNameReply(UUID profileId, string firstname, string lastname)
        {
        }

        public void SendObjectPropertiesFamilyData(ISceneEntity Entity, uint RequestFlags)
        {
        }

        public void SendObjectPropertiesReply(ISceneEntity entity)
        {
        }

        public virtual void SendPayPrice(UUID objectID, int[] payPrice)
        {
        }

        public virtual void SendPlayAttachedSound(UUID soundID, UUID objectID, UUID ownerID, float gain,
                                                  byte flags)
        {
        }

        public virtual void SendPreLoadSound(UUID objectID, UUID ownerID, UUID soundID)
        {
        }

        public virtual void SendRegionHandshake(RegionInfo regionInfo, RegionHandshakeArgs args)
        {
            if (OnRegionHandShakeReply != null)
            {
                OnRegionHandShakeReply(this);
            }
        }

        public void SendRegionInfoToEstateMenu(RegionInfoForEstateMenuArgs args)
        {
        }

        public virtual void SendRegionTeleport(ulong regionHandle, byte simAccess, IPEndPoint regionExternalEndPoint,
                                               uint locationID, uint flags, string capsURL)
        {
        }

        public virtual void SendRemoveInventoryItem(UUID itemID)
        {
        }

        public void SendScriptQuestion(UUID objectID, string taskName, string ownerName, UUID itemID, int question)
        {
        }

        public void SendScriptRunningReply(UUID objectID, UUID itemID, bool running)
        {
        }

        public void SendShutdownConnectionNotice()
        {
        }

        public void SendSimStats(SimStats stats)
        {
        }

        public virtual void SendStartPingCheck(byte seq)
        {
        }

        public void SendSunPos(Vector3 sunPos, Vector3 sunVel, ulong time, uint dlen, uint ylen, float phase)
        {
        }

        public void SendSystemAlertMessage(string message)
        {
        }

        public void SendTakeControls(int controls, bool passToAgent, bool TakeControls)
        {
        }

        public virtual void SendTaskInventory(UUID taskID, short serial, byte[] fileName)
        {
        }

        public void SendTelehubInfo(UUID ObjectID, string ObjectName, Vector3 ObjectPos, Quaternion ObjectRot, List<Vector3> SpawnPoint)
        {
        }

        public virtual void SendTeleportFailed(string reason)
        {
        }

        public virtual void SendTeleportProgress(uint flags, string message)
        {
        }

        public virtual void SendTeleportStart(uint flags)
        {
        }

        public void SendTexture(AssetBase TextureAsset)
        {
        }

        public void SendTriggeredSound(UUID soundID, UUID ownerID, UUID objectID, UUID parentID, ulong handle, Vector3 position, float gain)
        {
        }

        public void SendViewerEffect(ViewerEffectPacket.EffectBlock[] effectBlocks)
        {
        }

        public void SendViewerTime(int phase)
        {
        }

        public virtual void SendWearables(AvatarWearable[] wearables, int serial)
        {
        }
        public virtual void SendWindData(Vector2[] windSpeeds)
        {
        }

        public virtual void SendXferPacket(ulong xferID, uint packet, byte[] data)
        {
        }

        public void SendXferRequest(ulong XferID, short AssetType, UUID vFileID, byte FilePath, byte[] FileName)
        {
        }

        public virtual void SetChildAgentThrottle(byte[] throttle)
        {
        }
        public void SetClientInfo(ClientInfo info)
        {
        }

        public void Start()
        {
            // We never start the client, so always fail.
            throw new NotImplementedException();
        }

        public void Stop()
        {
        }
        public void Terminate()
        {
        }
        #endregion Overrriden Methods IGNORE
        #region IClientAPI Members

        public bool AddGenericPacketHandler(string MethodName, GenericMessage handler)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void RefreshGroupMembership()
        {
        }

        public void SendAgentDropGroup(UUID groupID)
        {
        }

        public void SendAvatarClassifiedReply(UUID targetID, UUID[] classifiedID, string[] name)
        {
        }

        public void SendAvatarClassifiedReply(UUID targetID, Dictionary<UUID, string> classifieds)
        {
        }

        public void SendAvatarNotesReply(UUID targetID, string text)
        {
        }

        public void SendAvatarPicksReply(UUID targetID, Dictionary<UUID, string> picks)
        {
        }

        public void SendClassifiedInfoReply(UUID classifiedID, UUID creatorID, uint creationDate, uint expirationDate, uint category, string name, string description, UUID parcelID, uint parentEstate, UUID snapshotID, string simName, Vector3 globalPos, string parcelName, byte classifiedFlags, int price)
        {
        }
        public void SendCreateGroupReply(UUID groupID, bool success, string message)
        {
        }

        public void SendMuteListUpdate(string filename)
        {
        }

        public void SendParcelDwellReply(int localID, UUID parcelID, float dwell)
        {
        }

        public void SendPickInfoReply(UUID pickID, UUID creatorID, bool topPick, UUID parcelID, string name, string desc, UUID snapshotID, string user, string originalName, string simName, Vector3 posGlobal, int sortOrder, bool enabled)
        {
        }

        public void SendUseCachedMuteList()
        {
        }

        public void SendUserInfoReply(bool imViaEmail, bool visible, string email)
        {
        }
        #endregion IClientAPI Members
    }
}