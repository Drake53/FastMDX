﻿using LT = FastMDX.TextureAnimation;

namespace FastMDX {
    using static OptionalBlocks;
    using Transforms = System.Collections.Generic.Dictionary<OptionalBlocks, IOptionalBlocksParser<LT>>;

    public unsafe struct TextureAnimation : IDataRW {
        public Transform<Vec3> Translation, Scaling;
        public Transform<Vec4> Rotation;

        void IDataRW.ReadFrom(DataStream ds) {
            var end = ds.Offset + ds.ReadStruct<uint>();
            ds.ReadOptionalBlocks(ref this, _knownTransforms, end);
        }

        void IDataRW.WriteTo(DataStream ds) {
            var offset = ds.Offset;
            ds.Skip(sizeof(uint));
            ds.WriteOptionalBlocks(ref this, _knownTransforms);
            ds.SetValueAt(offset, ds.Offset - offset);
        }

        static readonly Transforms _knownTransforms = new Transforms {
            [KTAT] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.Translation),
            [KTAR] = new OptionalBlockParser<Transform<Vec4>, LT>((ref LT p) => ref p.Rotation),
            [KTAS] = new OptionalBlockParser<Transform<Vec3>, LT>((ref LT p) => ref p.Scaling),
        };
    }
}
