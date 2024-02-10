using System;
using Unity.Netcode;

namespace Player
{
    public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
    {
        public ulong ClientId;
        public int ColorIndex;

        public bool Equals(PlayerData other)
        {
            return ClientId == other.ClientId && ColorIndex == other.ColorIndex;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref ColorIndex);
        }
    }
}