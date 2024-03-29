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
using OpenMetaverse.Assets;
using OpenSim.Framework;
using OpenSim.Region.Framework.Scenes;

namespace OpenSim.Tests.Common
{
    /// <summary>
    /// Utility functions for carrying out task inventory tests.
    /// </summary>
    ///
    public static class TaskInventoryHelpers
    {
        /// <summary>
        /// Add a notecard item to the given part.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="part"></param>
        /// <param name="itemName"></param>
        /// <param name="itemIDFrag">UUID or UUID stem</param>
        /// <param name="assetIDFrag">UUID or UUID stem</param>
        /// <param name="text">The tex to put in the notecard.</param>
        /// <returns>The item that was added</returns>
        public static TaskInventoryItem AddNotecard(
            Scene scene, SceneObjectPart part, string itemName, string itemIDStem, string assetIDStem, string text)
        {
            return AddNotecard(
                scene, part, itemName, TestHelpers.ParseStem(itemIDStem), TestHelpers.ParseStem(assetIDStem), text);
        }

        /// <summary>
        /// Add a notecard item to the given part.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="part"></param>
        /// <param name="itemName"></param>
        /// <param name="itemID"></param>
        /// <param name="assetID"></param>
        /// <param name="text">The tex to put in the notecard.</param>
        /// <returns>The item that was added</returns>
        public static TaskInventoryItem AddNotecard(
            Scene scene, SceneObjectPart part, string itemName, UUID itemID, UUID assetID, string text)
        {
            AssetNotecard nc = new AssetNotecard();
            nc.BodyText = text;
            nc.Encode();

            AssetBase ncAsset
                = AssetHelpers.CreateAsset(assetID, AssetType.Notecard, nc.AssetData, UUID.Zero);
            scene.AssetService.Store(ncAsset);

            TaskInventoryItem ncItem
                = new TaskInventoryItem
                    {
                        Name = itemName,
                        AssetID = assetID,
                        ItemID = itemID,
                        Type = (int)AssetType.Notecard,
                        InvType = (int)InventoryType.Notecard
                    };
            part.Inventory.AddInventoryItem(ncItem, true);

            return ncItem;
        }

        /// <summary>
        /// Add a simple script to the given part.
        /// </summary>
        /// <remarks>
        /// TODO: Accept input for item and asset IDs to avoid mysterious script failures that try to use any of these
        /// functions more than once in a test.
        /// </remarks>
        /// <param name="scene"></param>
        /// <param name="part"></param>
        /// <returns>The item that was added</returns>
        public static TaskInventoryItem AddScript(Scene scene, SceneObjectPart part)
        {
            return AddScript(scene, part, "scriptItem", "default { state_entry() { llSay(0, \"Hello World\"); } }");
        }

        /// <summary>
        /// Add a simple script to the given part.
        /// </summary>
        /// <remarks>
        /// TODO: Accept input for item and asset IDs so that we have completely replicatable regression tests rather
        /// than a random component.
        /// </remarks>
        /// <param name="scene"></param>
        /// <param name="part"></param>
        /// <param name="scriptName">Name of the script to add</param>
        /// <param name="scriptSource">LSL script source</param>
        /// <returns>The item that was added</returns>
        public static TaskInventoryItem AddScript(
            Scene scene, SceneObjectPart part, string scriptName, string scriptSource)
        {
            AssetScriptText ast = new AssetScriptText();
            ast.Source = scriptSource;
            ast.Encode();

            UUID assetUuid = UUID.Random();
            UUID itemUuid = UUID.Random();

            AssetBase asset
                = AssetHelpers.CreateAsset(assetUuid, AssetType.LSLText, ast.AssetData, UUID.Zero);
            scene.AssetService.Store(asset);
            TaskInventoryItem item
                = new TaskInventoryItem
                    {
                        Name = scriptName,
                        AssetID = assetUuid,
                        ItemID = itemUuid,
                        Type = (int)AssetType.LSLText,
                        InvType = (int)InventoryType.LSL
                    };
            part.Inventory.AddInventoryItem(item, true);

            return item;
        }

        /// <summary>
        /// Add a scene object item to the given part.
        /// </summary>
        /// <remarks>
        /// TODO: Accept input for item and asset IDs to avoid mysterious script failures that try to use any of these
        /// functions more than once in a test.
        /// </remarks>
        ///
        /// <param name="scene"></param>
        /// <param name="sop"></param>
        /// <param name="itemName"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        public static TaskInventoryItem AddSceneObject(
            Scene scene, SceneObjectPart sop, string itemName, UUID id, UUID userId)
        {
            SceneObjectGroup taskSceneObject = SceneHelpers.CreateSceneObject(1, UUID.Zero);
            AssetBase taskSceneObjectAsset = AssetHelpers.CreateAsset(0x10, taskSceneObject);
            scene.AssetService.Store(taskSceneObjectAsset);
            TaskInventoryItem taskSceneObjectItem
                = new TaskInventoryItem
                    {
                        Name = itemName,
                        AssetID = taskSceneObjectAsset.FullID,
                        ItemID = id,
                        OwnerID = userId,
                        Type = (int)AssetType.Object,
                        InvType = (int)InventoryType.Object
                    };
            sop.Inventory.AddInventoryItem(taskSceneObjectItem, true);

            return taskSceneObjectItem;
        }
    }
}