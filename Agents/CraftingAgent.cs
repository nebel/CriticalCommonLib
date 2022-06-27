using System;
using System.Linq;
using System.Reflection;
using CriticalCommonLib.Crafting;
using CriticalCommonLib.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.GeneratedSheets;

namespace CriticalCommonLib.Agents
{
    //Not really an agent but seems a good as spot as any
    public unsafe class CraftingAgent
    {
        private const int OffsetCraftingAgent = 352;

        private const int OffsetStep       = 248;
        private const int OffsetStatus     = 200;
        private const int OffsetDurability = 120;
        private const int OffsetHqChance   = 168;
        private const int OffsetQuality    = 152;
        private const int OffsetProgress   = 88;
        private const int OffsetResultItemId   = 264;

        public  AddonSynthesis* Pointer;
        private byte*           _agent;


        public static implicit operator CraftingAgent(IntPtr ptr)
        {
            var ret = new CraftingAgent { Pointer = (AddonSynthesis*)ptr };
            if (ret)
                ret._agent = *(byte**)((byte*)ret.Pointer + OffsetCraftingAgent);

            return ret;
        }

        public static implicit operator bool(CraftingAgent ptr)
            => ptr.Pointer != null;


        public int Step
            => _agent == null ? 0 : *(int*)(_agent + OffsetStep);

        public CraftStatus Status
            => _agent == null ? 0 : *(CraftStatus*)(_agent + OffsetStatus);

        public int Durability
            => _agent == null ? 0 : *(int*)(_agent + OffsetDurability);

        public int HqChance
            => _agent == null ? 0 : *(int*)(_agent + OffsetHqChance);

        public int Quality
            => _agent == null ? 0 : *(int*)(_agent + OffsetQuality);

        public int Progress
            => _agent == null ? 0 : *(int*)(_agent + OffsetProgress);

        public uint ResultItemId
            => _agent == null ? 0 : *(uint*)(_agent + OffsetResultItemId);

        public uint CraftType => (uint) (Service.ClientState.LocalPlayer?.ClassJob.GameData?.DohDolJobIndex ?? 0);
        
        public uint Recipe
        {
            get
            {
                if (ExcelCache.GetSheet<Recipe>()
                    .Any(c => c.CraftType.Row == CraftType && c.ItemResult.Row == ResultItemId))
                {
                    return ExcelCache.GetSheet<Recipe>()
                        .Single(c => c.CraftType.Row == CraftType && c.ItemResult.Row == ResultItemId).RowId;
                }

                return 0;
            }
        }
    }
}