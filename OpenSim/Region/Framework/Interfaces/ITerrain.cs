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
using OpenSim.Framework;

namespace OpenSim.Region.Framework.Interfaces
{
    public interface IMapImageGenerator
    {
        System.Drawing.Bitmap CreateMapTile();

        System.Drawing.Bitmap CreateViewImage(Vector3 camPos, Vector3 camDir, float fov, int width, int height, bool useTextures);

        byte[] WriteJpeg2000Image();
    }

    public interface ITerrain
    {
        void CheckHeightValues();

        void ExportImage(string filename, string gradientmap);

        byte[] ExportJpegImage(string gradientmap);

        void FlattenTerrain(double rx, double ry, double size, double amount);

        double GetHeight(int x, int y);

        float[] GetHeights1D();

        void GetHeights1D(float[] heights);

        float[,] GetHeights2D();

        double[,] GetHeights2DD();

        void HillsGenerator();

        void LoadFromFileF32(string filename);

        void LoadFromFileF32(string filename, int dimensionX, int dimensionY, int lowerboundX, int lowerboundY);

        void LoadFromFileF64(string filename);

        void LoadFromFileIMG(string filename, int dimensionX, int dimensionY, int lowerboundX, int lowerboundY);

        void LoadFromFileSLRAW(string filename);

        void LowerTerrain(double rx, double ry, double size, double amount);

        void ModifyTerrain(float height, float seconds, byte brushsize, byte action, float north, float west,
                           IClientAPI remoteUser);

        void NoiseTerrain(double rx, double ry, double size, double amount);

        void RaiseTerrain(double rx, double ry, double size, double amount);

        void ResetTaint();

        void RevertTerrain(double rx, double ry, double size, double amount);

        bool RunTerrainCmd(string[] args, ref string resultText, string simName);

        void SaveRevertMap();

        void SetHeights2D(float[,] heights);

        void SetHeights2D(double[,] heights);

        void SetRange(float min, float max);

        void SetSeed(int val);

        void SmoothTerrain(double rx, double ry, double size, double amount);

        void SwapRevertMaps();

        bool Tainted();

        bool Tainted(int x, int y);
        void WriteToFileF32(string filename);

        void WriteToFileF64(string filename);
        void WriteToFileHiRAW(string filename);

        void WriteToFileRAW(string filename);
    }
}